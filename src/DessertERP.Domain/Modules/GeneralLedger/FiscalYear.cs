using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public enum FiscalYearStatus { Open, Closed, OnHold }

public class FiscalYear : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid FiscalCalendarId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string CalendarType { get; private set; } = FiscalCalendarTypes.Monthly;
    public FiscalYearStatus Status { get; private set; } = FiscalYearStatus.Open;
    public int PeriodCount { get; private set; }
    public FiscalCalendar? FiscalCalendar { get; private set; }

    private readonly List<FiscalPeriod> _periods = [];
    public IReadOnlyCollection<FiscalPeriod> Periods => _periods.AsReadOnly();

    private FiscalYear() { }

    public FiscalYear(
        Guid organizationId,
        Guid fiscalCalendarId,
        string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        string calendarType)
    {
        if (fiscalCalendarId == Guid.Empty) throw new ArgumentException("Fiscal calendar is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Fiscal year name is required.");
        if (startDate >= endDate) throw new ArgumentException("Fiscal year start date must be before end date.");

        OrganizationId = organizationId;
        FiscalCalendarId = fiscalCalendarId;
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        StartDate = startDate;
        EndDate = endDate;
        CalendarType = FiscalCalendarTypes.Normalize(calendarType);
    }

    public FiscalPeriod AddPeriod(string name, DateTime startDate, DateTime endDate)
    {
        if (startDate < StartDate || endDate > EndDate)
            throw new InvalidOperationException($"Period dates must fall within the fiscal year ({StartDate:d} - {EndDate:d}).");
        if (startDate > endDate)
            throw new InvalidOperationException("Period start date must not be after end date.");
        if (_periods.Any(p => startDate <= p.EndDate && endDate >= p.StartDate))
            throw new InvalidOperationException("Fiscal periods cannot overlap.");

        var period = new FiscalPeriod(Id, _periods.Count + 1, name, startDate, endDate);
        _periods.Add(period);
        PeriodCount = _periods.Count;
        SetUpdated();
        return period;
    }

    public void RemovePeriod(Guid periodId)
    {
        var period = _periods.FirstOrDefault(x => x.Id == periodId)
            ?? throw new InvalidOperationException("Period not found on this fiscal year.");
        if (period.Status != FiscalPeriodStatus.Open)
            throw new InvalidOperationException("Only Open periods can be deleted.");

        _periods.Remove(period);
        var number = 1;
        foreach (var remaining in _periods.OrderBy(x => x.StartDate))
            remaining.SetPeriodNumber(number++);

        PeriodCount = _periods.Count;
        SetUpdated();
    }

    public void GenerateMonthlyPeriods()
    {
        _periods.Clear();
        var current = StartDate;
        var number = 1;
        while (current <= EndDate)
        {
            var periodEnd = new DateTime(current.Year, current.Month, DateTime.DaysInMonth(current.Year, current.Month));
            if (periodEnd > EndDate) periodEnd = EndDate;
            _periods.Add(new FiscalPeriod(Id, number, $"Period {number} - {current:MMM yyyy}", current, periodEnd));
            current = periodEnd.AddDays(1);
            number++;
        }
        PeriodCount = _periods.Count;
        SetUpdated();
    }

    public void GenerateQuarterlyPeriods()
    {
        _periods.Clear();
        var quarterNames = new[] { "Q1", "Q2", "Q3", "Q4" };
        var span = (EndDate - StartDate).TotalDays / 4;
        for (var i = 0; i < 4; i++)
        {
            var start = StartDate.AddDays(Math.Round(span * i));
            var end = i == 3 ? EndDate : StartDate.AddDays(Math.Round(span * (i + 1))).AddDays(-1);
            _periods.Add(new FiscalPeriod(Id, i + 1, $"{quarterNames[i]} - {start:yyyy}", start, end));
        }
        PeriodCount = _periods.Count;
        SetUpdated();
    }

    public void Generate445Periods() => GenerateRetailPeriods([4, 4, 5]);
    public void Generate454Periods() => GenerateRetailPeriods([4, 5, 4]);
    public void Generate544Periods() => GenerateRetailPeriods([5, 4, 4]);

    public void GeneratePeriodsForCalendar()
    {
        switch (CalendarType)
        {
            case FiscalCalendarTypes.Monthly: GenerateMonthlyPeriods(); break;
            case FiscalCalendarTypes.Quarterly: GenerateQuarterlyPeriods(); break;
            case FiscalCalendarTypes.Retail445: Generate445Periods(); break;
            case FiscalCalendarTypes.Retail454: Generate454Periods(); break;
            case FiscalCalendarTypes.Retail544: Generate544Periods(); break;
            case FiscalCalendarTypes.Custom: break;
            default: throw new InvalidOperationException($"Unsupported calendar type '{CalendarType}'.");
        }
    }

    private void GenerateRetailPeriods(int[] quarterlyPattern)
    {
        _periods.Clear();
        var weekPattern = Enumerable.Repeat(quarterlyPattern, 4).SelectMany(x => x).ToArray();
        var current = StartDate;
        for (var i = 0; i < weekPattern.Length && current <= EndDate; i++)
        {
            var end = current.AddDays(weekPattern[i] * 7 - 1);
            if (i == weekPattern.Length - 1 || end > EndDate) end = EndDate;
            _periods.Add(new FiscalPeriod(Id, i + 1, $"Period {i + 1}", current, end));
            current = end.AddDays(1);
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
