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
