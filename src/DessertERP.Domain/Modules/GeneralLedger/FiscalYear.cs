using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public enum FiscalYearStatus { Open, Closed, OnHold }

public class FiscalYear : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string CalendarType { get; private set; } = "Monthly";
    public FiscalYearStatus Status { get; private set; } = FiscalYearStatus.Open;
    public int PeriodCount { get; private set; }

    private readonly List<FiscalPeriod> _periods = new();
    public IReadOnlyCollection<FiscalPeriod> Periods => _periods.AsReadOnly();

    private FiscalYear() { }

    public FiscalYear(Guid organizationId, string name, string description, DateTime startDate, DateTime endDate, string calendarType = "Monthly")
    {
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        CalendarType = calendarType;
    }

    /// <summary>Add a single user-defined period. Validates it falls within the fiscal year.</summary>
    public FiscalPeriod AddPeriod(string name, DateTime startDate, DateTime endDate)
    {
        if (startDate < StartDate || endDate > EndDate)
            throw new InvalidOperationException($"Period dates must fall within the fiscal year ({StartDate:d} – {EndDate:d}).");
        if (startDate >= endDate)
            throw new InvalidOperationException("Period start date must be before end date.");

        int num = _periods.Count + 1;
        var period = new FiscalPeriod(Id, num, name, startDate, endDate);
        _periods.Add(period);
        PeriodCount = _periods.Count;
        SetUpdated();
        return period;
    }

    /// <summary>Remove a period (only if Open and not yet used for journal entries).</summary>
    public void RemovePeriod(Guid periodId)
    {
        var p = _periods.FirstOrDefault(x => x.Id == periodId)
            ?? throw new InvalidOperationException("Period not found on this fiscal year.");
        if (p.Status != FiscalPeriodStatus.Open)
            throw new InvalidOperationException("Only Open periods can be deleted.");
        _periods.Remove(p);
        // Re-number remaining periods
        int num = 1;
        foreach (var remaining in _periods.OrderBy(x => x.StartDate))
        {
            remaining.SetPeriodNumber(num++);
        }
        PeriodCount = _periods.Count;
        SetUpdated();
    }

    /// <summary>Bulk-generate monthly periods (1st → last day of each month).</summary>
    public void GenerateMonthlyPeriods()
    {
        _periods.Clear();
        var current = StartDate;
        int num = 1;
        while (current <= EndDate)
        {
            var periodEnd = new DateTime(current.Year, current.Month,
                DateTime.DaysInMonth(current.Year, current.Month));
            if (periodEnd > EndDate) periodEnd = EndDate;
            _periods.Add(new FiscalPeriod(Id, num, $"Period {num} - {current:MMM yyyy}", current, periodEnd));
            current = current.AddMonths(1);
            num++;
        }
        PeriodCount = _periods.Count;
        SetUpdated();
    }

    /// <summary>Bulk-generate quarterly periods (4 periods covering the FY).</summary>
    public void GenerateQuarterlyPeriods()
    {
        _periods.Clear();
        var quarterNames = new[] { "Q1", "Q2", "Q3", "Q4" };
        var span = (EndDate - StartDate).TotalDays / 4;
        for (int i = 0; i < 4; i++)
        {
            var start = StartDate.AddDays(Math.Round(span * i));
            var end   = i == 3 ? EndDate : StartDate.AddDays(Math.Round(span * (i + 1))).AddDays(-1);
            _periods.Add(new FiscalPeriod(Id, i + 1, $"{quarterNames[i]} - {start:yyyy}", start, end));
        }
        PeriodCount = _periods.Count;
        SetUpdated();
    }

    /// <summary>Bulk-generate 13 periods using the 4-4-5 retail calendar pattern.</summary>
    public void Generate445Periods()
    {
        _periods.Clear();
        // 4-4-5: 3 periods per quarter, week counts are 4,4,5 — total 52 weeks = 364 days
        int[] weekPattern = { 4, 4, 5, 4, 4, 5, 4, 4, 5, 4, 4, 5 };
        var current = StartDate;
        for (int i = 0; i < 12; i++)
        {
            int days = weekPattern[i] * 7;
            var end = current.AddDays(days - 1);
            if (i == 11) end = EndDate; // snap last period to FY end
            _periods.Add(new FiscalPeriod(Id, i + 1, $"Period {i + 1}", current, end));
            current = end.AddDays(1);
            if (current > EndDate) break;
        }
        PeriodCount = _periods.Count;
        SetUpdated();
    }

    public void Close()
    {
        if (Status != FiscalYearStatus.Open)
            throw new InvalidOperationException("Only an open fiscal year can be closed.");
        Status = FiscalYearStatus.Closed;
        SetUpdated();
    }

    public void PutOnHold()
    {
        if (Status != FiscalYearStatus.Open)
            throw new InvalidOperationException("Only an open fiscal year can be put on hold.");
        Status = FiscalYearStatus.OnHold;
        SetUpdated();
    }

    public void Reopen()
    {
        if (Status == FiscalYearStatus.Closed)
            throw new InvalidOperationException("A closed fiscal year cannot be reopened.");
        Status = FiscalYearStatus.Open;
        SetUpdated();
    }
}
