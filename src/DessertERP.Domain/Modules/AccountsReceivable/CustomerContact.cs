using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public class CustomerContact : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Title { get; private set; }          // e.g. "Accounts Payable Manager"
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Mobile { get; private set; }
    public bool IsPrimary { get; private set; }
    public string? Notes { get; private set; }

    // Navigation
    public Customer Customer { get; private set; } = null!;

    private CustomerContact() { }

    public CustomerContact(Guid organizationId, Guid customerId, string name,
        string? title, string? email, string? phone, string? mobile,
        string? notes = null, bool isPrimary = false)
    {
        OrganizationId = organizationId;
        CustomerId = customerId;
        Name = name;
        Title = title;
        Email = email;
        Phone = phone;
        Mobile = mobile;
        IsPrimary = isPrimary;
        Notes = notes;
    }

    public void Update(string name, string? title, string? email, string? phone,
        string? mobile, string? notes)
    {
        Name = name;
        Title = title;
        Email = email;
        Phone = phone;
        Mobile = mobile;
        Notes = notes;
        SetUpdated();
    }

    public void SetPrimary()  { IsPrimary = true;  SetUpdated(); }
    public void ClearPrimary() { IsPrimary = false; SetUpdated(); }
}
