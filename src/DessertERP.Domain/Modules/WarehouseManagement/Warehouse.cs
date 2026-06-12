using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class Warehouse : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public string Code           { get; private set; } = string.Empty;
    public string Name           { get; private set; } = string.Empty;
    public string? Address       { get; private set; }
    public string? City          { get; private set; }
    public string? Country       { get; private set; }
    public Guid?  WarehouseTypeId { get; private set; }
    public WarehouseType? WarehouseType { get; private set; }
    public Guid?  SiteId         { get; private set; }
    public OperationalSite? Site { get; private set; }
    public bool   IsActive       { get; private set; } = true;
    public bool   IsDefault      { get; private set; }

    public ICollection<WarehouseLocation> Locations { get; private set; } = new List<WarehouseLocation>();

    private Warehouse() { }

    public Warehouse(Guid organizationId, string code, string name,
        string? address = null, string? city = null, string? country = null,
        bool isDefault = false, Guid? warehouseTypeId = null, Guid? siteId = null)
    {
        OrganizationId = organizationId;
        Code      = code.Trim().ToUpper();
        Name      = name.Trim();
        Address   = address;
        City      = city;
        Country   = country;
        IsDefault = isDefault;
        WarehouseTypeId = warehouseTypeId;
        SiteId = siteId;
    }

    public void Update(string name, string? address, string? city, string? country,
        Guid? warehouseTypeId, Guid? siteId)
    {
        Name    = name.Trim();
        Address = address;
        City    = city;
        Country = country;
        WarehouseTypeId = warehouseTypeId;
        SiteId = siteId;
        SetUpdated();
    }

    public void Activate()   { IsActive = true;  SetUpdated(); }
    public void Deactivate() { IsActive = false; SetUpdated(); }
    public void SetDefault() { IsDefault = true; SetUpdated(); }
}
