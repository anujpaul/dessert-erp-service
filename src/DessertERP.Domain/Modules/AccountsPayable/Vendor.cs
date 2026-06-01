using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum VendorStatus { Active, Inactive, OnHold, Blacklisted }

public class Vendor : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string VendorNumber { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public string Currency { get; private set; } = "USD";
    public int PaymentTermsDays { get; private set; } = 30;
    public string? TaxId { get; private set; }
    public string? BankAccountName { get; private set; }
    public string? BankAccountNumber { get; private set; }
    public VendorStatus Status { get; private set; } = VendorStatus.Active;

    // Export tracking
    public bool      IsExported { get; private set; }
    public DateTime? ExportedAt { get; private set; }

    private Vendor() { }

    public Vendor(Guid organizationId, string vendorNumber, string name, string? email = null,
        string? phone = null, string? address = null,
        string currency = "USD", int paymentTermsDays = 30, string? taxId = null)
    {
        OrganizationId = organizationId;
        VendorNumber = vendorNumber;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        Currency = currency;
        PaymentTermsDays = paymentTermsDays;
        TaxId = taxId;
    }

    public void Update(string name, string? email, string? phone, string? address,
        int paymentTermsDays, string? bankAccountName, string? bankAccountNumber)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        PaymentTermsDays = paymentTermsDays;
        BankAccountName = bankAccountName;
        BankAccountNumber = bankAccountNumber;
        SetUpdated();
    }

    public void SetStatus(VendorStatus status) { Status = status; SetUpdated(); }

    public void MarkExported() { IsExported = true; ExportedAt = DateTime.UtcNow; SetUpdated(); }
    public void ResetExport()  { IsExported = false; ExportedAt = null; SetUpdated(); }
}
