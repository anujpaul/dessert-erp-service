using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Organization;

public enum OrganizationStatus { Active, Inactive, Suspended }

public class Organization : BaseEntity
{
    public string Code { get; private set; } = string.Empty;       // e.g. "CORP01"
    public string Name { get; private set; } = string.Empty;       // e.g. "Dessert Corp"
    public string BaseCurrency { get; private set; } = "USD";
    public int FiscalYearStartMonth { get; private set; } = 1;     // 1 = January
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? TaxId { get; private set; }
    public string? LogoUrl         { get; private set; }
    public string  DefaultCurrency { get; private set; } = "USD";
    public string? Timezone        { get; private set; }
    public OrganizationStatus Status { get; private set; } = OrganizationStatus.Active;

    private Organization() { }

    public static Organization Create(
        string code,
        string name,
        string baseCurrency = "USD",
        int fiscalYearStartMonth = 1,
        string? address = null,
        string? phone = null,
        string? email = null,
        string? taxId = null)
    {
        if (string.IsNullOrWhiteSpace(code))     throw new ArgumentException("Organization code is required.");
        if (string.IsNullOrWhiteSpace(name))     throw new ArgumentException("Organization name is required.");
        if (fiscalYearStartMonth is < 1 or > 12) throw new ArgumentException("Fiscal year start month must be 1–12.");

        return new Organization
        {
            Code = code.ToUpperInvariant().Trim(),
            Name = name.Trim(),
            BaseCurrency = baseCurrency.ToUpperInvariant(),
            FiscalYearStartMonth = fiscalYearStartMonth,
            Address = address,
            Phone = phone,
            Email = email,
            TaxId = taxId
        };
    }

    public void Update(string name, string? address, string? phone, string? email, string? taxId, string? logoUrl)
    {
        Name    = name.Trim();
        Address = address;
        Phone   = phone;
        Email   = email;
        TaxId   = taxId;
        LogoUrl = logoUrl;
        SetUpdated();
    }

    public void UpdateSettings(string name, string? logoUrl, string defaultCurrency, string? timezone, string? taxId, string? address)
    {
        Name            = name.Trim();
        LogoUrl         = logoUrl;
        DefaultCurrency = defaultCurrency.ToUpperInvariant();
        Timezone        = timezone;
        TaxId           = taxId;
        Address         = address;
        SetUpdated();
    }

    public void Suspend()
    {
        Status = OrganizationStatus.Suspended;
        SetUpdated();
    }

    public void Activate()
    {
        Status = OrganizationStatus.Active;
        SetUpdated();
    }
}
