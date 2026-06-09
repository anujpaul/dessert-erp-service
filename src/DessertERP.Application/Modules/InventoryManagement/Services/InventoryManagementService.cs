using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.InventoryManagement.DTOs;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.InventoryManagement.Services;

// ── Interface ────────────────────────────────────────────────────────────────

public interface IInventoryManagementService
{
    // List / detail
    Task<PagedInventoryItemsDto> GetItemsAsync(
        string? search = null, string? filter = null, string? category = null,
        string? brand = null, string? location = null, decimal? minOnHand = null,
        decimal? maxOnHand = null, string? sortBy = null, bool descending = false,
        int page = 1, int pageSize = 50, CancellationToken ct = default);
    Task<InventoryFilterOptionsDto> GetFilterOptionsAsync(CancellationToken ct = default);
    Task<InventoryItemDto>              GetItemAsync(Guid inventoryRecordId, CancellationToken ct = default);
    Task<InventorySummaryDto>           GetSummaryAsync(CancellationToken ct = default);

    // Transactions
    Task<IEnumerable<InventoryTransactionDto>> GetTransactionsAsync(Guid productVariantId, int take = 100, CancellationToken ct = default);
    Task<IEnumerable<InventoryTransactionDto>> GetRecentTransactionsAsync(int take = 50, CancellationToken ct = default);

    // Adjustments
    Task<InventoryItemDto> AdjustStockAsync(Guid inventoryRecordId, AdjustStockRequest req, CancellationToken ct = default);
    Task<InventoryItemDto> SetOnHandAsync(Guid inventoryRecordId, SetOnHandRequest req, CancellationToken ct = default);
    Task<InventoryItemDto> UpdateThresholdsAsync(Guid inventoryRecordId, UpdateThresholdsRequest req, CancellationToken ct = default);
    Task<WarehouseInventoryAllocationDto> GetWarehouseBalancesAsync(Guid inventoryRecordId, CancellationToken ct = default);
    Task<WarehouseInventoryAllocationDto> SetWarehouseBalanceAsync(
        Guid inventoryRecordId, SetWarehouseInventoryBalanceRequest req, CancellationToken ct = default);

    // Reports
    Task<IEnumerable<LowStockItemDto>>    GetLowStockAsync(CancellationToken ct = default);
    Task<IEnumerable<StockValuationDto>>  GetValuationByCategory(CancellationToken ct = default);
}

// ── Implementation ────────────────────────────────────────────────────────────

public class InventoryManagementService : IInventoryManagementService
{
    private readonly IAppDbContext _db;

    public InventoryManagementService(IAppDbContext db) => _db = db;

    // ── List / detail ─────────────────────────────────────────────────────────

    public async Task<PagedInventoryItemsDto> GetItemsAsync(
        string? search = null, string? filter = null, string? category = null,
        string? brand = null, string? location = null, decimal? minOnHand = null,
        decimal? maxOnHand = null, string? sortBy = null, bool descending = false,
        int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        var query = _db.InventoryRecords
            .Include(r => r.ProductVariant)
                .ThenInclude(v => v!.Product)
                    .ThenInclude(p => p!.Category)
            .Include(r => r.ProductVariant)
                .ThenInclude(v => v!.Product)
                    .ThenInclude(p => p!.Brand)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(r =>
                r.ProductVariant!.Sku.ToLower().Contains(s) ||
                r.ProductVariant.Product!.Name.ToLower().Contains(s));
        }

