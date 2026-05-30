using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.GeneralLedger;

public class JournalLine : BaseEntity
{
    public Guid JournalEntryId { get; private set; }
    public Guid AccountId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Debit { get; private set; }
    public decimal Credit { get; private set; }
    public int LineOrder { get; private set; }

    public JournalEntry? JournalEntry { get; private set; }
    public Account? Account { get; private set; }

    private JournalLine() { }

    public JournalLine(Guid journalEntryId, Guid accountId, string description,
        decimal debit, decimal credit, int lineOrder)
    {
        JournalEntryId = journalEntryId;
        AccountId = accountId;
        Description = description;
        Debit = debit;
        Credit = credit;
        LineOrder = lineOrder;
    }
}
