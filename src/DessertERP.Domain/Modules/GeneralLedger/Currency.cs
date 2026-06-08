using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

/// <summary>
/// Represents an ISO 4217 currency managed per organisation.
/// One currency per org is flagged IsBase (exchange rate = 1.0).
/// All other currencies carry a rate relative to the base currency.
/// </summary>
public class Currency : BaseEntity
{
    public Guid   OrganizationId { get; private set; }

    /// <summary>ISO 4217 code, e.g. "USD", "EUR".</summary>
    public string Code           { get; private set; } = string.Empty;

    /// <summary>Full name, e.g. "US Dollar".</summary>
    public string Name           { get; private set; } = string.Empty;

    /// <summary>Currency symbol, e.g. "$", "€".</summary>
    public string Symbol         { get; private set; } = string.Empty;

    /// <summary>Number of decimal places for this currency (0 for JPY, 2 for USD).</summary>
    public int    DecimalPlaces  { get; private set; } = 2;

    /// <summary>Units of this currency equal to 1 unit of the base currency.</summary>
    public decimal ExchangeRate  { get; private set; } = 1m;

    /// <summary>True for exactly one currency per organisation — the functional currency.</summary>
    public bool   IsBase         { get; private set; }

    public bool   IsActive       { get; private set; } = true;

    /// <summary>ISO 4217 numeric code, e.g. 840 for USD.</summary>
    public int?   NumericCode    { get; private set; }

    /// <summary>Optional: which country/region primarily uses this currency.</summary>
    public string? Country       { get; private set; }

    /// <summary>Date/time the exchange rate was last updated.</summary>
    public DateTime? RateUpdatedAt { get; private set; }

    private Currency() { }

    public Currency(
        Guid    organizationId,
        string  code,
        string  name,
        string  symbol,
        int     decimalPlaces  = 2,
        decimal exchangeRate   = 1m,
        bool    isBase         = false,
        int?    numericCode    = null,
        string? country        = null)
    {
        OrganizationId = organizationId;
        Code           = code.ToUpperInvariant();
        Name           = name;
        Symbol         = symbol;
        DecimalPlaces  = decimalPlaces;
        ExchangeRate   = exchangeRate;
        IsBase         = isBase;
        NumericCode    = numericCode;
        Country        = country;
    }

    // ── Domain methods ────────────────────────────────────────────────────────

    public void UpdateExchangeRate(decimal newRate)
    {
        if (IsBase)
            throw new InvalidOperationException("Cannot change the exchange rate of the base currency.");
        if (newRate <= 0)
            throw new ArgumentException("Exchange rate must be positive.", nameof(newRate));

        ExchangeRate  = newRate;
        RateUpdatedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void UpdateDetails(string name, string symbol, int decimalPlaces, string? country, int? numericCode)
    {
        Name          = name;
        Symbol        = symbol;
        DecimalPlaces = decimalPlaces;
        Country       = country;
        NumericCode   = numericCode;
        SetUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdated();
    }

    public void Deactivate()
    {
        if (IsBase)
            throw new InvalidOperationException("Cannot deactivate the base currency.");
        IsActive = false;
        SetUpdated();
    }

    public void SetAsBase()
    {
        IsBase       = true;
        ExchangeRate = 1m;
        IsActive     = true;
        SetUpdated();
    }
}
