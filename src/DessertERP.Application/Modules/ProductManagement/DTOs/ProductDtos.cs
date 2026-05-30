namespace DessertERP.Application.Modules.ProductManagement.DTOs;

// ── Category ──────────────────────────────────────────────────────────────────

public record CategoryDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    Guid? ParentCategoryId,
    string? ParentCategoryName,
    int DisplayOrder,
    bool IsActive,
    int ProductCount
);

public record CreateCategoryRequest(
    string Code,
    string Name,
    string? Description = null,
    Guid? ParentCategoryId = null,
    int DisplayOrder = 0
);

// ── Brand ─────────────────────────────────────────────────────────────────────

public record BrandDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    string? Country,
    string? Website,
    string? LogoUrl,
    bool IsActive,
    int ProductCount
);

public record CreateBrandRequest(
    string Code,
    string Name,
    string? Description = null,
    string? Country = null,
    string? Website = null
);

// ── Product ───────────────────────────────────────────────────────────────────

public record ProductSummaryDto(
    Guid Id,
    string Sku,
    string Name,
    string CategoryName,
    string? BrandName,
    string ProductType,
    string GenderTarget,
    decimal BasePrice,
    string Currency,
    string Status,
    int VariantCount,
    decimal TotalStock
);

public record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    string? LongDescription,
    Guid CategoryId,
    string CategoryName,
    Guid? BrandId,
    string? BrandName,
    string ProductType,
    string GenderTarget,
    string UnitOfMeasure,
    decimal BasePrice,
    decimal BaseCost,
    decimal TaxRate,
    string Currency,
    string? Tags,
    string? ImageUrl,
    string Status,
    Guid? PreferredVendorId,
    string? PreferredVendorName,
    DateTime CreatedAt,
    IEnumerable<ProductVariantDto> Variants
);

public record CreateProductRequest(
    string Sku,
    string Name,
    Guid CategoryId,
    string ProductType,
    decimal BasePrice,
    decimal BaseCost,
    decimal TaxRate,
    string UnitOfMeasure = "Each",
    Guid? BrandId = null,
    string GenderTarget = "Unisex",
    string? Description = null,
    string? Tags = null,
    string Currency = "USD"
);

public record UpdateProductRequest(
    string Name,
    string? Description,
    string? LongDescription,
    Guid CategoryId,
    Guid? BrandId,
    decimal BasePrice,
    decimal BaseCost,
    decimal TaxRate,
    string ProductType,
    string GenderTarget,
    string? Tags,
    string? ImageUrl
);

// ── Variant ───────────────────────────────────────────────────────────────────

public record ProductVariantDto(
    Guid Id,
    Guid ProductId,
    string Sku,
    string? Barcode,
    string Size,
    string? Color,
    string? Material,
    decimal? PriceOverride,
    decimal? CostOverride,
    decimal EffectivePrice,
    decimal EffectiveCost,
    decimal Weight,
    string Status,
    // inventory
    decimal QuantityOnHand,
    decimal QuantityReserved,
    decimal QuantityAvailable,
    decimal ReorderPoint,
    bool NeedsReorder,
    string? Location
);

public record CreateVariantRequest(
    string Size,
    string? Color = null,
    string? Material = null,
    string? Barcode = null,
    decimal? PriceOverride = null,
    decimal? CostOverride = null,
    decimal Weight = 0m,
    // initial inventory
    decimal InitialStock = 0m,
    decimal ReorderPoint = 5m,
    decimal MinimumStock = 0m,
    decimal MaximumStock = 100m,
    string? Location = null
);

public record UpdateVariantPricingRequest(
    decimal? PriceOverride,
    decimal? CostOverride
);

// ── Inventory ─────────────────────────────────────────────────────────────────

public record InventoryDto(
    Guid Id,
    Guid ProductVariantId,
    string VariantSku,
    string ProductName,
    string Size,
    string? Color,
    decimal QuantityOnHand,
    decimal QuantityReserved,
    decimal QuantityAvailable,
    decimal ReorderPoint,
    decimal MinimumStock,
    decimal MaximumStock,
    bool NeedsReorder,
    string? Location,
    DateTime? LastCountDate
);

public record AdjustInventoryRequest(
    decimal Delta,
    string Reason
);

public record SetInventoryRequest(
    decimal Quantity,
    DateTime CountDate
);

public record UpdateThresholdsRequest(
    decimal ReorderPoint,
    decimal MinimumStock,
    decimal MaximumStock,
    string? Location
);

// ── Lookup (for AR sales order line picker + AP PO line picker) ───────────────

public record VariantLookupDto(
    Guid VariantId,
    string Sku,
    string? Barcode,
    string ProductName,
    string Size,
    string? Color,
    string? Material,
    decimal EffectivePrice,
    decimal EffectiveCost,   // used by AP to pre-fill unit cost
    decimal TaxRate,
    string UnitOfMeasure,
    decimal QuantityAvailable,
    Guid? PreferredVendorId,
    string? PreferredVendorName
);

// ── Preferred Vendor ──────────────────────────────────────────────────────────
public record SetPreferredVendorRequest(Guid? VendorId);
