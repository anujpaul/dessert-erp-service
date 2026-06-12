using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class WarehouseType : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private WarehouseType() { }

    public WarehouseType(Guid organizationId, string name, string? description = null)
    {
        OrganizationId = organizationId;
        Name = name.Trim();
        Description = description?.Trim();
    }
}

public class OperationalSite : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? Country { get; private set; }
    public bool IsRetailStore { get; private set; }
    public bool IsFulfillmentCenter { get; private set; }
    public bool IsReturnCenter { get; private set; }
    public bool IsWarehouse { get; private set; }
    public bool IsActive { get; private set; } = true;

    private OperationalSite() { }

    public OperationalSite(Guid organizationId, string code, string name,
        string? address, string? city, string? country, bool isRetailStore,
        bool isFulfillmentCenter, bool isReturnCenter, bool isWarehouse)
    {
        OrganizationId = organizationId;
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        Address = address?.Trim();
        City = city?.Trim();
        Country = country?.Trim();
        IsRetailStore = isRetailStore;
        IsFulfillmentCenter = isFulfillmentCenter;
        IsReturnCenter = isReturnCenter;
        IsWarehouse = isWarehouse;
    }
}
