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

    public Category? ParentCategory { get; private set; }

    private Category() { }

    public Category(Guid organizationId, string code, string name,
        Guid? parentCategoryId = null, string? description = null, int displayOrder = 0)
    {
        OrganizationId = organizationId;
        Code = code.ToUpperInvariant().Trim();
        Name = name.Trim();
        ParentCategoryId = parentCategoryId;
        Description = description;
        DisplayOrder = displayOrder;
    }

    public void Update(string name, string? description, int displayOrder)
    {
        Name = name.Trim();
        Description = description;
        DisplayOrder = displayOrder;
        SetUpdated();
    }

    public void Deactivate() { IsActive = false; SetUpdated(); }
    public void Activate()   { IsActive = true;  SetUpdated(); }
}
