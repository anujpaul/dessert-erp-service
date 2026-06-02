using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Retail;

public class RetailStore : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public string StoreCode      { get; private set; } = string.Empty;
    public string Name           { get; private set; } = string.Empty;
    public string? Address       { get; private set; }
    public string? Phone         { get; private set; }
    public string? ManagerName   { get; private set; }
    public bool   IsActive       { get; private set; } = true;

    private RetailStore() { }

    public RetailStore(Guid organizationId, string storeCode, string name,
        string? address = null, string? phone = null, string? managerName = null)
    {
        OrganizationId = organizationId;
        StoreCode      = storeCode.Trim().ToUpperInvariant();
        Name           = name.Trim();
        Address        = address?.Trim();
        Phone          = phone?.Trim();
        ManagerName    = managerName?.Trim();
    }

    public void Update(string name, string? address, string? phone, string? managerName)
    {
        Name        = name.Trim();
        Address     = address?.Trim();
        Phone       = phone?.Trim();
        ManagerName = managerName?.Trim();
        SetUpdated();
    }

    public void Activate()   { IsActive = true;  SetUpdated(); }
    public void Deactivate() { IsActive = false; SetUpdated(); }
}
