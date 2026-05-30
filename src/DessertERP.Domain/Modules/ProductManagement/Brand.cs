using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

public class Brand : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Country { get; private set; }
    public string? Website { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Brand() { }

    public Brand(Guid organizationId, string code, string name,
        string? description = null, string? country = null, string? website = null)
    {
        OrganizationId = organizationId;
        Code = code.ToUpperInvariant().Trim();
        Name = name.Trim();
        Description = description;
        Country = country;
        Website = website;
    }

    public void Update(string name, string? description, string? country, string? website, string? logoUrl)
    {
        Name = name.Trim();
        Description = description;
        Country = country;
        Website = website;
        LogoUrl = logoUrl;
        SetUpdated();
    }

    public void Deactivate() { IsActive = false; SetUpdated(); }
    public void Activate()   { IsActive = true;  SetUpdated(); }
}
