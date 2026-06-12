using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public static class FiscalCalendarTypes
{
    public const string Monthly = "Monthly";
    public const string Quarterly = "Quarterly";
    public const string Retail445 = "4-4-5";
    public const string Retail454 = "4-5-4";
    public const string Retail544 = "5-4-4";
    public const string Custom = "Custom";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(
        [Monthly, Quarterly, Retail445, Retail454, Retail544, Custom],
        StringComparer.OrdinalIgnoreCase);

    public static string Normalize(string value)
    {
        var match = All.FirstOrDefault(type =>
            string.Equals(type, value?.Trim(), StringComparison.OrdinalIgnoreCase));
        return match ?? throw new ArgumentException(
            $"Unsupported fiscal calendar type '{value}'. Valid values: {string.Join(", ", All)}.");
    }
}

public class FiscalCalendar : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string CalendarType { get; private set; } = FiscalCalendarTypes.Monthly;
    public bool IsDefault { get; private set; }

    private readonly List<FiscalYear> _fiscalYears = [];
    public IReadOnlyCollection<FiscalYear> FiscalYears => _fiscalYears.AsReadOnly();

    private FiscalCalendar() { }

    public FiscalCalendar(
        Guid organizationId,
        string name,
        string description,
        string calendarType,
        bool isDefault = false)
    {
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Calendar name is required.");

        OrganizationId = organizationId;
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        CalendarType = FiscalCalendarTypes.Normalize(calendarType);
        IsDefault = isDefault;
    }

    public void SetDefault(bool isDefault)
    {
        IsDefault = isDefault;
        SetUpdated();
    }
}
