namespace DessertERP.Application.Modules.InventoryManagement.DTOs;

// ── Summary / list ──────────────────────────────────────────────────────────

public record InventoryItemDto(
    Guid   Id,                // InventoryRecord.Id
    Guid   ProductVariantId,
    string Sku,
    string ProductName,
    string? VariantDescription,  // "Blue / XL"
    string? Category,
    string? Brand,
    string UnitOfMeasure,

    // Quantity buckets
    decimal OnHand,
    decimal Reserved,           // committed to sales orders
    decimal OnOrder,            // open POs not yet received
    decimal Available,          // OnHand - Reserved
    decimal Projected,          // OnHand + OnOrder - Reserved

    // Thresholds
    decimal ReorderPoint,
    decimal MinimumStock,
    decimal MaximumStock,

    // Costing
    decimal AverageCost,
    decimal StockValue,         // OnHand * AverageCost

    // Metadata
    string? Location,
    DateTime? LastCountDate,
    DateTime? LastReceivedDate,

    // Status flags
    bool NeedsReorder,
    bool IsOutOfStock
);

public record InventorySummaryDto(
    int  TotalSkus,
    int  LowStockCount,
    int  OutOfStockCount,
    decimal TotalStockValue,
    int  OnOrderLines         // distinct SKUs with OnOrder > 0
);

public record PagedInventoryItemsDto(
    IReadOnlyList<InventoryItemDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record InventoryFilterOptionsDto(
    IReadOnlyList<string> Categories,
    IReadOnlyList<string> Brands,
    IReadOnlyList<string> Locations
);

// ── Transaction ledger ──────────────────────────────────────────────────────

public record InventoryTransactionDto(
    Guid   Id,
    Guid   ProductVariantId,
    string Sku,
    string TransactionType,
    decimal Quantity,
    decimal UnitCost,
    decimal BalanceAfter,
    string? ReferenceNumber,
    Guid?  ReferenceDocumentId,
    string? Notes,
    string? CreatedBy,
    DateTime TransactionDate
);

// ── Request types ───────────────────────────────────────────────────────────

public record AdjustStockRequest(
    /// <summary>Positive = add stock, Negative = remove stock.</summary>
    decimal Quantity,
    /// <summary>"AdjustmentIn" or "AdjustmentOut"</summary>
    string  AdjustmentType,
    decimal UnitCost,
    string? Notes,
    string? CreatedBy
);

public record SetOnHandRequest(
    decimal Quantity,
    decimal UnitCost,
    string? Notes,
    string? CreatedBy
);

public record UpdateThresholdsRequest(
    decimal ReorderPoint,
    decimal MinimumStock,
    decimal MaximumStock,
    string? Location
);

public record WarehouseInventoryBalanceDto(
    Guid Id,
    Guid WarehouseId,
    string WarehouseCode,
    string WarehouseName,
    Guid WarehouseLocationId,
    string LocationCode,
    decimal OnHand,
    decimal Reserved,
    decimal Available
);

public record WarehouseInventoryAllocationDto(
    decimal TotalOnHand,
    decimal AllocatedOnHand,
    decimal UnallocatedOnHand,
    IReadOnlyList<WarehouseInventoryBalanceDto> Balances
);

public record SetWarehouseInventoryBalanceRequest(
    Guid WarehouseId,
    Guid WarehouseLocationId,
    decimal QuantityOnHand
);

// ── Reports ─────────────────────────────────────────────────────────────────

public record LowStockItemDto(
    Guid   ProductVariantId,
    string Sku,
    string ProductName,
    string? VariantDescription,
    decimal OnHand,
    decimal ReorderPoint,
    decimal OnOrder,
    string? PreferredVendorName
);

public record StockValuationDto(
    string Category,
    int    SkuCount,
    decimal TotalOnHand,
    decimal TotalValue
);
