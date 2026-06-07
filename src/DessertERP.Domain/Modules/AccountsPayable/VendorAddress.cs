using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum VendorAddressType { Billing, RemitTo, Shipping, Other }

public class VendorAddress : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid VendorId { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public VendorAddressType AddressType { get; private set; }
    public bool IsPrimary { get; private set; }
    public string Line1 { get; private set; } = string.Empty;
    public string? Line2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string Country { get; private set; } = "US";

    // Navigation
    public Vendor Vendor { get; private set; } = null!;

    private VendorAddress() { }

    public VendorAddress(Guid organizationId, Guid vendorId, string label,
        VendorAddressType addressType, string line1, string? line2,
        string city, string? state, string? postalCode, string country = "US",
        bool isPrimary = false)
    {
        OrganizationId = organizationId;
        VendorId = vendorId;
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

    public void Update(string label, VendorAddressType addressType, string line1, string? line2,
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
