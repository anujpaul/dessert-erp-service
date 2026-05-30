using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public enum FiscalPeriodStatus { Open, Closed, OnHold }

public class FiscalPeriod : BaseEntity
{
    public Guid FiscalYearId { get; private set; }
    public int PeriodNumber { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public FiscalPeriodStatus Status { get; private set; } = FiscalPeriodStatus.Open;

    public FiscalYear? FiscalYear { get; private set; }

    private FiscalPeriod() { }

    public FiscalPeriod(Guid fiscalYearId, int periodNumber, string name, DateTime startDate, DateTime endDate)
    {
        FiscalYearId = fiscalYearId;
        PeriodNumber = periodNumber;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void Close()
    {
        if (Status == FiscalPeriodStatus.Closed)
            throw new InvalidOperationException("Period is already closed.");
        Status = FiscalPeriodStatus.Closed;
        SetUpdated();
    }

    public void Reopen()
    {
        if (Status != FiscalPeriodStatus.Closed)
            throw new InvalidOperationException("Only a closed period can be reopened.");
        Status = FiscalPeriodStatus.Open;
        SetUpdated();
    }
}
