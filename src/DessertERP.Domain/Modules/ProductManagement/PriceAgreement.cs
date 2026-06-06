using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

/// <summary>Level this agreement targets — Item (product) or SKU (variant).</summary>
public enum AgreementLevel { Product, Variant }

/// <summary>Which price this agreement sets — the selling price or the cost.</summary>
public enum AgreementPriceType { SalesPrice, Cost }

/// <summary>
/// Trade / Price Agreement — overrides the Sales Price or Cost for a
/// specific Product (Item) or Variant (SKU) over a date range.
///
/// Resolution priority when both Product-level and Variant-level
/// agreements exist for the same price type, Variant wins.
/// </summary>
public class PriceAgreement : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public string Name           { get; private set; } = string.Empty;
    public string? Notes         { get; private set; }

    // Target level: Product (Item) or Variant (SKU)
    public AgreementLevel     Level     { get; private set; }
    public Guid?              ProductId { get; private set; }   // if Level = Product
    public Guid?              VariantId { get; private set; }   // if Level = Variant

    // Which price this agreement sets and its fixed value
    public AgreementPriceType PriceType { get; private set; }
    public decimal            Value     { get; private set; }   // the fixed price / cost
    public string             Currency  { get; private set; } = "USD";

    // Validity
    public DateTime StartDate { get; private set; }
    public DateTime EndDate   { get; private set; }             // default 2159-12-31

    public bool IsActive { get; private set; } = true;

    /// <summary>Variant-level wins over Product-level (higher = wins).</summary>
    public int Priority { get; private set; }

    // Navigation
    public Product?        Product { get; private set; }
    public ProductVariant? Variant { get; private set; }

    private PriceAgreement() { }

    public PriceAgreement(
        Guid orgId, string name, AgreementLevel level,
        AgreementPriceType priceType, decimal value, string currency,
        DateTime startDate, DateTime? endDate = null,
        Guid? productId = null, Guid? variantId = null,
        string? notes = null)
    {
        OrganizationId = orgId;
        Name      = name.Trim();
        Notes     = notes;
        Level     = level;
        PriceType = priceType;
        Value     = value;
        Currency  = currency;
        StartDate = startDate.Date;
        EndDate   = (endDate ?? new DateTime(2159, 12, 31)).Date;
        ProductId = productId;
        VariantId = variantId;

        Priority = level switch
        {
            AgreementLevel.Variant => 20,
            AgreementLevel.Product => 10,
            _ => 10
        };
    }

    public void Update(string name, AgreementPriceType priceType, decimal value,
        DateTime startDate, DateTime endDate, bool isActive, string? notes)
    {
        Name      = name.Trim();
        PriceType = priceType;
        Value     = value;
        StartDate = startDate.Date;
        EndDate   = endDate.Date;
        IsActive  = isActive;
        Notes     = notes;
        SetUpdated();
    }

    public void Deactivate() { IsActive = false; SetUpdated(); }
    public void Activate()   { IsActive = true;  SetUpdated(); }

    /// <summary>
    /// Returns the fixed agreed value (sales price or cost — both are stored as a fixed amount).
    /// </summary>
    public decimal ResolveValue() => Value;

    /// <summary>Returns true if this agreement is in effect on the given date.</summary>
    public bool IsValidOn(DateTime date) =>
        IsActive && date.Date >= StartDate && date.Date <= EndDate;
}
