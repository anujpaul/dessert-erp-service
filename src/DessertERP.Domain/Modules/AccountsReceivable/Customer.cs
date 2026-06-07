using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum CustomerStatus { Active, Inactive, OnHold, Blacklisted }

public class Customer : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string CustomerNumber { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }        // legacy — kept for compat
    public string? BillingAddress { get; private set; }
    public string? ShippingAddress { get; private set; }
    public string? Website { get; private set; }
    public string? Notes { get; private set; }
    public string Currency { get; private set; } = "USD";
    public int PaymentTermsDays { get; private set; } = 30;
    public decimal CreditLimit { get; private set; } = 10000m;
    public CustomerStatus Status { get; private set; } = CustomerStatus.Active;

    private Customer() { }

    public Customer(Guid organizationId, string customerNumber, string name, string? email = null,
        string? phone = null, string? address = null,
        string currency = "USD", int paymentTermsDays = 30, decimal creditLimit = 10000m,
        string? billingAddress = null, string? shippingAddress = null,
        string? website = null, string? notes = null)
    {
        OrganizationId = organizationId;
        CustomerNumber = customerNumber;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        BillingAddress = billingAddress ?? address;
        ShippingAddress = shippingAddress ?? address;
        Website = website;
        Notes = notes;
        Currency = currency;
        PaymentTermsDays = paymentTermsDays;
        CreditLimit = creditLimit;
    }

    public void Update(string name, string? email, string? phone, string? billingAddress,
        string? shippingAddress, int paymentTermsDays, decimal creditLimit,
        string? website = null, string? notes = null)
    {
        Name = name;
        Email = email;
        Phone = phone;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        Address = billingAddress;  // keep legacy in sync
        Website = website;
        Notes = notes;
        PaymentTermsDays = paymentTermsDays;
        CreditLimit = creditLimit;
        SetUpdated();
    }

    public void SetStatus(CustomerStatus status) { Status = status; SetUpdated(); }

    // Navigation — loaded via .Include() in queries
    public ICollection<CustomerAddress> Addresses { get; private set; } = new List<CustomerAddress>();
    public ICollection<CustomerContact> Contacts  { get; private set; } = new List<CustomerContact>();
}
