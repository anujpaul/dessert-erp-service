using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

public class Category : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Default tax rate for all products in this category (e.g. 8.25).
    /// Individual products can override this with TaxRateOverride.
    /// </summary>
    public decimal TaxRate { get; private set; }

    /// <summary>
    /// Tax code used by the tax engine to look up jurisdiction-specific rates.
    /// Examples: CLOTHING, FOOTWEAR, FOOD_BASIC, FOOD_PREPARED, DIGITAL, EXEMPT.
    /// Leave blank to inherit general tax rules.
    /// </summary>
    public string? TaxCode { get; private set; }

    public Category? ParentCategory { get; private set; }

    private Category() { }

    public Category(Guid organizationId, string code, string name,
        Guid? parentCategoryId = null, string? description = null, int displayOrder = 0,
        decimal taxRate = 0, string? taxCode = null)
    {
        OrganizationId   = organizationId;
        Code             = code.ToUpperInvariant().Trim();
        Name             = name.Trim();
        ParentCategoryId = parentCategoryId;
        Description      = description;
        DisplayOrder     = displayOrder;
        TaxRate          = taxRate;
        TaxCode          = taxCode?.Trim().ToUpperInvariant();
    }

    public void Update(string name, string? description, int displayOrder,
        decimal taxRate, string? taxCode)
    {
        Name        = name.Trim();
        Description = description;
        DisplayOrder = displayOrder;
        TaxRate      = taxRate;
        TaxCode      = taxCode?.Trim().ToUpperInvariant();
        SetUpdated();
    }

    public void Deactivate() { IsActive = false; SetUpdated(); }
    public void Activate()   { IsActive = true;  SetUpdated(); }
}