        if (filter == "low-stock")
            query = query.Where(r => r.QuantityOnHand <= r.ReorderPoint && r.QuantityOnHand > 0);
        else if (filter == "out-of-stock")
            query = query.Where(r => r.QuantityOnHand <= 0);
        else if (filter == "on-order")
            query = query.Where(r => r.QuantityOnOrder > 0);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(r => r.ProductVariant!.Product!.Category!.Name == category);
        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(r => r.ProductVariant!.Product!.Brand!.Name == brand);
        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(r => r.Location == location);
        if (minOnHand.HasValue)
            query = query.Where(r => r.QuantityOnHand >= minOnHand.Value);
        if (maxOnHand.HasValue)
            query = query.Where(r => r.QuantityOnHand <= maxOnHand.Value);

        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 10, 200);
        var totalCount = await query.CountAsync(ct);

        query = (sortBy?.ToLowerInvariant(), descending) switch
        {
            ("product", false) => query.OrderBy(r => r.ProductVariant!.Product!.Name),
            ("product", true) => query.OrderByDescending(r => r.ProductVariant!.Product!.Name),
            ("onhand", false) => query.OrderBy(r => r.QuantityOnHand),
            ("onhand", true) => query.OrderByDescending(r => r.QuantityOnHand),
            ("available", false) => query.OrderBy(r => r.QuantityOnHand - r.QuantityReserved),
            ("available", true) => query.OrderByDescending(r => r.QuantityOnHand - r.QuantityReserved),
            ("value", false) => query.OrderBy(r => r.QuantityOnHand * r.AverageCost),
            ("value", true) => query.OrderByDescending(r => r.QuantityOnHand * r.AverageCost),
            (_, true) => query.OrderByDescending(r => r.ProductVariant!.Sku),
            _ => query.OrderBy(r => r.ProductVariant!.Sku)
        };

        var records = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedInventoryItemsDto(
            records.Select(ToDto).ToList(),
            page,
            pageSize,
            totalCount,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }

    public async Task<InventoryFilterOptionsDto> GetFilterOptionsAsync(CancellationToken ct = default)
    {
        var categories = await _db.InventoryRecords
            .AsNoTracking()
            .Where(r => r.ProductVariant!.Product!.Category != null)
            .Select(r => r.ProductVariant!.Product!.Category!.Name)
            .Distinct().OrderBy(x => x).ToListAsync(ct);
        var brands = await _db.InventoryRecords
            .AsNoTracking()
            .Where(r => r.ProductVariant!.Product!.Brand != null)
            .Select(r => r.ProductVariant!.Product!.Brand!.Name)
            .Distinct().OrderBy(x => x).ToListAsync(ct);
        var locations = await _db.InventoryRecords
            .AsNoTracking()
            .Where(r => r.Location != null && r.Location != "")
            .Select(r => r.Location!)
            .Distinct().OrderBy(x => x).ToListAsync(ct);

        return new InventoryFilterOptionsDto(categories, brands, locations);
    }

    public async Task<InventoryItemDto> GetItemAsync(Guid inventoryRecordId, CancellationToken ct = default)
    {
        var r = await _db.InventoryRecords
            .Include(r => r.ProductVariant)
                .ThenInclude(v => v!.Product)
                    .ThenInclude(p => p!.Category)
            .Include(r => r.ProductVariant)
                .ThenInclude(v => v!.Product)
                    .ThenInclude(p => p!.Brand)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == inventoryRecordId, ct)
            ?? throw new InvalidOperationException("Inventory record not found.");
        return ToDto(r);
    }

    public async Task<InventorySummaryDto> GetSummaryAsync(CancellationToken ct = default)
    {
        var records = _db.InventoryRecords.AsNoTracking();
        return new InventorySummaryDto(
            TotalSkus:       await records.CountAsync(ct),
            LowStockCount:   await records.CountAsync(r => r.QuantityOnHand <= r.ReorderPoint && r.QuantityOnHand > 0, ct),
            OutOfStockCount: await records.CountAsync(r => r.QuantityOnHand <= 0, ct),
            TotalStockValue: await records.SumAsync(r => r.QuantityOnHand * r.AverageCost, ct),
            OnOrderLines:    await records.CountAsync(r => r.QuantityOnOrder > 0, ct)
        );
    }

    // ── Transactions ──────────────────────────────────────────────────────────

    public async Task<IEnumerable<InventoryTransactionDto>> GetTransactionsAsync(
        Guid productVariantId, int take = 100, CancellationToken ct = default)
    {
        var txns = await _db.InventoryTransactions
            .Include(t => t.ProductVariant)
            .AsNoTracking()
            .Where(t => t.ProductVariantId == productVariantId)
            .OrderByDescending(t => t.TransactionDate)
            .Take(take)
            .ToListAsync(ct);
        return txns.Select(ToTxnDto);
    }

    public async Task<IEnumerable<InventoryTransactionDto>> GetRecentTransactionsAsync(
        int take = 50, CancellationToken ct = default)
    {
        var txns = await _db.InventoryTransactions
            .Include(t => t.ProductVariant)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .Take(take)
            .ToListAsync(ct);
        return txns.Select(ToTxnDto);
    }

    // ── Adjustments ───────────────────────────────────────────────────────────

    public async Task<InventoryItemDto> AdjustStockAsync(
        Guid inventoryRecordId, AdjustStockRequest req, CancellationToken ct = default)
    {
        var record = await LoadRecord(inventoryRecordId, ct);

        InventoryTransactionType txnType;
        if (req.Quantity > 0)
        {
            txnType = InventoryTransactionType.AdjustmentIn;
            record.ReceiveStock(req.Quantity, req.UnitCost);
        }
        else if (req.Quantity < 0)
        {
            txnType = InventoryTransactionType.AdjustmentOut;
            record.IssueStock(Math.Abs(req.Quantity));
        }
        else
        {
            throw new InvalidOperationException("Adjustment quantity cannot be zero.");
        }

        _db.InventoryTransactions.Add(new InventoryTransaction(
            record.OrganizationId,
            record.ProductVariantId,
            txnType,
            req.Quantity,
            req.UnitCost,
            record.QuantityOnHand,
            referenceNumber: "ADJ",
            notes: req.Notes,
            createdBy: req.CreatedBy));

        await _db.SaveChangesAsync(ct);
        return await GetItemAsync(inventoryRecordId, ct);
    }

    public async Task<InventoryItemDto> SetOnHandAsync(
        Guid inventoryRecordId, SetOnHandRequest req, CancellationToken ct = default)
    {
        var record = await LoadRecord(inventoryRecordId, ct);
        var delta  = req.Quantity - record.QuantityOnHand;

        // Cycle count: set absolute quantity and optionally refresh average cost
        if (req.UnitCost > 0 && req.Quantity > 0)
        {
            // Receive the delta to update weighted avg cost, then fix quantity to exact target
            if (delta > 0)
                record.ReceiveStock(delta, req.UnitCost);
            else if (delta < 0)
                record.IssueStock(Math.Abs(delta));
            // After the movement quantity is already at req.Quantity; stamp the count date
            record.SetOnHand(req.Quantity, DateTime.UtcNow);
        }
        else
        {
            record.SetOnHand(req.Quantity, DateTime.UtcNow);
        }

        var txnType = delta >= 0 ? InventoryTransactionType.AdjustmentIn : InventoryTransactionType.AdjustmentOut;

        _db.InventoryTransactions.Add(new InventoryTransaction(
            record.OrganizationId,
            record.ProductVariantId,
            txnType,
            delta,
            req.UnitCost,
            record.QuantityOnHand,
            referenceNumber: "CYCLE-COUNT",
            notes: req.Notes ?? "Cycle count adjustment",
            createdBy: req.CreatedBy));

        await _db.SaveChangesAsync(ct);
        return await GetItemAsync(inventoryRecordId, ct);
    }

    public async Task<InventoryItemDto> UpdateThresholdsAsync(
        Guid inventoryRecordId, UpdateThresholdsRequest req, CancellationToken ct = default)
    {
        var record = await LoadRecord(inventoryRecordId, ct);
        record.SetThresholds(req.ReorderPoint, req.MinimumStock, req.MaximumStock, req.Location);
        await _db.SaveChangesAsync(ct);
        return await GetItemAsync(inventoryRecordId, ct);
    }

    public async Task<WarehouseInventoryAllocationDto> GetWarehouseBalancesAsync(
        Guid inventoryRecordId, CancellationToken ct = default)
    {
        var record = await LoadRecord(inventoryRecordId, ct);
        return await BuildWarehouseAllocation(record, ct);
    }

    public async Task<WarehouseInventoryAllocationDto> SetWarehouseBalanceAsync(
        Guid inventoryRecordId, SetWarehouseInventoryBalanceRequest req, CancellationToken ct = default)
    {
        if (req.QuantityOnHand < 0)
            throw new InvalidOperationException("Warehouse quantity cannot be negative.");

        var record = await LoadRecord(inventoryRecordId, ct);
        var warehouse = await _db.Warehouses
            .FirstOrDefaultAsync(w => w.Id == req.WarehouseId && w.OrganizationId == record.OrganizationId, ct)
            ?? throw new InvalidOperationException("Warehouse not found.");
        if (!warehouse.IsActive)
            throw new InvalidOperationException("Inventory cannot be assigned to an inactive warehouse.");

        var location = await _db.WarehouseLocations
            .FirstOrDefaultAsync(l => l.Id == req.WarehouseLocationId && l.WarehouseId == warehouse.Id, ct)
            ?? throw new InvalidOperationException("The selected location does not belong to this warehouse.");
        if (!location.IsActive)
            throw new InvalidOperationException("Inventory cannot be assigned to an inactive location.");

        var balance = await _db.WarehouseInventoryBalances.FirstOrDefaultAsync(
            b => b.ProductVariantId == record.ProductVariantId
                && b.WarehouseId == warehouse.Id
                && b.WarehouseLocationId == location.Id, ct);

        var otherAllocated = await _db.WarehouseInventoryBalances
            .Where(b => b.ProductVariantId == record.ProductVariantId
                && (balance == null || b.Id != balance.Id))
            .SumAsync(b => b.QuantityOnHand, ct);

        if (otherAllocated + req.QuantityOnHand > record.QuantityOnHand)
            throw new InvalidOperationException(
                $"Cannot allocate {req.QuantityOnHand}; only {record.QuantityOnHand - otherAllocated} remains unallocated.");

        if (balance is null)
        {
            balance = new Domain.Modules.WarehouseManagement.WarehouseInventoryBalance(
                record.OrganizationId, record.ProductVariantId, warehouse.Id, location.Id, req.QuantityOnHand);
            _db.WarehouseInventoryBalances.Add(balance);
        }
        else
        {
            balance.SetOnHand(req.QuantityOnHand);
        }

        await _db.SaveChangesAsync(ct);
        return await BuildWarehouseAllocation(record, ct);
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    public async Task<IEnumerable<LowStockItemDto>> GetLowStockAsync(CancellationToken ct = default)
    {
        var records = await _db.InventoryRecords
            .Include(r => r.ProductVariant)
                .ThenInclude(v => v!.Product)
                    .ThenInclude(p => p!.Category)
            .AsNoTracking()
            .Where(r => r.QuantityOnHand <= r.ReorderPoint)
            .OrderBy(r => r.QuantityOnHand)
            .ToListAsync(ct);

        // Get preferred vendor names separately to avoid complex EF navigation
        var vendorIds = records
            .Where(r => r.ProductVariant?.Product?.PreferredVendorId != null)
            .Select(r => r.ProductVariant!.Product!.PreferredVendorId!.Value)
            .Distinct()
            .ToList();

        var vendors = vendorIds.Count > 0
            ? await _db.Vendors.AsNoTracking()
                .Where(v => vendorIds.Contains(v.Id))
                .ToDictionaryAsync(v => v.Id, v => v.Name, ct)
            : new Dictionary<Guid, string>();

        return records.Select(r =>
        {
            var vendorName = r.ProductVariant?.Product?.PreferredVendorId != null
                && vendors.TryGetValue(r.ProductVariant.Product.PreferredVendorId.Value, out var vn) ? vn : null;
            return new LowStockItemDto(
                r.ProductVariantId,
                r.ProductVariant?.Sku ?? "",
                r.ProductVariant?.Product?.Name ?? "",
                BuildVariantDesc(r.ProductVariant),
                r.QuantityOnHand,
                r.ReorderPoint,
                r.QuantityOnOrder,
                vendorName);
        });
    }

    public async Task<IEnumerable<StockValuationDto>> GetValuationByCategory(CancellationToken ct = default)
    {
        var records = await _db.InventoryRecords
            .Include(r => r.ProductVariant)
                .ThenInclude(v => v!.Product)
                    .ThenInclude(p => p!.Category)
            .AsNoTracking()
            .ToListAsync(ct);

        return records
            .GroupBy(r => r.ProductVariant?.Product?.Category?.Name ?? "Uncategorized")
            .Select(g => new StockValuationDto(
                Category:    g.Key,
                SkuCount:    g.Count(),
                TotalOnHand: g.Sum(r => r.QuantityOnHand),
                TotalValue:  g.Sum(r => r.QuantityOnHand * r.AverageCost)))
            .OrderByDescending(v => v.TotalValue);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<InventoryRecord> LoadRecord(Guid id, CancellationToken ct)
    {
        return await _db.InventoryRecords
            .FirstOrDefaultAsync(r => r.Id == id, ct)
            ?? throw new InvalidOperationException("Inventory record not found.");
    }

    private async Task<WarehouseInventoryAllocationDto> BuildWarehouseAllocation(
        InventoryRecord record, CancellationToken ct)
    {
        var balances = await _db.WarehouseInventoryBalances
            .AsNoTracking()
            .Where(b => b.ProductVariantId == record.ProductVariantId)
            .Include(b => b.Warehouse)
            .Include(b => b.WarehouseLocation)
            .OrderBy(b => b.Warehouse!.Code)
            .ThenBy(b => b.WarehouseLocation!.Code)
            .Select(b => new WarehouseInventoryBalanceDto(
                b.Id,
                b.WarehouseId,
                b.Warehouse!.Code,
                b.Warehouse.Name,
                b.WarehouseLocationId,
                b.WarehouseLocation!.Code,
                b.QuantityOnHand,
                b.QuantityReserved,
                b.QuantityOnHand - b.QuantityReserved))
            .ToListAsync(ct);

        var allocated = balances.Sum(b => b.OnHand);
        return new WarehouseInventoryAllocationDto(
            record.QuantityOnHand,
            allocated,
            Math.Max(0m, record.QuantityOnHand - allocated),
            balances);
    }

    private static string? BuildVariantDesc(ProductVariant? v)
    {
        if (v == null) return null;
        var parts = new[] { v.Color, v.Size, v.Material }.Where(s => !string.IsNullOrWhiteSpace(s));
        return string.Join(" / ", parts);
    }

    private static InventoryItemDto ToDto(InventoryRecord r)
    {
        var v = r.ProductVariant;
        var p = v?.Product;
        return new InventoryItemDto(
            Id:                 r.Id,
            ProductVariantId:   r.ProductVariantId,
            Sku:                v?.Sku ?? "",
            ProductName:        p?.Name ?? "",
            VariantDescription: BuildVariantDesc(v),
            Category:           p?.Category?.Name,
            Brand:              p?.Brand?.Name,
            UnitOfMeasure:      p?.UnitOfMeasure ?? "Each",
            OnHand:             r.QuantityOnHand,
            Reserved:           r.QuantityReserved,
            OnOrder:            r.QuantityOnOrder,
            Available:          r.QuantityAvailable,
            Projected:          r.QuantityProjected,
            ReorderPoint:       r.ReorderPoint,
            MinimumStock:       r.MinimumStock,
            MaximumStock:       r.MaximumStock,
            AverageCost:        r.AverageCost,
            StockValue:         r.StockValue,
            Location:           r.Location,
            LastCountDate:      r.LastCountDate,
            LastReceivedDate:   r.LastReceivedDate,
            NeedsReorder:       r.NeedsReorder,
            IsOutOfStock:       r.IsOutOfStock);
    }

    private static InventoryTransactionDto ToTxnDto(InventoryTransaction t) =>
        new(t.Id, t.ProductVariantId, t.ProductVariant?.Sku ?? "",
            t.TransactionType.ToString(), t.Quantity, t.UnitCost, t.BalanceAfter,
            t.ReferenceNumber, t.ReferenceDocumentId, t.Notes, t.CreatedBy, t.TransactionDate);
}
