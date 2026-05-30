using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.ProductManagement.DTOs;
using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.ProductManagement.Services;

public interface IProductManagementService
{
    // Categories
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest req, CancellationToken ct = default);
    Task DeleteCategoryAsync(Guid id, CancellationToken ct = default);

    // Brands
    Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default);
    Task<BrandDto> CreateBrandAsync(CreateBrandRequest req, CancellationToken ct = default);
    Task DeleteBrandAsync(Guid id, CancellationToken ct = default);

    // Products
    Task<IEnumerable<ProductSummaryDto>> GetProductsAsync(
        string? categoryId = null, string? brandId = null,
        string? status = null, string? search = null,
        CancellationToken ct = default);
    Task<ProductDto?> GetProductAsync(Guid id, CancellationToken ct = default);
    Task<ProductDto> CreateProductAsync(CreateProductRequest req, CancellationToken ct = default);
    Task UpdateProductAsync(Guid id, UpdateProductRequest req, CancellationToken ct = default);
    Task ChangeProductStatusAsync(Guid id, string status, CancellationToken ct = default);

    // Variants
    Task<ProductVariantDto> AddVariantAsync(Guid productId, CreateVariantRequest req, CancellationToken ct = default);
    Task UpdateVariantPricingAsync(Guid variantId, UpdateVariantPricingRequest req, CancellationToken ct = default);
    Task DeactivateVariantAsync(Guid variantId, CancellationToken ct = default);

    // Inventory
    Task<IEnumerable<InventoryDto>> GetInventoryAsync(
        bool? needsReorder = null, CancellationToken ct = default);
    Task AdjustInventoryAsync(Guid variantId, AdjustInventoryRequest req, CancellationToken ct = default);
    Task SetInventoryAsync(Guid variantId, SetInventoryRequest req, CancellationToken ct = default);
    Task UpdateThresholdsAsync(Guid variantId, UpdateThresholdsRequest req, CancellationToken ct = default);

    // Preferred vendor
    Task SetPreferredVendorAsync(Guid productId, Guid? vendorId, CancellationToken ct = default);

    // Lookup for sales order line picker + AP PO line picker
    Task<IEnumerable<VariantLookupDto>> SearchVariantsAsync(string? q, CancellationToken ct = default);
}

