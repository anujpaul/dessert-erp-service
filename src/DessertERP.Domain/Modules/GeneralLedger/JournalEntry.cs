using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public enum JournalEntryStatus { Draft, Posted, Voided }

public class JournalEntry : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string EntryNumber { get; private set; } = string.Empty;
    public DateTime EntryDate { get; private set; }
    public Guid FiscalPeriodId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Reference { get; private set; } = string.Empty;
    public string JournalType { get; private set; } = "General";
    public JournalEntryStatus Status { get; private set; } = JournalEntryStatus.Draft;
    public string Currency { get; private set; } = "USD";
    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private set; }

    public FiscalPeriod? FiscalPeriod { get; private set; }

    private readonly List<JournalLine> _lines = new();
    public IReadOnlyCollection<JournalLine> Lines => _lines.AsReadOnly();

    private JournalEntry() { }

    public JournalEntry(Guid organizationId, string entryNumber, DateTime entryDate, Guid fiscalPeriodId,
        string description, string reference, string journalType = "General", string currency = "USD")
    {
        OrganizationId = organizationId;
        EntryNumber = entryNumber;
        EntryDate = entryDate;
        FiscalPeriodId = fiscalPeriodId;
        Description = description;
        Reference = reference;
        JournalType = journalType;
        Currency = currency;
    }

    public void AddLine(Guid accountId, string description, decimal debit, decimal credit)
    {
        if (Status != JournalEntryStatus.Draft)
            throw new InvalidOperationException("Can only add lines to a draft journal entry.");
        if (debit < 0 || credit < 0)
            throw new InvalidOperationException("Debit and credit amounts must be non-negative.");
        if (debit > 0 && credit > 0)
            throw new InvalidOperationException("A line cannot have both debit and credit amounts.");

        _lines.Add(new JournalLine(Id, accountId, description, debit, credit, _lines.Count + 1));
        Recalc();
        SetUpdated();
    }

    public void Post()
    {
        if (Status != JournalEntryStatus.Draft)
            throw new InvalidOperationException("Only a draft entry can be posted.");
        if (_lines.Count < 2)
            throw new InvalidOperationException("A journal entry must have at least two lines.");
        if (TotalDebit != TotalCredit)
            throw new InvalidOperationException($"Debits ({TotalDebit}) must equal credits ({TotalCredit}).");

        Status = JournalEntryStatus.Posted;
        SetUpdated();
    }

    public void Void()
    {
        if (Status != JournalEntryStatus.Posted)
            throw new InvalidOperationException("Only a posted entry can be voided.");
        Status = JournalEntryStatus.Voided;
        SetUpdated();
    }

    private void Recalc()
    {
        TotalDebit = _lines.Sum(l => l.Debit);
        TotalCredit = _lines.Sum(l => l.Credit);
    }
}
