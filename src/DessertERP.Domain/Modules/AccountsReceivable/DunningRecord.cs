using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum DunningLevel    { Reminder = 1, FirstNotice = 2, FinalNotice = 3, LegalAction = 4 }
public enum DunningStatus   { Sent, Resolved, Escalated, Ignored }

/// <summary>
/// Dunning Record — a collections notice sent to a customer for overdue invoices.
/// Each escalation creates a new record with a higher DunningLevel.
/// </summary>
public class DunningRecord : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public Guid   CustomerId     { get; private set; }
    public Guid   ARInvoiceId    { get; private set; }
    public string DunningNumber  { get; private set; } = string.Empty;

    public DunningLevel  Level   { get; private set; }
    public DunningStatus Status  { get; private set; } = DunningStatus.Sent;

    public DateTime SentDate     { get; private set; }
    public DateTime FollowUpDate { get; private set; }  // expected response by
    public decimal  OutstandingAmount { get; private set; }

    public string?  Notes        { get; private set; }
    public string?  AssignedTo   { get; private set; }  // collections agent
    public DateTime? ResolvedAt  { get; private set; }
    public string?  ResolutionNotes { get; private set; }

    public Customer?  Customer  { get; private set; }
    public ARInvoice? ARInvoice { get; private set; }

    private DunningRecord() { }

    public DunningRecord(Guid organizationId, string dunningNumber, Guid customerId,
        Guid arInvoiceId, DunningLevel level, DateTime sentDate, DateTime followUpDate,
        decimal outstandingAmount, string? assignedTo = null, string? notes = null)
    {
        OrganizationId    = organizationId;
        DunningNumber     = dunningNumber.Trim();
        CustomerId        = customerId;
        ARInvoiceId       = arInvoiceId;
        Level             = level;
        SentDate          = sentDate;
        FollowUpDate      = followUpDate;
        OutstandingAmount = outstandingAmount;
        AssignedTo        = assignedTo;
        Notes             = notes;
    }

    public void Resolve(string? resolutionNotes = null)
    {
        Status          = DunningStatus.Resolved;
        ResolvedAt      = DateTime.UtcNow;
        ResolutionNotes = resolutionNotes;
        SetUpdated();
    }

    public void Escalate()
    {
        Status = DunningStatus.Escalated;
        SetUpdated();
    }

    public void MarkIgnored()
    {
        Status = DunningStatus.Ignored;
        SetUpdated();
    }

    public void Reassign(string agent) { AssignedTo = agent; SetUpdated(); }
}