public class ProductManagementService : IProductManagementService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public ProductManagementService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db = db;
        _org = org;
    }

    // ── Categories ────────────────────────────────────────────────────────────

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var cats = await _db.Categories
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
            .ToListAsync(ct);

        var productCounts = await _db.CatalogProducts
            .Where(p => !p.IsDeleted)
            .GroupBy(p => p.CategoryId)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Key, g => g.Count, ct);

        var catIndex = cats.ToDictionary(c => c.Id, c => c.Name);

        return cats.Select(c => new CategoryDto(
            c.Id, c.Code, c.Name, c.Description, c.ParentCategoryId,
            c.ParentCategoryId.HasValue && catIndex.TryGetValue(c.ParentCategoryId.Value, out var pn) ? pn : null,
            c.DisplayOrder, c.IsActive,
            productCounts.GetValueOrDefault(c.Id, 0)));
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest req, CancellationToken ct = default)
    {
        var exists = await _db.Categories.AnyAsync(
            c => c.Code == req.Code.ToUpperInvariant() && !c.IsDeleted, ct);
        if (exists) throw new InvalidOperationException($"Category code '{req.Code}' already exists.");

        var cat = new Category(_org.OrganizationId, req.Code, req.Name,
            req.ParentCategoryId, req.Description, req.DisplayOrder);
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync(ct);
        return new CategoryDto(cat.Id, cat.Code, cat.Name, cat.Description,
            cat.ParentCategoryId, null, cat.DisplayOrder, cat.IsActive, 0);
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken ct = default)
    {
        var cat = await _db.Categories.FindAsync(new object[] { id }, ct)
            ?? throw new InvalidOperationException("Category not found.");
        var hasProducts = await _db.CatalogProducts.AnyAsync(p => p.CategoryId == id && !p.IsDeleted, ct);
        if (hasProducts) throw new InvalidOperationException("Cannot delete a category that has products.");
        cat.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Brands ────────────────────────────────────────────────────────────────

    public async Task<IEnumerable<BrandDto>> GetBrandsAsync(CancellationToken ct = default)
    {
        var brands = await _db.Brands
            .Where(b => !b.IsDeleted)
            .OrderBy(b => b.Name)
            .ToListAsync(ct);

        var productCounts = await _db.CatalogProducts
            .Where(p => !p.IsDeleted && p.BrandId.HasValue)
            .GroupBy(p => p.BrandId!.Value)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Key, g => g.Count, ct);

        return brands.Select(b => new BrandDto(
            b.Id, b.Code, b.Name, b.Description, b.Country,
            b.Website, b.LogoUrl, b.IsActive,
            productCounts.GetValueOrDefault(b.Id, 0)));
    }

    public async Task<BrandDto> CreateBrandAsync(CreateBrandRequest req, CancellationToken ct = default)
    {
        var exists = await _db.Brands.AnyAsync(
            b => b.Code == req.Code.ToUpperInvariant() && !b.IsDeleted, ct);
        if (exists) throw new InvalidOperationException($"Brand code '{req.Code}' already exists.");

        var brand = new Brand(_org.OrganizationId, req.Code, req.Name,
            req.Description, req.Country, req.Website);
        _db.Brands.Add(brand);
        await _db.SaveChangesAsync(ct);
        return new BrandDto(brand.Id, brand.Code, brand.Name, brand.Description,
            brand.Country, brand.Website, brand.LogoUrl, brand.IsActive, 0);
    }

    public async Task DeleteBrandAsync(Guid id, CancellationToken ct = default)
    {
        var brand = await _db.Brands.FindAsync(new object[] { id }, ct)
            ?? throw new InvalidOperationException("Brand not found.");
        var hasProducts = await _db.CatalogProducts.AnyAsync(p => p.BrandId == id && !p.IsDeleted, ct);
        if (hasProducts) throw new InvalidOperationException("Cannot delete a brand that has products assigned to it.");
        brand.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Products ──────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ProductSummaryDto>> GetProductsAsync(
        string? categoryId = null, string? brandId = null,
        string? status = null, string? search = null,
        CancellationToken ct = default)
    {
        var q = _db.CatalogProducts
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Inventory)
            .Where(p => !p.IsDeleted)
            .AsQueryable();

        if (Guid.TryParse(categoryId, out var catId))
            q = q.Where(p => p.CategoryId == catId);
        if (Guid.TryParse(brandId, out var brId))
            q = q.Where(p => p.BrandId == brId);
        if (!string.IsNullOrWhiteSpace(status)
            && Enum.TryParse<ProductStatus>(status, out var ps))
            q = q.Where(p => p.Status == ps);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(p => p.Name.Contains(search) || p.Sku.Contains(search)
                           || (p.Tags != null && p.Tags.Contains(search)));

        var list = await q.OrderBy(p => p.Category!.Name).ThenBy(p => p.Name).ToListAsync(ct);

        return list.Select(p =>
        {
            var activeVariants = p.Variants.Where(v => v.Status != VariantStatus.Discontinued).ToList();
            var totalStock = activeVariants.Sum(v => v.Inventory?.QuantityOnHand ?? 0);
            return new ProductSummaryDto(
                p.Id, p.Sku, p.Name,
                p.Category?.Name ?? "—",
                p.Brand?.Name,
                p.ProductType.ToString(),
                p.GenderTarget.ToString(),
                p.BasePrice, p.Currency,
                p.Status.ToString(),
                activeVariants.Count,
                totalStock);
        });
    }

    public async Task<ProductDto?> GetProductAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _db.CatalogProducts
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Inventory)
            .Where(p => p.Id == id && !p.IsDeleted)
            .FirstOrDefaultAsync(ct);

        if (p is null) return null;

        string? preferredVendorName = null;
        if (p.PreferredVendorId.HasValue)
        {
            var vendor = await _db.Vendors
                .FirstOrDefaultAsync(v => v.Id == p.PreferredVendorId.Value && !v.IsDeleted, ct);
            preferredVendorName = vendor?.Name;
        }
        return MapProduct(p, preferredVendorName);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest req, CancellationToken ct = default)
    {
        var skuExists = await _db.CatalogProducts.AnyAsync(
            p => p.Sku == req.Sku.ToUpperInvariant() && !p.IsDeleted, ct);
        if (skuExists) throw new InvalidOperationException($"SKU '{req.Sku}' already exists.");

        if (!Enum.TryParse<ProductType>(req.ProductType, out var pt))
            throw new InvalidOperationException($"Invalid product type '{req.ProductType}'.");
        if (!Enum.TryParse<GenderTarget>(req.GenderTarget, out var gt))
            throw new InvalidOperationException($"Invalid gender target '{req.GenderTarget}'.");

        var product = new Domain.Modules.ProductManagement.Product(
            _org.OrganizationId, req.Sku, req.Name, req.CategoryId,
            pt, req.BasePrice, req.BaseCost, req.TaxRate,
            req.UnitOfMeasure, req.BrandId, gt, req.Description, req.Tags, req.Currency);

        _db.CatalogProducts.Add(product);
        await _db.SaveChangesAsync(ct);
        return MapProduct(product);
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductRequest req, CancellationToken ct = default)
    {
        var p = await _db.CatalogProducts.FindAsync(new object[] { id }, ct)
            ?? throw new InvalidOperationException("Product not found.");

        if (!Enum.TryParse<ProductType>(req.ProductType, out var pt))
            throw new InvalidOperationException($"Invalid product type '{req.ProductType}'.");
        if (!Enum.TryParse<GenderTarget>(req.GenderTarget, out var gt))
            throw new InvalidOperationException($"Invalid gender target '{req.GenderTarget}'.");

        p.Update(req.Name, req.Description, req.LongDescription,
            req.CategoryId, req.BrandId, req.BasePrice, req.BaseCost,
            req.TaxRate, pt, gt, req.Tags, req.ImageUrl);
        await _db.SaveChangesAsync(ct);
    }

    public async Task ChangeProductStatusAsync(Guid id, string status, CancellationToken ct = default)
    {
        var p = await _db.CatalogProducts.FindAsync(new object[] { id }, ct)
            ?? throw new InvalidOperationException("Product not found.");
        switch (status.ToLowerInvariant())
        {
            case "active":       p.Activate();     break;
            case "inactive":     p.Deactivate();   break;
            case "discontinued": p.Discontinue();  break;
            default: throw new InvalidOperationException($"Unknown status '{status}'.");
        }
        await _db.SaveChangesAsync(ct);
    }

    // ── Variants ──────────────────────────────────────────────────────────────

    public async Task<ProductVariantDto> AddVariantAsync(Guid productId, CreateVariantRequest req, CancellationToken ct = default)
    {
        var p = await _db.CatalogProducts
            .Include(x => x.Variants)
            .FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Product not found.");

        var variant = p.AddVariant(req.Size, req.Color, req.Material,
            req.Barcode, req.PriceOverride, req.CostOverride);

        // Create inventory record with initial stock
        var inv = new InventoryRecord(_org.OrganizationId, variant.Id,
            req.InitialStock, req.ReorderPoint, req.MinimumStock, req.MaximumStock, req.Location);
        _db.InventoryRecords.Add(inv);

        await _db.SaveChangesAsync(ct);
        return MapVariant(variant, inv, p.BasePrice, p.BaseCost);
    }

    public async Task UpdateVariantPricingAsync(Guid variantId, UpdateVariantPricingRequest req, CancellationToken ct = default)
    {
        var v = await _db.ProductVariants.FindAsync(new object[] { variantId }, ct)
            ?? throw new InvalidOperationException("Variant not found.");
        v.UpdatePricing(req.PriceOverride, req.CostOverride);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeactivateVariantAsync(Guid variantId, CancellationToken ct = default)
    {
        var v = await _db.ProductVariants.FindAsync(new object[] { variantId }, ct)
            ?? throw new InvalidOperationException("Variant not found.");
        v.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    // ── Inventory ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<InventoryDto>> GetInventoryAsync(
        bool? needsReorder = null, CancellationToken ct = default)
    {
        var q = _db.InventoryRecords
            .Include(i => i.ProductVariant)
                .ThenInclude(v => v!.Product)
            .Where(i => !i.IsDeleted)
            .AsQueryable();

        if (needsReorder == true)
            q = q.Where(i => i.QuantityOnHand <= i.ReorderPoint);

        var list = await q
            .OrderBy(i => i.ProductVariant!.Product!.Name)
            .ThenBy(i => i.ProductVariant!.Size)
            .ToListAsync(ct);

        return list.Select(i => new InventoryDto(
            i.Id, i.ProductVariantId,
            i.ProductVariant?.Sku ?? "—",
            i.ProductVariant?.Product?.Name ?? "—",
            i.ProductVariant?.Size ?? "—",
            i.ProductVariant?.Color,
            i.QuantityOnHand, i.QuantityReserved, i.QuantityAvailable,
            i.ReorderPoint, i.MinimumStock, i.MaximumStock,
            i.NeedsReorder, i.Location, i.LastCountDate));
    }

    public async Task AdjustInventoryAsync(Guid variantId, AdjustInventoryRequest req, CancellationToken ct = default)
    {
        var inv = await _db.InventoryRecords
            .FirstOrDefaultAsync(i => i.ProductVariantId == variantId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Inventory record not found.");
        inv.AdjustQuantity(req.Delta, req.Reason);
        await _db.SaveChangesAsync(ct);
    }

    public async Task SetInventoryAsync(Guid variantId, SetInventoryRequest req, CancellationToken ct = default)
    {
        var inv = await _db.InventoryRecords
            .FirstOrDefaultAsync(i => i.ProductVariantId == variantId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Inventory record not found.");
        inv.SetOnHand(req.Quantity, req.CountDate);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateThresholdsAsync(Guid variantId, UpdateThresholdsRequest req, CancellationToken ct = default)
    {
        var inv = await _db.InventoryRecords
            .FirstOrDefaultAsync(i => i.ProductVariantId == variantId && !i.IsDeleted, ct)
            ?? throw new InvalidOperationException("Inventory record not found.");
        inv.UpdateThresholds(req.ReorderPoint, req.MinimumStock, req.MaximumStock, req.Location);
        await _db.SaveChangesAsync(ct);
    }

    // ── Preferred Vendor ──────────────────────────────────────────────────────

    public async Task SetPreferredVendorAsync(Guid productId, Guid? vendorId, CancellationToken ct = default)
    {
        var p = await _db.CatalogProducts.FindAsync(new object[] { productId }, ct)
            ?? throw new InvalidOperationException("Product not found.");

        if (vendorId.HasValue)
        {
            var vendorExists = await _db.Vendors
                .AnyAsync(v => v.Id == vendorId.Value && !v.IsDeleted, ct);
            if (!vendorExists)
                throw new InvalidOperationException("Vendor not found.");
        }

        p.SetPreferredVendor(vendorId);
        await _db.SaveChangesAsync(ct);
    }

    // ── Variant Lookup (for AR sales order + AP PO) ───────────────────────────

    public async Task<IEnumerable<VariantLookupDto>> SearchVariantsAsync(
        string? q, CancellationToken ct = default)
    {
        var query = _db.ProductVariants
            .Include(v => v.Product)
            .Include(v => v.Inventory)
            .Where(v => !v.IsDeleted && v.Status == VariantStatus.Active
                     && v.Product != null && v.Product.Status == ProductStatus.Active)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(v =>
                v.Sku.Contains(q) ||
                v.Product!.Name.Contains(q) ||
                (v.Barcode != null && v.Barcode.Contains(q)) ||
                (v.Color != null && v.Color.Contains(q)));

        var variants = await query
            .OrderBy(v => v.Product!.Name)
            .ThenBy(v => v.Size)
            .Take(50)
            .ToListAsync(ct);

        // Load vendor names for preferred vendors in one query
        var vendorIds = variants
            .Where(v => v.Product?.PreferredVendorId.HasValue == true)
            .Select(v => v.Product!.PreferredVendorId!.Value)
            .Distinct().ToList();
        var vendorNames = vendorIds.Any()
            ? await _db.Vendors.Where(v => vendorIds.Contains(v.Id) && !v.IsDeleted)
                .ToDictionaryAsync(v => v.Id, v => v.Name, ct)
            : new Dictionary<Guid, string>();

        return variants.Select(v => new VariantLookupDto(
            v.Id, v.Sku, v.Barcode,
            v.Product?.Name ?? "—",
            v.Size, v.Color, v.Material,
            v.EffectivePrice(v.Product?.BasePrice ?? 0),
            v.EffectiveCost(v.Product?.BaseCost ?? 0),
            v.Product?.TaxRate ?? 0,
            v.Product?.UnitOfMeasure ?? "Each",
            v.Inventory?.QuantityAvailable ?? 0,
            v.Product?.PreferredVendorId,
            v.Product?.PreferredVendorId.HasValue == true
                && vendorNames.TryGetValue(v.Product.PreferredVendorId.Value, out var vn) ? vn : null));
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static ProductDto MapProduct(
        Domain.Modules.ProductManagement.Product p,
        string? preferredVendorName = null) => new(
        p.Id, p.Sku, p.Name, p.Description, p.LongDescription,
        p.CategoryId, p.Category?.Name ?? "—",
        p.BrandId, p.Brand?.Name,
        p.ProductType.ToString(), p.GenderTarget.ToString(),
        p.UnitOfMeasure, p.BasePrice, p.BaseCost, p.TaxRate, p.Currency,
        p.Tags, p.ImageUrl, p.Status.ToString(),
        p.PreferredVendorId, preferredVendorName,
        p.CreatedAt,
        p.Variants.Select(v => MapVariant(v, v.Inventory, p.BasePrice, p.BaseCost)));

    private static ProductVariantDto MapVariant(
        ProductVariant v, InventoryRecord? inv,
        decimal productBasePrice, decimal productBaseCost) => new(
        v.Id, v.ProductId, v.Sku, v.Barcode, v.Size, v.Color, v.Material,
        v.PriceOverride, v.CostOverride,
        v.EffectivePrice(productBasePrice), v.EffectiveCost(productBaseCost),
        v.Weight, v.Status.ToString(),
        inv?.QuantityOnHand ?? 0, inv?.QuantityReserved ?? 0,
        inv?.QuantityAvailable ?? 0, inv?.ReorderPoint ?? 0,
        inv?.NeedsReorder ?? false, inv?.Location);
}
