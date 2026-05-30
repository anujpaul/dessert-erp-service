using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

public enum VariantStatus { Active, Inactive, Discontinued }

public class ProductVariant : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Sku { get; private set; } = string.Empty;      // Unique per org
    public string? Barcode { get; private set; }                  // UPC / EAN / GTIN
    public string Size { get; private set; } = string.Empty;      // S, M, L, XL / 8, 9, 10 / 32x30
    public string? Color { get; private set; }
    public string? Material { get; private set; }
    public string? AdditionalAttributes { get; private set; }     // JSON for extra attrs
    public decimal? PriceOverride { get; private set; }           // null = use product BasePrice
    public decimal? CostOverride { get; private set; }            // null = use product BaseCost
    public decimal Weight { get; private set; }                   // in lbs
    public VariantStatus Status { get; private set; } = VariantStatus.Active;

    public Product? Product { get; private set; }
    public InventoryRecord? Inventory { get; private set; }

    private ProductVariant() { }

    public ProductVariant(Guid organizationId, Guid productId, string sku,
        string size, string? color, string? material,
        string? barcode = null, decimal? priceOverride = null, decimal? costOverride = null,
        decimal weight = 0m)
    {
        OrganizationId = organizationId;
        ProductId = productId;
        Sku = sku.Trim().ToUpperInvariant();
        Size = size.Trim();
        Color = color?.Trim();
        Material = material?.Trim();
        Barcode = barcode?.Trim();
        PriceOverride = priceOverride;
        CostOverride = costOverride;
        Weight = weight;
    }

    public void UpdatePricing(decimal? priceOverride, decimal? costOverride)
    {
        PriceOverride = priceOverride;
        CostOverride = costOverride;
        SetUpdated();
    }

    public void UpdateBarcode(string? barcode) { Barcode = barcode; SetUpdated(); }

    public void Deactivate()    { Status = VariantStatus.Inactive;      SetUpdated(); }
    public void Activate()      { Status = VariantStatus.Active;        SetUpdated(); }
    public void Discontinue()   { Status = VariantStatus.Discontinued;  SetUpdated(); }

    /// <summary>Effective selling price: variant override OR product base price.</summary>
    public decimal EffectivePrice(decimal productBasePrice)
        => PriceOverride ?? productBasePrice;

    /// <summary>Effective cost: variant override OR product base cost.</summary>
    public decimal EffectiveCost(decimal productBaseCost)
        => CostOverride ?? productBaseCost;
}
