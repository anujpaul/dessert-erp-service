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
    public string? Address { get; private set; }
    public string Currency { get; private set; } = "USD";
    public int PaymentTermsDays { get; private set; } = 30;
    public decimal CreditLimit { get; private set; } = 10000m;
    public CustomerStatus Status { get; private set; } = CustomerStatus.Active;

    private Customer() { }

    public Customer(Guid organizationId, string customerNumber, string name, string? email = null,
        string? phone = null, string? address = null,
        string currency = "USD", int paymentTermsDays = 30, decimal creditLimit = 10000m)
    {
        OrganizationId = organizationId;
        CustomerNumber = customerNumber;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        Currency = currency;
        PaymentTermsDays = paymentTermsDays;
        CreditLimit = creditLimit;
    }

    public void Update(string name, string? email, string? phone, string? address,
        int paymentTermsDays, decimal creditLimit)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        PaymentTermsDays = paymentTermsDays;
        CreditLimit = creditLimit;
        SetUpdated();
    }

    public void SetStatus(CustomerStatus status) { Status = status; SetUpdated(); }
}
