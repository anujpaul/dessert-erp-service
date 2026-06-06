using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.DataManagement.DTOs;
using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.DataManagement;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;
using PMProduct = DessertERP.Domain.Modules.ProductManagement.Product;

namespace DessertERP.Application.Modules.DataManagement.Services;

// ──────────────────────────────────────────────────────────────────────────────
// Interface
// ──────────────────────────────────────────────────────────────────────────────

public interface IDataManagementService
{
    // Job CRUD
    Task<ImportJobDto> CreateImportJobAsync(Guid orgId, string entityType, string fileFormat,
        string fileName, string filePath, string triggeredBy = "upload", CancellationToken ct = default);

    Task<List<ImportJobDto>> GetImportJobsAsync(Guid orgId, CancellationToken ct = default);
    Task<ImportJobDto?> GetImportJobAsync(Guid jobId, CancellationToken ct = default);
    Task<List<ImportJobRowDto>> GetStagedRowsAsync(Guid jobId, int page = 1, int pageSize = 100, CancellationToken ct = default);

    // Stage → Validate → Promote (three explicit phases; ProcessImportJobAsync runs all three)
    Task StageAsync(Guid jobId, CancellationToken ct = default);
    Task ValidateAsync(Guid jobId, CancellationToken ct = default);
    Task PromoteAsync(Guid jobId, CancellationToken ct = default);
    Task<ImportResult> ProcessImportJobAsync(Guid jobId, CancellationToken ct = default);

    /// <summary>Auto-confirm all Draft sales orders created by the given import job.</summary>
    Task<int> AutoConfirmSalesOrdersAsync(Guid importJobId, CancellationToken ct = default);

    // Export (all records)
    Task<(byte[] Data, string FileName, string ContentType)> ExportAsync(
        Guid orgId, string entityType, string fileFormat, CancellationToken ct = default);

    /// <summary>
    /// Export only records where IsExported=false, stamp them, return entity IDs + blob bytes.
    /// </summary>
    Task<ExportUnexportedResult> ExportUnexportedAsync(
        Guid orgId, string entityType, string fileFormat, CancellationToken ct = default);

    /// <summary>Reset IsExported=false on the given entity IDs so they are picked up next run.</summary>
    Task<int> ResetExportAsync(Guid orgId, string entityType, IEnumerable<Guid> entityIds, CancellationToken ct = default);

    // Templates
    (byte[] Data, string FileName, string ContentType) GetTemplate(string entityType, string fileFormat);
}

// ──────────────────────────────────────────────────────────────────────────────
// Implementation
// ──────────────────────────────────────────────────────────────────────────────

