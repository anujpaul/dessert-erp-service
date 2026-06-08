using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.CashBank;

public enum CashJournalStatus { Draft, Posted, Voided }

/// <summary>
/// Cash Journal — a batch of petty-cash or manual cash entries before GL posting.
/// Each line is a cash receipt or disbursement with a GL account allocation.
/// </summary>
public class CashJournal : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public Guid   BankAccountId  { get; private set; }  // the petty-cash / cash account

    public string JournalNumber  { get; private set; } = string.Empty;
    public DateTime JournalDate  { get; private set; }
    public string   Description  { get; private set; } = string.Empty;
    public CashJournalStatus Status { get; private set; } = CashJournalStatus.Draft;

    public decimal TotalDebits  { get; private set; }
    public decimal TotalCredits { get; private set; }

    public DateTime? PostedAt  { get; private set; }
    public string?   PostedBy  { get; private set; }
    public string?   Notes     { get; private set; }

    public BankAccount? BankAccount { get; private set; }

    private readonly List<CashJournalLine> _lines = new();
    public IReadOnlyCollection<CashJournalLine> Lines => _lines.AsReadOnly();

    private CashJournal() { }

    public CashJournal(Guid organizationId, Guid bankAccountId,
        string journalNumber, DateTime journalDate,
        string description, string? notes = null)
    {
        OrganizationId = organizationId;
        BankAccountId  = bankAccountId;
        JournalNumber  = journalNumber;
        JournalDate    = journalDate;
        Description    = description.Trim();
        Notes          = notes;
    }

    public CashJournalLine AddLine(Guid glAccountId, string description,
        decimal debit, decimal credit, string? reference = null)
    {
        if (Status != CashJournalStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to Draft journals.");
        if (debit < 0 || credit < 0)
            throw new ArgumentException("Debit and credit amounts must be non-negative.");
        if (debit == 0 && credit == 0)
            throw new ArgumentException("A line must have a non-zero debit or credit.");

        var line = new CashJournalLine(Id, glAccountId, description, debit, credit, reference);
        _lines.Add(line);
        Recalculate();
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != CashJournalStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from Draft journals.");
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        Recalculate();
        SetUpdated();
    }

    public void Post(string postedBy)
    {
        if (Status != CashJournalStatus.Draft)
            throw new InvalidOperationException("Only Draft journals can be posted.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot post a journal with no lines.");
        if (Math.Abs(TotalDebits - TotalCredits) > 0.01m)
            throw new InvalidOperationException(
                $"Journal is not balanced. Debits={TotalDebits:C}, Credits={TotalCredits:C}.");
        Status   = CashJournalStatus.Posted;
        PostedAt = DateTime.UtcNow;
        PostedBy = postedBy;
        SetUpdated();
    }

    public void Void()
    {
        if (Status == CashJournalStatus.Voided)
            throw new InvalidOperationException("Already voided.");
        if (Status == CashJournalStatus.Draft)
            throw new InvalidOperationException("Delete a draft instead of voiding.");
        Status = CashJournalStatus.Voided;
        SetUpdated();
    }

    private void Recalculate()
    {
        TotalDebits  = _lines.Sum(l => l.Debit);
        TotalCredits = _lines.Sum(l => l.Credit);
    }
}

public class CashJournalLine : BaseEntity
{
    public Guid   JournalId   { get; private set; }
    public Guid   GLAccountId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Debit      { get; private set; }
    public decimal Credit     { get; private set; }
    public string? Reference  { get; private set; }

    public CashJournal? Journal { get; private set; }

    private CashJournalLine() { }

    public CashJournalLine(Guid journalId, Guid glAccountId,
        string description, decimal debit, decimal credit, string? reference)
    {
        JournalId   = journalId;
        GLAccountId = glAccountId;
        Description = description.Trim();
        Debit       = debit;
        Credit      = credit;
        Reference   = reference;
    }
}
