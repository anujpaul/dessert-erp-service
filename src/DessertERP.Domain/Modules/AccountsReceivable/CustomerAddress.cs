using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum AddressType { Billing, Shipping, Other }

public class CustomerAddress : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Label { get; private set; } = string.Empty;   // e.g. "Head Office", "Warehouse"
    public AddressType AddressType { get; private set; }
    public bool IsPrimary { get; private set; }
    public string Line1 { get; private set; } = string.Empty;
    public string? Line2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string Country { get; private set; } = "US";

    // Navigation
    public Customer Customer { get; private set; } = null!;

    private CustomerAddress() { }

    public CustomerAddress(Guid organizationId, Guid customerId, string label,
        AddressType addressType, string line1, string? line2,
        string city, string? state, string? postalCode, string country = "US",
        bool isPrimary = false)
    {
        OrganizationId = organizationId;
        CustomerId = customerId;
        Label = label;
        AddressType = addressType;
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        IsPrimary = isPrimary;
    }

    public void Update(string label, AddressType addressType, string line1, string? line2,
        string city, string? state, string? postalCode, string country)
    {
        Label = label;
        AddressType = addressType;
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        SetUpdated();
    }

    public void SetPrimary()  { IsPrimary = true;  SetUpdated(); }
    public void ClearPrimary() { IsPrimary = false; SetUpdated(); }

    // Computed — ignored by EF (configured via b.Ignore(e => e.SingleLine))
    public string SingleLine =>
        string.Join(", ", new[] { Line1, Line2, City, State, PostalCode, Country }
            .Where(s => !string.IsNullOrWhiteSpace(s)));
}