public class DataManagementService : IDataManagementService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    private static readonly JsonSerializerOptions _jsonOpts =
        new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

    public DataManagementService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db  = db;
        _org = org;
    }

    // ── Job management ─────────────────────────────────────────────────────────

    public async Task<ImportJobDto> CreateImportJobAsync(Guid orgId, string entityType, string fileFormat,
        string fileName, string filePath, string triggeredBy = "upload", CancellationToken ct = default)
    {
        var et  = Enum.Parse<EntityType>(entityType, true);
        var ff  = Enum.Parse<FileFormat>(fileFormat, true);
        var job = new ImportJob(orgId, et, ff, fileName, filePath, triggeredBy);
        _db.ImportJobs.Add(job);
        await _db.SaveChangesAsync(ct);
        return ToDto(job, 0, 0, 0, 0);
    }

    public async Task<List<ImportJobDto>> GetImportJobsAsync(Guid orgId, CancellationToken ct = default)
    {
        var jobs = await _db.ImportJobs
            .Where(j => j.OrganizationId == orgId)
            .OrderByDescending(j => j.CreatedAt)
            .Take(100)
            .ToListAsync(ct);

        // Aggregate staging stats per job
        var jobIds = jobs.Select(j => j.Id).ToList();
        var stagingStats = await _db.ImportJobRows
            .Where(r => jobIds.Contains(r.ImportJobId))
            .GroupBy(r => new { r.ImportJobId, r.Status })
            .Select(g => new { g.Key.ImportJobId, g.Key.Status, Count = g.Count() })
            .ToListAsync(ct);

        return jobs.Select(j =>
        {
            var stats = stagingStats.Where(s => s.ImportJobId == j.Id).ToList();
            return ToDto(j,
                stats.Where(s => s.Status == RowStatus.Valid).Sum(s => s.Count),
                stats.Where(s => s.Status == RowStatus.Invalid).Sum(s => s.Count),
                stats.Where(s => s.Status == RowStatus.Promoted).Sum(s => s.Count),
                stats.Sum(s => s.Count));
        }).ToList();
    }

    public async Task<ImportJobDto?> GetImportJobAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _db.ImportJobs.FirstOrDefaultAsync(j => j.Id == jobId, ct);
        if (job is null) return null;
        var stats = await GetRowStats(jobId, ct);
        return ToDto(job, stats.valid, stats.invalid, stats.promoted, stats.total);
    }

    public async Task<List<ImportJobRowDto>> GetStagedRowsAsync(
        Guid jobId, int page = 1, int pageSize = 100, CancellationToken ct = default)
    {
        var rows = await _db.ImportJobRows
            .Where(r => r.ImportJobId == jobId)
            .OrderBy(r => r.RowNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return rows.Select(r => new ImportJobRowDto(
            r.Id, r.RowNumber, r.Status.ToString(), r.ErrorMessage, r.PromotedEntityId, r.PromotedAt
        )).ToList();
    }

    // ── Full pipeline ──────────────────────────────────────────────────────────

    public async Task<ImportResult> ProcessImportJobAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _db.ImportJobs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Import job not found.");

        job.Start();
        await _db.SaveChangesAsync(ct);

        try
        {
            await StageAsync(jobId, ct);
            await ValidateAsync(jobId, ct);
            await PromoteAsync(jobId, ct);
        }
        catch (Exception ex)
        {
            job = await _db.ImportJobs.FirstOrDefaultAsync(j => j.Id == jobId, ct)!;
            job!.Fail(ex.Message);
            await _db.SaveChangesAsync(ct);
        }

        // Build result summary
        var (valid, invalid, promoted, total) = await GetRowStats(jobId, ct);
        var failedRows = await _db.ImportJobRows
            .Where(r => r.ImportJobId == jobId && r.Status == RowStatus.Invalid)
            .Select(r => new { r.RowNumber, r.ErrorMessage })
            .Take(20)
            .ToListAsync(ct);

        var result = new ImportResult
        {
            TotalRows   = total,
            SuccessRows = promoted,
            RowResults  = failedRows.Select(r => new RowResult
            {
                Row     = r.RowNumber,
                Success = false,
                Error   = r.ErrorMessage
            }).ToList()
        };
        return result;
    }

    // ── Phase 1: Stage ─────────────────────────────────────────────────────────
    // Parse the file and insert one ImportJobRow per data row.
    // No business logic — just raw payload as JSON dict.

    public async Task StageAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _db.ImportJobs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Import job not found.");

        var rawRows = ParseFileToRows(job);
        int rowNum  = 1;
        foreach (var dict in rawRows)
        {
            var payload = JsonSerializer.Serialize(dict);
            _db.ImportJobRows.Add(new ImportJobRow(jobId, rowNum++, payload));
        }

        job.MarkStaged(rawRows.Count);
        await _db.SaveChangesAsync(ct);
    }

    // ── Phase 2: Validate ──────────────────────────────────────────────────────
    // Load reference data, then validate each Pending staging row.
    // Marks each row Valid or Invalid (with reason).

    public async Task ValidateAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _db.ImportJobs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Import job not found.");

        var rows = await _db.ImportJobRows
            .Where(r => r.ImportJobId == jobId && r.Status == RowStatus.Pending)
            .ToListAsync(ct);

        if (!rows.Any()) return;

        // Load reference data once for the whole batch
        var orgId = job.OrganizationId;

        switch (job.EntityType)
        {
            case EntityType.Vendor:
                var existingVendors = (await _db.Vendors
                    .Where(v => v.OrganizationId == orgId)
                    .Select(v => v.VendorNumber)
                    .ToListAsync(ct)).ToHashSet();
                foreach (var row in rows)
                    ValidateVendorRow(row, existingVendors);
                break;

            case EntityType.Product:
                var categories = await _db.Categories
                    .Where(c => c.OrganizationId == orgId)
                    .ToDictionaryAsync(c => c.Code, c => c.Id, ct);
                var brands = await _db.Brands
                    .Where(b => b.OrganizationId == orgId)
                    .ToDictionaryAsync(b => b.Code, b => b.Id, ct);
                var existingSkus = (await _db.CatalogProducts
                    .Where(p => p.OrganizationId == orgId)
                    .Select(p => p.Sku)
                    .ToListAsync(ct)).ToHashSet();
                foreach (var row in rows)
                    ValidateProductRow(row, categories, brands, existingSkus);
                break;

            case EntityType.SalesOrder:
                var customers = (await _db.Customers
                    .Where(c => c.OrganizationId == orgId)
                    .Select(c => c.CustomerNumber)
                    .ToListAsync(ct)).ToHashSet();
                var soVariants = (await _db.ProductVariants
                    .Where(v => v.OrganizationId == orgId)
                    .Select(v => v.Sku)
                    .ToListAsync(ct)).ToHashSet();
                foreach (var row in rows)
                    ValidateSalesOrderRow(row, customers, soVariants);
                break;

            case EntityType.PurchaseOrder:
                var vendors = (await _db.Vendors
                    .Where(v => v.OrganizationId == orgId)
                    .Select(v => v.VendorNumber)
                    .ToListAsync(ct)).ToHashSet();
                var poVariants = (await _db.ProductVariants
                    .IgnoreQueryFilters()
                    .Where(v => v.OrganizationId == orgId)
                    .Select(v => v.Sku)
                    .ToListAsync(ct)).ToHashSet();
                foreach (var row in rows)
                    ValidatePurchaseOrderRow(row, vendors, poVariants);
                break;
        }

        await _db.SaveChangesAsync(ct);
    }

    private static void ValidateVendorRow(ImportJobRow row, HashSet<string> existing)
    {
        try
        {
            var d = ParsePayload(row.RawPayload);
            var vn = Get(d, VendorFields.VendorNumber);
            if (string.IsNullOrWhiteSpace(vn))   throw new Exception("VendorNumber is required.");
            if (string.IsNullOrWhiteSpace(Get(d, VendorFields.Name))) throw new Exception("Name is required.");
            if (existing.Contains(vn))             throw new Exception($"Vendor '{vn}' already exists.");
            existing.Add(vn); // prevent duplicate in same batch
            row.MarkValid();
        }
        catch (Exception ex) { row.MarkInvalid(ex.Message); }
    }

    private static void ValidateProductRow(ImportJobRow row,
        Dictionary<string, Guid> categories, Dictionary<string, Guid> brands, HashSet<string> existingSkus)
    {
        try
        {
            var d   = ParsePayload(row.RawPayload);
            var sku = Get(d, ProductFields.Sku);
            if (string.IsNullOrWhiteSpace(sku))  throw new Exception("Sku is required.");
            if (string.IsNullOrWhiteSpace(Get(d, ProductFields.Name))) throw new Exception("Name is required.");
            if (existingSkus.Contains(sku))       throw new Exception($"SKU '{sku}' already exists.");
            var catCode = Get(d, ProductFields.CategoryCode);
            if (string.IsNullOrWhiteSpace(catCode)) throw new Exception("CategoryCode is required.");
            if (!categories.ContainsKey(catCode)) throw new Exception($"Category '{catCode}' not found.");
            var brandCode = Get(d, ProductFields.BrandCode);
            if (!string.IsNullOrWhiteSpace(brandCode) && !brands.ContainsKey(brandCode))
                throw new Exception($"Brand '{brandCode}' not found.");
            existingSkus.Add(sku);
            row.MarkValid();
        }
        catch (Exception ex) { row.MarkInvalid(ex.Message); }
    }

    private static void ValidateSalesOrderRow(ImportJobRow row,
        HashSet<string> customers, HashSet<string> variants)
    {
        try
        {
            var d  = ParsePayload(row.RawPayload);
            var cn = Get(d, SalesOrderFields.CustomerNumber);
            if (string.IsNullOrWhiteSpace(cn)) throw new Exception("CustomerNumber is required.");
            if (!customers.Contains(cn))        throw new Exception($"Customer '{cn}' not found.");
            var od = Get(d, SalesOrderFields.OrderDate);
            if (!DateTime.TryParse(od, out _)) throw new Exception($"Invalid OrderDate '{od}'.");
            var vSku = Get(d, SalesOrderFields.VariantSku);
            if (string.IsNullOrWhiteSpace(vSku)) throw new Exception("VariantSku is required.");
            if (!variants.Contains(vSku))         throw new Exception($"Variant SKU '{vSku}' not found.");
            if (!decimal.TryParse(Get(d, SalesOrderFields.Quantity), NumberStyles.Any, CultureInfo.InvariantCulture, out var qty) || qty <= 0)
                throw new Exception("Quantity must be a positive number.");
            row.MarkValid();
        }
        catch (Exception ex) { row.MarkInvalid(ex.Message); }
    }

    private static void ValidatePurchaseOrderRow(ImportJobRow row,
        HashSet<string> vendors, HashSet<string> variants)
    {
        try
        {
            var d  = ParsePayload(row.RawPayload);
            var vn = Get(d, PurchaseOrderFields.VendorNumber);
            if (string.IsNullOrWhiteSpace(vn)) throw new Exception("VendorNumber is required.");
            if (!vendors.Contains(vn))          throw new Exception($"Vendor '{vn}' not found.");
            var od = Get(d, PurchaseOrderFields.OrderDate);
            if (!DateTime.TryParse(od, out _)) throw new Exception($"Invalid OrderDate '{od}'.");
            var vSku = Get(d, PurchaseOrderFields.VariantSku);
            if (string.IsNullOrWhiteSpace(vSku)) throw new Exception("VariantSku is required.");
            if (!variants.Contains(vSku))         throw new Exception($"Variant SKU '{vSku}' not found.");
            row.MarkValid();
        }
        catch (Exception ex) { row.MarkInvalid(ex.Message); }
    }

    // ── Phase 3: Promote ───────────────────────────────────────────────────────
    // Move Valid staging rows into real domain tables.

    public async Task PromoteAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _db.ImportJobs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Import job not found.");

        var rows = await _db.ImportJobRows
            .Where(r => r.ImportJobId == jobId && r.Status == RowStatus.Valid)
            .OrderBy(r => r.RowNumber)
            .ToListAsync(ct);

        if (!rows.Any()) return;

        var orgId = job.OrganizationId;
        int success = 0, failed = 0;

        switch (job.EntityType)
        {
            case EntityType.Vendor:
                foreach (var row in rows)
                    (success, failed) = await PromoteVendorRow(row, orgId, success, failed, ct);
                break;

            case EntityType.Product:
                var cats   = await _db.Categories.Where(c => c.OrganizationId == orgId).ToDictionaryAsync(c => c.Code, c => c.Id, ct);
                var brnds  = await _db.Brands.Where(b => b.OrganizationId == orgId).ToDictionaryAsync(b => b.Code, b => b.Id, ct);
                foreach (var row in rows)
                    (success, failed) = await PromoteProductRow(row, orgId, cats, brnds, success, failed, ct);
                break;

            case EntityType.SalesOrder:
                var custMap    = await _db.Customers.Where(c => c.OrganizationId == orgId).ToDictionaryAsync(c => c.CustomerNumber, c => c, ct);
                var variantMap = await _db.ProductVariants.Where(v => v.OrganizationId == orgId).ToDictionaryAsync(v => v.Sku, v => v, ct);
                var prodMap    = await _db.CatalogProducts.Where(p => p.OrganizationId == orgId).ToDictionaryAsync(p => p.Id, p => p, ct);
                (success, failed) = await PromoteSalesOrderRows(rows, orgId, custMap, variantMap, prodMap, success, failed, ct);
                break;

            case EntityType.PurchaseOrder:
                var vendMap = await _db.Vendors.Where(v => v.OrganizationId == orgId).ToDictionaryAsync(v => v.VendorNumber, v => v, ct);
                var poVars  = await _db.ProductVariants.IgnoreQueryFilters().Where(v => v.OrganizationId == orgId).ToDictionaryAsync(v => v.Sku, v => v, ct);
                (success, failed) = await PromotePurchaseOrderRows(rows, orgId, vendMap, poVars, success, failed, ct);
                break;
        }

        var (valid, invalid, promoted, total) = await GetRowStats(jobId, ct);
        job.Complete(total, promoted, invalid,
            invalid == 0 ? null : $"{invalid} row(s) failed validation — see row details.");
        await _db.SaveChangesAsync(ct);
    }

    // ── Vendor promotion ───────────────────────────────────────────────────────

    private async Task<(int success, int failed)> PromoteVendorRow(
        ImportJobRow row, Guid orgId, int success, int failed, CancellationToken ct)
    {
        try
        {
            var d = ParsePayload(row.RawPayload);
            var vendor = new Vendor(orgId,
                Get(d, VendorFields.VendorNumber) ?? "",
                Get(d, VendorFields.Name) ?? "",
                Get(d, VendorFields.Email),
                Get(d, VendorFields.Phone),
                BuildAddress(d),
                GetOr(d, VendorFields.Currency, "USD"),
                GetInt(d, VendorFields.PaymentTermsDays, 30),
                Get(d, VendorFields.TaxId));
            vendor.Update(
                Get(d, VendorFields.Name) ?? "",
                Get(d, VendorFields.Email),
                Get(d, VendorFields.Phone),
                BuildAddress(d),
                GetInt(d, VendorFields.PaymentTermsDays, 30),
                Get(d, VendorFields.BankAccountName),
                Get(d, VendorFields.BankAccountNumber));
            _db.Vendors.Add(vendor);
            await _db.SaveChangesAsync(ct);
            row.MarkPromoted(vendor.Id);
            await _db.SaveChangesAsync(ct);
            return (success + 1, failed);
        }
        catch (Exception ex)
        {
            row.MarkInvalid($"Promote failed: {ex.Message}");
            await _db.SaveChangesAsync(ct);
            return (success, failed + 1);
        }
    }

    // ── Product promotion ──────────────────────────────────────────────────────

    private async Task<(int success, int failed)> PromoteProductRow(
        ImportJobRow row, Guid orgId,
        Dictionary<string, Guid> categories, Dictionary<string, Guid> brands,
        int success, int failed, CancellationToken ct)
    {
        try
        {
            var d  = ParsePayload(row.RawPayload);
            categories.TryGetValue(Get(d, ProductFields.CategoryCode) ?? "", out var catId);
            Guid? brandId = null;
            var bc = Get(d, ProductFields.BrandCode);
            if (!string.IsNullOrWhiteSpace(bc) && brands.TryGetValue(bc, out var bid)) brandId = bid;

            if (!Enum.TryParse<ProductType>(GetOr(d, ProductFields.ProductType, "Other"), true, out var pt))  pt = ProductType.Other;
            if (!Enum.TryParse<GenderTarget>(GetOr(d, ProductFields.GenderTarget, "Unisex"), true, out var gt)) gt = GenderTarget.Unisex;

            var product = new PMProduct(orgId,
                Get(d, ProductFields.Sku)!,
                Get(d, ProductFields.Name)!,
                catId, pt,
                GetDecimal(d, ProductFields.BasePrice),
                GetDecimal(d, ProductFields.BaseCost),
                GetOr(d, ProductFields.UnitOfMeasure, "Each"),
                brandId, gt,
                Get(d, ProductFields.Description),
                Get(d, ProductFields.Tags),
                GetOr(d, ProductFields.Currency, "USD"),
                GetDecimalN(d, ProductFields.TaxRate));  // optional per-product override; null = inherit from category

            _db.CatalogProducts.Add(product);

            // Optional variant
            var size = Get(d, ProductFields.VariantSize);
            if (!string.IsNullOrWhiteSpace(size))
                product.AddVariant(size,
                    Get(d, ProductFields.VariantColor),
                    Get(d, ProductFields.VariantMaterial),
                    Get(d, ProductFields.VariantBarcode),
                    GetDecimalN(d, ProductFields.VariantPriceOverride),
                    GetDecimalN(d, ProductFields.VariantCostOverride));

            await _db.SaveChangesAsync(ct);
            row.MarkPromoted(product.Id);
            await _db.SaveChangesAsync(ct);
            return (success + 1, failed);
        }
        catch (Exception ex)
        {
            row.MarkInvalid($"Promote failed: {ex.Message}");
            await _db.SaveChangesAsync(ct);
            return (success, failed + 1);
        }
    }

    // ── Sales Order promotion (grouped by CustomerNumber|OrderDate|CustomerRef) ─

    private async Task<(int success, int failed)> PromoteSalesOrderRows(
        List<ImportJobRow> rows, Guid orgId,
        Dictionary<string, Customer> customers,
        Dictionary<string, ProductVariant> variants,
        Dictionary<Guid, PMProduct> products,
        int success, int failed, CancellationToken ct)
    {
        var groups = rows
            .Select(r => (Row: r, Data: ParsePayload(r.RawPayload)))
            .GroupBy(x => $"{Get(x.Data, SalesOrderFields.CustomerNumber)}|{Get(x.Data, SalesOrderFields.OrderDate)}|{Get(x.Data, SalesOrderFields.CustomerRef)}");

        foreach (var group in groups)
        {
            var firstRow  = group.First();
            var firstData = firstRow.Data;
            try
            {
                if (!customers.TryGetValue(Get(firstData, SalesOrderFields.CustomerNumber) ?? "", out var customer))
                    throw new Exception($"Customer not found.");

                DateTime.TryParse(Get(firstData, SalesOrderFields.OrderDate), out var orderDate);
                DateTime? shipDate = null;
                if (DateTime.TryParse(Get(firstData, SalesOrderFields.RequestedShipDate), out var sd)) shipDate = sd;

                var orderCount  = await _db.SalesOrders.CountAsync(ct);
                var orderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{(orderCount + 1):D4}";

                var order = new SalesOrder(orgId, orderNumber, customer.Id, orderDate,
                    Get(firstData, SalesOrderFields.Description) ?? "",
                    Get(firstData, SalesOrderFields.CustomerRef) ?? "",
                    GetOr(firstData, SalesOrderFields.Currency, "USD"), shipDate);

                foreach (var (row, lineData) in group)
                {
                    var sku = Get(lineData, SalesOrderFields.VariantSku) ?? "";
                    if (!variants.TryGetValue(sku, out var variant)) throw new Exception($"Variant '{sku}' not found.");
                    products.TryGetValue(variant.ProductId, out var prod);
                    order.AddLine(variant.Id, sku, prod?.Name ?? sku, null,
                        prod?.UnitOfMeasure ?? "Each",
                        GetDecimal(lineData, SalesOrderFields.Quantity),
                        GetDecimal(lineData, SalesOrderFields.UnitPrice),
                        GetDecimal(lineData, SalesOrderFields.TaxRate),
                        GetDecimal(lineData, SalesOrderFields.DiscountPct));
                }

                _db.SalesOrders.Add(order);
                await _db.SaveChangesAsync(ct);

                foreach (var (row, _) in group)
                {
                    row.MarkPromoted(order.Id);
                    success++;
                }
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                foreach (var (row, _) in group)
                {
                    row.MarkInvalid($"Promote failed: {ex.Message}");
                    failed++;
                }
                await _db.SaveChangesAsync(ct);
            }
        }
        return (success, failed);
    }

    // ── Purchase Order promotion ───────────────────────────────────────────────

    private async Task<(int success, int failed)> PromotePurchaseOrderRows(
        List<ImportJobRow> rows, Guid orgId,
        Dictionary<string, Vendor> vendors,
        Dictionary<string, ProductVariant> variants,
        int success, int failed, CancellationToken ct)
    {
        var groups = rows
            .Select(r => (Row: r, Data: ParsePayload(r.RawPayload)))
            .GroupBy(x => $"{Get(x.Data, PurchaseOrderFields.VendorNumber)}|{Get(x.Data, PurchaseOrderFields.OrderDate)}|{Get(x.Data, PurchaseOrderFields.Description)}");

        foreach (var group in groups)
        {
            var firstRow  = group.First();
            var firstData = firstRow.Data;
            try
            {
                if (!vendors.TryGetValue(Get(firstData, PurchaseOrderFields.VendorNumber) ?? "", out var vendor))
                    throw new Exception("Vendor not found.");

                DateTime.TryParse(Get(firstData, PurchaseOrderFields.OrderDate), out var orderDate);
                DateTime? expectedDate = null;
                if (DateTime.TryParse(Get(firstData, PurchaseOrderFields.ExpectedDate), out var ed)) expectedDate = ed;

                var poCount  = await _db.PurchaseOrders.CountAsync(ct);
                var poNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{(poCount + 1):D4}";

                var po = new PurchaseOrder(orgId, poNumber, vendor.Id, orderDate,
                    Get(firstData, PurchaseOrderFields.Description) ?? "",
                    GetOr(firstData, PurchaseOrderFields.Currency, "USD"),
                    expectedDate);

                foreach (var (_, lineData) in group)
                {
                    var sku = Get(lineData, PurchaseOrderFields.VariantSku) ?? "";
                    if (!variants.TryGetValue(sku, out var variant)) throw new Exception($"Variant '{sku}' not found.");
                    po.AddLine(variant.Id, sku, sku, "Each",
                        GetDecimal(lineData, PurchaseOrderFields.OrderedQty),
                        GetDecimal(lineData, PurchaseOrderFields.UnitCost),
                        GetDecimal(lineData, PurchaseOrderFields.TaxRate));
                }

                _db.PurchaseOrders.Add(po);
                await _db.SaveChangesAsync(ct);

                foreach (var (row, _) in group)
                {
                    row.MarkPromoted(po.Id);
                    success++;
                }
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                foreach (var (row, _) in group)
                {
                    row.MarkInvalid($"Promote failed: {ex.Message}");
                    failed++;
                }
                await _db.SaveChangesAsync(ct);
            }
        }
        return (success, failed);
    }

    // ── Auto-confirm ───────────────────────────────────────────────────────────

    public async Task<int> AutoConfirmSalesOrdersAsync(Guid importJobId, CancellationToken ct = default)
    {
        // Find the SalesOrder IDs that were promoted by this job
        var promotedIds = await _db.ImportJobRows
            .Where(r => r.ImportJobId == importJobId
                     && r.Status == RowStatus.Promoted
                     && r.PromotedEntityId.HasValue)
            .Select(r => r.PromotedEntityId!.Value)
            .Distinct()
            .ToListAsync(ct);

        if (!promotedIds.Any()) return 0;

        var orders = await _db.SalesOrders
            .Where(o => promotedIds.Contains(o.Id) && o.Status == Domain.Modules.AccountsReceivable.SalesOrderStatus.Draft)
            .Include(o => o.Lines)
            .ToListAsync(ct);

        int confirmed = 0;
        foreach (var order in orders)
        {
            try
            {
                order.Confirm();
                confirmed++;
            }
            catch { /* skip orders that can't be confirmed (no lines, wrong status, etc.) */ }
        }

        if (confirmed > 0)
            await _db.SaveChangesAsync(ct);

        return confirmed;
    }

    // ── Export ─────────────────────────────────────────────────────────────────

    public async Task<(byte[] Data, string FileName, string ContentType)> ExportAsync(
        Guid orgId, string entityType, string fileFormat, CancellationToken ct = default)
    {
        var et = Enum.Parse<EntityType>(entityType, true);
        var ff = Enum.Parse<FileFormat>(fileFormat, true);

        var (data, fileName) = et switch
        {
            EntityType.Vendor        => await ExportVendorsAsync(orgId, ff, ct),
            EntityType.Product       => await ExportProductsAsync(orgId, ff, ct),
            EntityType.SalesOrder    => await ExportSalesOrdersAsync(orgId, ff, ct),
            EntityType.PurchaseOrder => await ExportPurchaseOrdersAsync(orgId, ff, ct),
            _ => throw new InvalidOperationException($"Unknown entity type: {et}")
        };

        var contentType = ff switch
        {
            FileFormat.Csv  => "text/csv",
            FileFormat.Json => "application/json",
            FileFormat.Xml  => "application/xml",
            _ => "application/octet-stream"
        };
        return (data, fileName, contentType);
    }

    private async Task<(byte[], string)> ExportVendorsAsync(Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var vendors = await _db.Vendors.Where(v => v.OrganizationId == orgId).ToListAsync(ct);

        var rows = vendors.Select(v => new Dictionary<string, string?>
        {
            [VendorFields.VendorNumber]      = v.VendorNumber,
            [VendorFields.Name]              = v.Name,
            [VendorFields.Email]             = v.Email,
            [VendorFields.Phone]             = v.Phone,
            [VendorFields.Address]           = v.Address,
            [VendorFields.Currency]          = v.Currency,
            [VendorFields.PaymentTermsDays]  = v.PaymentTermsDays.ToString(),
            [VendorFields.TaxId]             = v.TaxId,
            [VendorFields.BankAccountName]   = v.BankAccountName,
            [VendorFields.BankAccountNumber] = v.BankAccountNumber
        }).ToList();

        return (SerializeRows(rows, VendorFields.All, ff, "Vendor", "Vendors"),
                $"vendors_export.{FormatExt(ff)}");
    }

    private async Task<(byte[], string)> ExportProductsAsync(Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var products = await _db.CatalogProducts.Where(p => p.OrganizationId == orgId).ToListAsync(ct);
        var catMap   = await _db.Categories.Where(c => c.OrganizationId == orgId).ToDictionaryAsync(c => c.Id, c => c.Code, ct);
        var brandMap = await _db.Brands.Where(b => b.OrganizationId == orgId).ToDictionaryAsync(b => b.Id, b => b.Code, ct);

        var rows = products.Select(p => new Dictionary<string, string?>
        {
            [ProductFields.Sku]           = p.Sku,
            [ProductFields.Name]          = p.Name,
            [ProductFields.Description]   = p.Description,
            [ProductFields.LongDescription]= p.LongDescription,
            [ProductFields.CategoryCode]  = catMap.TryGetValue(p.CategoryId, out var cc) ? cc : null,
            [ProductFields.BrandCode]     = p.BrandId.HasValue && brandMap.TryGetValue(p.BrandId.Value, out var bc) ? bc : null,
            [ProductFields.ProductType]   = p.ProductType.ToString(),
            [ProductFields.GenderTarget]  = p.GenderTarget.ToString(),
            [ProductFields.BasePrice]     = p.BasePrice.ToString(CultureInfo.InvariantCulture),
            [ProductFields.BaseCost]      = p.BaseCost.ToString(CultureInfo.InvariantCulture),
            [ProductFields.TaxRate]       = p.TaxRateOverride?.ToString(CultureInfo.InvariantCulture) ?? "",
            [ProductFields.UnitOfMeasure] = p.UnitOfMeasure,
            [ProductFields.Currency]      = p.Currency,
            [ProductFields.Tags]          = p.Tags,
            [ProductFields.ImageUrl]      = p.ImageUrl,
            [ProductFields.Status]        = p.Status.ToString()
        }).ToList();

        return (SerializeRows(rows, ProductFields.All, ff, "Product", "Products"),
                $"products_export.{FormatExt(ff)}");
    }

    private async Task<(byte[], string)> ExportSalesOrdersAsync(Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var orders   = await _db.SalesOrders.Where(o => o.OrganizationId == orgId).Include(o => o.Lines).ToListAsync(ct);
        var custNums = await _db.Customers.Where(c => c.OrganizationId == orgId).ToDictionaryAsync(c => c.Id, c => c.CustomerNumber, ct);

        var rows = orders.SelectMany(o =>
            o.Lines.Select(l => new Dictionary<string, string?>
            {
                [SalesOrderFields.CustomerNumber]    = custNums.GetValueOrDefault(o.CustomerId, ""),
                [SalesOrderFields.OrderDate]         = o.OrderDate.ToString("yyyy-MM-dd"),
                [SalesOrderFields.RequestedShipDate] = o.RequestedShipDate?.ToString("yyyy-MM-dd"),
                [SalesOrderFields.Description]       = o.Description,
                [SalesOrderFields.CustomerRef]       = o.CustomerRef,
                [SalesOrderFields.Currency]          = o.Currency,
                [SalesOrderFields.VariantSku]        = l.Sku,
                [SalesOrderFields.Quantity]          = l.Quantity.ToString(CultureInfo.InvariantCulture),
                [SalesOrderFields.UnitPrice]         = l.UnitPrice.ToString(CultureInfo.InvariantCulture),
                [SalesOrderFields.DiscountPct]       = l.DiscountPct.ToString(CultureInfo.InvariantCulture),
                [SalesOrderFields.TaxRate]           = l.TaxRate.ToString(CultureInfo.InvariantCulture)
            })).ToList();

        return (SerializeRows(rows, SalesOrderFields.All, ff, "SalesOrder", "SalesOrders"),
                $"sales_orders_export.{FormatExt(ff)}");
    }

    private async Task<(byte[], string)> ExportPurchaseOrdersAsync(Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var orders  = await _db.PurchaseOrders.Where(o => o.OrganizationId == orgId).Include(o => o.Lines).ToListAsync(ct);
        var vendNums = await _db.Vendors.Where(v => v.OrganizationId == orgId).ToDictionaryAsync(v => v.Id, v => v.VendorNumber, ct);

        var rows = orders.SelectMany(o =>
            o.Lines.Select(l => new Dictionary<string, string?>
            {
                [PurchaseOrderFields.VendorNumber]  = vendNums.GetValueOrDefault(o.VendorId, ""),
                [PurchaseOrderFields.OrderDate]      = o.OrderDate.ToString("yyyy-MM-dd"),
                [PurchaseOrderFields.ExpectedDate]   = o.ExpectedDate?.ToString("yyyy-MM-dd"),
                [PurchaseOrderFields.Description]    = o.Description,
                [PurchaseOrderFields.Currency]       = o.Currency,
                [PurchaseOrderFields.VariantSku]     = l.ProductCode,
                [PurchaseOrderFields.OrderedQty]     = l.OrderedQty.ToString(CultureInfo.InvariantCulture),
                [PurchaseOrderFields.UnitCost]       = l.UnitCost.ToString(CultureInfo.InvariantCulture),
                [PurchaseOrderFields.TaxRate]        = l.TaxRate.ToString(CultureInfo.InvariantCulture)
            })).ToList();

        return (SerializeRows(rows, PurchaseOrderFields.All, ff, "PurchaseOrder", "PurchaseOrders"),
                $"purchase_orders_export.{FormatExt(ff)}");
    }

    // ── Templates ──────────────────────────────────────────────────────────────

    public (byte[] Data, string FileName, string ContentType) GetTemplate(string entityType, string fileFormat)
    {
        var et = Enum.Parse<EntityType>(entityType, true);
        var ff = Enum.Parse<FileFormat>(fileFormat, true);

        var (fields, singular, plural) = et switch
        {
            EntityType.Vendor        => (VendorFields.All,       "Vendor",        "Vendors"),
            EntityType.Product       => (ProductFields.All,      "Product",       "Products"),
            EntityType.SalesOrder    => (SalesOrderFields.All,   "SalesOrder",    "SalesOrders"),
            EntityType.PurchaseOrder => (PurchaseOrderFields.All,"PurchaseOrder", "PurchaseOrders"),
            _ => throw new InvalidOperationException()
        };

        // Empty template = header row only (one blank example row)
        var emptyRow = fields.ToDictionary(f => f, _ => (string?)string.Empty);
        var data     = SerializeRows(new[] { emptyRow }, fields, ff, singular, plural);
        return (data, $"{et.ToString().ToLower()}_template.{FormatExt(ff)}",
                ff switch { FileFormat.Json => "application/json", FileFormat.Xml => "application/xml", _ => "text/csv" });
    }

    // ── Serialization helpers ──────────────────────────────────────────────────

    /// <summary>
    /// Unified serializer: given rows as dicts, produces CSV, JSON, or XML bytes.
    /// </summary>
    private static byte[] SerializeRows(
        IEnumerable<Dictionary<string, string?>> rows,
        string[] columns, FileFormat ff,
        string singularElement, string pluralElement)
    {
        var list = rows.ToList();
        return ff switch
        {
            FileFormat.Csv  => SerializeCsv(list, columns),
            FileFormat.Json => SerializeJson(list, columns),
            FileFormat.Xml  => SerializeXml(list, columns, singularElement, pluralElement),
            _ => throw new InvalidOperationException($"Unsupported format: {ff}")
        };
    }

    private static byte[] SerializeCsv(List<Dictionary<string, string?>> rows, string[] columns)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", columns));
        foreach (var row in rows)
            sb.AppendLine(string.Join(",", columns.Select(c => CsvEscape(row.TryGetValue(c, out var v) ? v : null))));
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static byte[] SerializeJson(List<Dictionary<string, string?>> rows, string[] columns)
    {
        // Emit only the defined columns, in order
        var ordered = rows.Select(row =>
            columns.ToDictionary(c => c, c => row.TryGetValue(c, out var v) ? v : null));
        return JsonSerializer.SerializeToUtf8Bytes(ordered, _jsonOpts);
    }

    private static byte[] SerializeXml(
        List<Dictionary<string, string?>> rows, string[] columns,
        string singularElement, string pluralElement)
    {
        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement(pluralElement,
                rows.Select(row =>
                    new XElement(singularElement,
                        columns
                            .Where(c => row.TryGetValue(c, out var v) && v is not null)
                            .Select(c => new XElement(c, row[c]))))));
        using var ms = new MemoryStream();
        doc.Save(ms);
        return ms.ToArray();
    }

    // ── File parsers ───────────────────────────────────────────────────────────
    // All parsers return List<Dictionary<string, string>> — format-agnostic rows.

    private static List<Dictionary<string, string>> ParseFileToRows(ImportJob job)
    {
        var content = File.ReadAllText(job.FilePath);
        return job.FileFormat switch
        {
            FileFormat.Csv  => ParseCsvToRows(content),
            FileFormat.Json => ParseJsonToRows(content),
            FileFormat.Xml  => ParseXmlToRows(content),
            _ => throw new InvalidOperationException($"Unsupported file format: {job.FileFormat}")
        };
    }

    private static List<Dictionary<string, string>> ParseCsvToRows(string content)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) return new();

        var headers = SplitCsvLine(lines[0]).Select(h => h.Trim().Trim('"')).ToArray();
        var results = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            var vals = SplitCsvLine(lines[i]);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int j = 0; j < headers.Length; j++)
                dict[headers[j]] = j < vals.Length ? vals[j].Trim().Trim('"') : string.Empty;
            results.Add(dict);
        }
        return results;
    }

    private static List<Dictionary<string, string>> ParseJsonToRows(string content)
    {
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var raw  = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(content, opts) ?? new();
        return raw.Select(item =>
            item.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.ValueKind == JsonValueKind.Null ? string.Empty
                    : kv.Value.ToString(),
                StringComparer.OrdinalIgnoreCase))
            .ToList();
    }

    private static List<Dictionary<string, string>> ParseXmlToRows(string content)
    {
        var doc      = XDocument.Parse(content);
        var results  = new List<Dictionary<string, string>>();
        var rootEl   = doc.Root ?? throw new InvalidOperationException("Invalid XML: no root element.");

        foreach (var item in rootEl.Elements())
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var el in item.Elements())
                dict[el.Name.LocalName] = el.Value;
            results.Add(dict);
        }
        return results;
    }

    // ── Low-level CSV helpers ──────────────────────────────────────────────────

    private static string[] SplitCsvLine(string line)
    {
        var results  = new List<string>();
        bool inQuotes = false;
        var  current  = new StringBuilder();
        foreach (char c in line)
        {
            if (c == '"')      { inQuotes = !inQuotes; }
            else if (c == ',' && !inQuotes) { results.Add(current.ToString()); current.Clear(); }
            else current.Append(c);
        }
        results.Add(current.ToString());
        return results.ToArray();
    }

    private static string CsvEscape(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }

    // ── Payload dict helpers ───────────────────────────────────────────────────

    private static Dictionary<string, string> ParsePayload(string json)
        => JsonSerializer.Deserialize<Dictionary<string, string>>(json, _jsonOpts)
           ?? new Dictionary<string, string>();

    private static string? Get(Dictionary<string, string> d, string key)
        => d.TryGetValue(key, out var v) && !string.IsNullOrWhiteSpace(v) ? v.Trim() : null;

    private static string GetOr(Dictionary<string, string> d, string key, string fallback)
        => Get(d, key) ?? fallback;

    private static int GetInt(Dictionary<string, string> d, string key, int fallback)
        => int.TryParse(Get(d, key), out var i) ? i : fallback;

    private static decimal GetDecimal(Dictionary<string, string> d, string key)
        => decimal.TryParse(Get(d, key), NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0m;

    private static decimal? GetDecimalN(Dictionary<string, string> d, string key)
    {
        var raw = Get(d, key);
        return decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
    }

    private static string? BuildAddress(Dictionary<string, string> d)
    {
        var parts = new[] {
            Get(d, VendorFields.Address), Get(d, VendorFields.City),
            Get(d, VendorFields.State),   Get(d, VendorFields.PostalCode),
            Get(d, VendorFields.Country)
        }.Where(p => !string.IsNullOrWhiteSpace(p));
        var combined = string.Join(", ", parts);
        return string.IsNullOrWhiteSpace(combined) ? null : combined;
    }

    // ── Stats helper ───────────────────────────────────────────────────────────

    private async Task<(int valid, int invalid, int promoted, int total)> GetRowStats(
        Guid jobId, CancellationToken ct)
    {
        var stats = await _db.ImportJobRows
            .Where(r => r.ImportJobId == jobId)
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        return (
            stats.Where(s => s.Status == RowStatus.Valid).Sum(s => s.Count),
            stats.Where(s => s.Status == RowStatus.Invalid).Sum(s => s.Count),
            stats.Where(s => s.Status == RowStatus.Promoted).Sum(s => s.Count),
            stats.Sum(s => s.Count)
        );
    }

    // ── ExportUnexportedAsync ──────────────────────────────────────────────────

    public async Task<ExportUnexportedResult> ExportUnexportedAsync(
        Guid orgId, string entityType, string fileFormat, CancellationToken ct = default)
    {
        var et = Enum.Parse<EntityType>(entityType, true);
        var ff = Enum.Parse<FileFormat>(fileFormat, true);

        return et switch
        {
            EntityType.SalesOrder    => await ExportUnexportedSalesOrdersAsync(orgId, ff, ct),
            EntityType.PurchaseOrder => await ExportUnexportedPurchaseOrdersAsync(orgId, ff, ct),
            EntityType.Vendor        => await ExportUnexportedVendorsAsync(orgId, ff, ct),
            EntityType.Product       => await ExportUnexportedProductsAsync(orgId, ff, ct),
            _ => throw new InvalidOperationException($"Unknown entity type: {et}")
        };
    }

    private string ContentTypeFor(FileFormat ff) => ff switch
    {
        FileFormat.Csv  => "text/csv",
        FileFormat.Json => "application/json",
        FileFormat.Xml  => "application/xml",
        _               => "application/octet-stream"
    };

    private async Task<ExportUnexportedResult> ExportUnexportedSalesOrdersAsync(
        Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var orders = await _db.SalesOrders
                        .Where(o => o.OrganizationId == orgId && !o.IsExported
                            && o.Status == SalesOrderStatus.Shipped)
                        .Include(o => o.Lines)
                        .ToListAsync(ct);

        var customers = await _db.Customers
                            .Where(c => c.OrganizationId == orgId)
                            .ToDictionaryAsync(c => c.Id, ct);

        var payments = await _db.ARPayments
                            .Include(p => p.ARInvoice)
                            .Where(p => p.OrganizationId == orgId && !p.IsDeleted)
                            .ToListAsync(ct);
        var paymentsByOrder = payments
                            .Where(p => p.ARInvoice?.SalesOrderId != null)
                            .GroupBy(p => p.ARInvoice!.SalesOrderId!.Value)
                            .ToDictionary(g => g.Key, g => g.ToList());

        // XML gets the full hierarchical format; CSV/JSON stay flat
        byte[] data = ff == FileFormat.Xml
            ? SerializeSalesOrdersXml(orders, customers, paymentsByOrder)
            : SerializeRows(
                orders.SelectMany(o => o.Lines.Where(l => !l.IsDeleted).Select(l =>
                {
                    customers.TryGetValue(o.CustomerId, out var cust);
                    return new Dictionary<string, string?>
                    {
                        [SalesOrderFields.OrderNumber]       = o.OrderNumber,
                        [SalesOrderFields.CustomerNumber]    = cust?.CustomerNumber,
                        [SalesOrderFields.OrderDate]         = o.OrderDate.ToString("yyyy-MM-dd"),
                        [SalesOrderFields.RequestedShipDate] = o.RequestedShipDate?.ToString("yyyy-MM-dd"),
                        [SalesOrderFields.Description]       = o.Description,
                        [SalesOrderFields.CustomerRef]       = o.CustomerRef,
                        [SalesOrderFields.Currency]          = o.Currency,
                        [SalesOrderFields.VariantSku]        = l.Sku,
                        [SalesOrderFields.Quantity]          = l.Quantity.ToString(),
                        [SalesOrderFields.UnitPrice]         = l.UnitPrice.ToString(),
                        [SalesOrderFields.DiscountPct]       = l.DiscountPct.ToString(),
                        [SalesOrderFields.TaxRate]           = l.TaxRate.ToString(),
                    };
                })).ToList(),
                SalesOrderFields.All, ff, "SalesOrder", "SalesOrders");

        var entities = orders.Select(o => (o.Id, (string)o.OrderNumber)).ToList();
        foreach (var o in orders) o.MarkExported();
        await _db.SaveChangesAsync(ct);

        return new ExportUnexportedResult(data, $"sales-orders-export.{FormatExt(ff)}", ContentTypeFor(ff), orders.Count, entities);
    }

    /// <summary>
    /// Hierarchical XML export — one &lt;SalesOrder&gt; per order with nested
    /// Header, Customer, Lines, Totals, Tax and Payments sections.
    /// </summary>
    private static byte[] SerializeSalesOrdersXml(
        List<Domain.Modules.AccountsReceivable.SalesOrder> orders,
        Dictionary<Guid, Domain.Modules.AccountsReceivable.Customer> customers,
        Dictionary<Guid, List<Domain.Modules.AccountsReceivable.ARPayment>> paymentsByOrder)
    {
        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("SalesOrders",
                new XAttribute("exportedAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                orders.Select(o =>
                {
                    customers.TryGetValue(o.CustomerId, out var cust);
                    var lines      = o.Lines.Where(l => !l.IsDeleted).ToList();
                    var subTotal   = lines.Sum(l => l.LineSubTotal);
                    var discTotal  = lines.Sum(l => l.DiscountAmount);
                    var taxTotal   = lines.Sum(l => l.TaxAmount);
                    var grandTotal = lines.Sum(l => l.LineTotal);
                    paymentsByOrder.TryGetValue(o.Id, out var pmts);
                    var paidAmount = (pmts ?? []).Sum(p => p.Amount);

                    return new XElement("SalesOrder",

                        new XElement("Header",
                            new XElement("OrderNumber",       o.OrderNumber),
                            new XElement("Status",            o.Status.ToString()),
                            new XElement("OrderDate",         o.OrderDate.ToString("yyyy-MM-dd")),
                            new XElement("RequestedShipDate", o.RequestedShipDate?.ToString("yyyy-MM-dd") ?? ""),
                            new XElement("ActualShipDate",    o.ActualShipDate?.ToString("yyyy-MM-dd") ?? ""),
                            new XElement("CustomerRef",       o.CustomerRef ?? ""),
                            new XElement("Description",       o.Description ?? ""),
                            new XElement("Currency",          o.Currency)
                        ),

                        new XElement("Customer",
                            new XElement("CustomerNumber",    cust?.CustomerNumber ?? ""),
                            new XElement("Name",              cust?.Name ?? ""),
                            new XElement("Email",             cust?.Email ?? ""),
                            new XElement("Phone",             cust?.Phone ?? ""),
                            new XElement("Address",           cust?.Address ?? ""),
                            new XElement("PaymentTermsDays",  cust?.PaymentTermsDays.ToString() ?? "")
                        ),

                        new XElement("Lines",
                            lines.Select(l => new XElement("SalesOrderLine",
                                new XElement("Sku",            l.Sku),
                                new XElement("ProductName",    l.ProductName),
                                new XElement("Variant",        l.VariantDescription ?? ""),
                                new XElement("UnitOfMeasure",  l.UnitOfMeasure),
                                new XElement("Quantity",       l.Quantity.ToString("F4")),
                                new XElement("UnitPrice",      l.UnitPrice.ToString("F4")),
                                new XElement("DiscountPct",    l.DiscountPct.ToString("F4")),
                                new XElement("DiscountAmount", l.DiscountAmount.ToString("F4")),
                                new XElement("TaxRate",        l.TaxRate.ToString("F4")),
                                new XElement("TaxAmount",      l.TaxAmount.ToString("F4")),
                                new XElement("LineTotal",      l.LineTotal.ToString("F4"))
                            ))
                        ),

                        new XElement("Totals",
                            new XElement("SubTotal",      subTotal.ToString("F4")),
                            new XElement("DiscountTotal", discTotal.ToString("F4")),
                            new XElement("TaxTotal",      taxTotal.ToString("F4")),
                            new XElement("GrandTotal",    grandTotal.ToString("F4")),
                            new XElement("PaidAmount",    paidAmount.ToString("F4")),
                            new XElement("BalanceDue",    (grandTotal - paidAmount).ToString("F4"))
                        ),

                        new XElement("Tax",
                            new XElement("Jurisdiction", ""),
                            new XElement("State",        ""),
                            new XElement("Zip",          ""),
                            new XElement("TaxTotal",     taxTotal.ToString("F4"))
                        ),

                        new XElement("Payments",
                            (pmts ?? []).Select(p => new XElement("Payment",
                                new XElement("PaymentNumber", p.PaymentNumber),
                                new XElement("PaymentDate",   p.PaymentDate.ToString("yyyy-MM-dd")),
                                new XElement("Method",        p.PaymentMethod.ToString()),
                                new XElement("Amount",        p.Amount.ToString("F4")),
                                new XElement("Reference",     p.Reference ?? ""),
                                new XElement("Status",        p.Status.ToString())
                            ))
                        )
                    );
                })
            )
        );

        using var ms = new MemoryStream();
        using (var writer = System.Xml.XmlWriter.Create(ms, new System.Xml.XmlWriterSettings
        {
            Indent      = true,
            IndentChars = "\t",
            Encoding    = new System.Text.UTF8Encoding(false)
        }))
            doc.Save(writer);
        return ms.ToArray();
    }

    private async Task<ExportUnexportedResult> ExportUnexportedPurchaseOrdersAsync(
        Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var orders  = await _db.PurchaseOrders.Where(o => o.OrganizationId == orgId && !o.IsExported)
                         .Include(o => o.Lines).ToListAsync(ct);
        var vendNums = await _db.Vendors.Where(v => v.OrganizationId == orgId)
                         .ToDictionaryAsync(v => v.Id, v => v.VendorNumber, ct);

        var rows = orders.SelectMany(o => o.Lines.Select(l => new Dictionary<string, string?>
        {
            [PurchaseOrderFields.VendorNumber]  = vendNums.TryGetValue(o.VendorId, out var vn) ? vn : null,
            [PurchaseOrderFields.OrderDate]     = o.OrderDate.ToString("yyyy-MM-dd"),
            [PurchaseOrderFields.ExpectedDate]  = o.ExpectedDate?.ToString("yyyy-MM-dd"),
            [PurchaseOrderFields.Description]   = o.Description,
            [PurchaseOrderFields.Currency]      = o.Currency,
            [PurchaseOrderFields.VariantSku]    = l.ProductCode,
            [PurchaseOrderFields.OrderedQty]    = l.OrderedQty.ToString(),
            [PurchaseOrderFields.UnitCost]      = l.UnitCost.ToString(),
            [PurchaseOrderFields.TaxRate]       = l.TaxRate.ToString(),
        })).ToList();

        var data = SerializeRows(rows, PurchaseOrderFields.All, ff, "PurchaseOrder", "PurchaseOrders");
        var entities = orders.Select(o => (o.Id, (string)o.PONumber)).ToList();

        foreach (var o in orders) o.MarkExported();
        await _db.SaveChangesAsync(ct);

        return new ExportUnexportedResult(data, $"purchase-orders-export.{FormatExt(ff)}", ContentTypeFor(ff), orders.Count, entities);
    }

    private async Task<ExportUnexportedResult> ExportUnexportedVendorsAsync(
        Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var vendors = await _db.Vendors.Where(v => v.OrganizationId == orgId && !v.IsExported).ToListAsync(ct);

        var rows = vendors.Select(v => new Dictionary<string, string?>
        {
            [VendorFields.VendorNumber]      = v.VendorNumber,
            [VendorFields.Name]              = v.Name,
            [VendorFields.Email]             = v.Email,
            [VendorFields.Phone]             = v.Phone,
            [VendorFields.Address]           = v.Address,
            [VendorFields.Currency]          = v.Currency,
            [VendorFields.PaymentTermsDays]  = v.PaymentTermsDays.ToString(),
            [VendorFields.TaxId]             = v.TaxId,
            [VendorFields.BankAccountName]   = v.BankAccountName,
            [VendorFields.BankAccountNumber] = v.BankAccountNumber,
        }).ToList();

        var data = SerializeRows(rows, VendorFields.All, ff, "Vendor", "Vendors");
        var entities = vendors.Select(v => (v.Id, v.VendorNumber)).ToList();

        foreach (var v in vendors) v.MarkExported();
        await _db.SaveChangesAsync(ct);

        return new ExportUnexportedResult(data, $"vendors-export.{FormatExt(ff)}", ContentTypeFor(ff), vendors.Count, entities);
    }

    private async Task<ExportUnexportedResult> ExportUnexportedProductsAsync(
        Guid orgId, FileFormat ff, CancellationToken ct)
    {
        var products = await _db.CatalogProducts.Where(p => p.OrganizationId == orgId && !p.IsExported).ToListAsync(ct);
        var catMap   = await _db.Categories.Where(c => c.OrganizationId == orgId).ToDictionaryAsync(c => c.Id, c => c.Code, ct);
        var brandMap = await _db.Brands.Where(b => b.OrganizationId == orgId).ToDictionaryAsync(b => b.Id, b => b.Code, ct);

        var rows = products.Select(p => new Dictionary<string, string?>
        {
            [ProductFields.Sku]           = p.Sku,
            [ProductFields.Name]          = p.Name,
            [ProductFields.Description]   = p.Description,
            [ProductFields.LongDescription]= p.LongDescription,
            [ProductFields.CategoryCode]  = catMap.TryGetValue(p.CategoryId, out var cc) ? cc : null,
            [ProductFields.BrandCode]     = p.BrandId.HasValue && brandMap.TryGetValue(p.BrandId.Value, out var bc) ? bc : null,
            [ProductFields.ProductType]   = p.ProductType.ToString(),
            [ProductFields.GenderTarget]  = p.GenderTarget.ToString(),
            [ProductFields.BasePrice]     = p.BasePrice.ToString(),
            [ProductFields.BaseCost]      = p.BaseCost.ToString(),
            [ProductFields.TaxRate]       = p.TaxRateOverride?.ToString() ?? "",
            [ProductFields.UnitOfMeasure] = p.UnitOfMeasure,
            [ProductFields.Currency]      = p.Currency,
            [ProductFields.Tags]          = p.Tags,
            [ProductFields.ImageUrl]      = p.ImageUrl,
            [ProductFields.Status]        = p.Status.ToString(),
        }).ToList();

        var data = SerializeRows(rows, ProductFields.All, ff, "Product", "Products");
        var entities = products.Select(p => (p.Id, p.Sku)).ToList();

        foreach (var p in products) p.MarkExported();
        await _db.SaveChangesAsync(ct);

        return new ExportUnexportedResult(data, $"products-export.{FormatExt(ff)}", ContentTypeFor(ff), products.Count, entities);
    }

    // ── ResetExportAsync ───────────────────────────────────────────────────────

    public async Task<int> ResetExportAsync(
        Guid orgId, string entityType, IEnumerable<Guid> entityIds, CancellationToken ct = default)
    {
        var et = Enum.Parse<EntityType>(entityType, true);
        var ids = entityIds.ToList();
        int count = 0;

        switch (et)
        {
            case EntityType.SalesOrder:
                var sos = await _db.SalesOrders
                    .Where(o => o.OrganizationId == orgId && ids.Contains(o.Id)).ToListAsync(ct);
                foreach (var o in sos) { o.ResetExport(); count++; }
                break;
            case EntityType.PurchaseOrder:
                var pos = await _db.PurchaseOrders
                    .Where(o => o.OrganizationId == orgId && ids.Contains(o.Id)).ToListAsync(ct);
                foreach (var o in pos) { o.ResetExport(); count++; }
                break;
            case EntityType.Vendor:
                var vs = await _db.Vendors
                    .Where(v => v.OrganizationId == orgId && ids.Contains(v.Id)).ToListAsync(ct);
                foreach (var v in vs) { v.ResetExport(); count++; }
                break;
            case EntityType.Product:
                var ps = await _db.CatalogProducts
                    .Where(p => p.OrganizationId == orgId && ids.Contains(p.Id)).ToListAsync(ct);
                foreach (var p in ps) { p.ResetExport(); count++; }
                break;
        }

        await _db.SaveChangesAsync(ct);
        return count;
    }

    // ── DTO mapper ─────────────────────────────────────────────────────────────

    private static ImportJobDto ToDto(ImportJob j, int valid, int invalid, int promoted, int staged) => new(
        j.Id, j.EntityType.ToString(), j.FileFormat.ToString(), j.FileName,
        j.Status.ToString(), j.TotalRows, j.SuccessRows, j.FailedRows,
        staged, valid, invalid, promoted,
        j.ErrorSummary, j.TriggeredBy, j.StartedAt, j.CompletedAt, j.CreatedAt);

    private static string FormatExt(FileFormat ff) => ff switch
    {
        FileFormat.Csv  => "csv",
        FileFormat.Json => "json",
        FileFormat.Xml  => "xml",
        _               => "dat"
    };
}
