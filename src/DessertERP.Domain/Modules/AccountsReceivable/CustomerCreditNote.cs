using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum ARCreditNoteStatus  { Draft, PendingApproval, Issued, Applied, Voided }
public enum ARCreditNoteReason  { Return, PriceCorrection, ServiceFailure, Dispute, Goodwill, Other }

/// <summary>
/// Customer Credit Note (AR Credit Memo) — reduces a customer's outstanding balance.
/// Issued for returns, price corrections, or dispute resolutions.
/// Lifecycle: Draft → (optional workflow) PendingApproval → Issued → Applied / Voided.
/// </summary>
public class CustomerCreditNote : BaseEntity
{
    public Guid   OrganizationId    { get; private set; }
    public string CreditNoteNumber  { get; private set; } = string.Empty;
    public Guid   CustomerId        { get; private set; }
    public Guid?  ARInvoiceId       { get; private set; }  // the invoice being corrected
    public Guid?  SalesOrderId      { get; private set; }  // the originating SO (returns)

    public DateTime CreditDate      { get; private set; }
    public string   Description     { get; private set; } = string.Empty;
    public string?  CustomerRef     { get; private set; }  // customer's own CN reference

    public decimal  SubTotal        { get; private set; }
    public decimal  TaxAmount       { get; private set; }
    public decimal  TotalAmount     { get; private set; }
    public decimal  AppliedAmount   { get; private set; }
    public decimal  AvailableCredit => TotalAmount - AppliedAmount;

    public ARCreditNoteStatus Status { get; private set; } = ARCreditNoteStatus.Draft;
    public ARCreditNoteReason Reason { get; private set; } = ARCreditNoteReason.Other;
    public string?  Notes           { get; private set; }

    // Workflow
    public Guid? WorkflowInstanceId { get; private set; }

    public Customer?   Customer   { get; private set; }
    public ARInvoice?  ARInvoice  { get; private set; }

    private CustomerCreditNote() { }

    public CustomerCreditNote(Guid organizationId, string creditNoteNumber,
        Guid customerId, DateTime creditDate, string description,
        decimal subTotal, decimal taxAmount,
        ARCreditNoteReason reason = ARCreditNoteReason.Other,
        Guid? arInvoiceId = null, Guid? salesOrderId = null,
        string? customerRef = null, string? notes = null)
    {
        OrganizationId  = organizationId;
        CreditNoteNumber = creditNoteNumber.Trim();
        CustomerId      = customerId;
        CreditDate      = creditDate;
        Description     = description.Trim();
        SubTotal        = subTotal;
        TaxAmount       = taxAmount;
        TotalAmount     = subTotal + taxAmount;
        Reason          = reason;
        ARInvoiceId     = arInvoiceId;
        SalesOrderId    = salesOrderId;
        CustomerRef     = customerRef;
        Notes           = notes;
    }

    /// <summary>Submit for approval workflow before issuing to customer.</summary>
    public void SubmitForApproval(Guid workflowInstanceId)
    {
        if (Status != ARCreditNoteStatus.Draft)
            throw new InvalidOperationException("Only Draft credit notes can be submitted for approval.");
        Status             = ARCreditNoteStatus.PendingApproval;
        WorkflowInstanceId = workflowInstanceId;
        SetUpdated();
    }

    /// <summary>Called by workflow engine when all steps approve.</summary>
    public void WorkflowApproved() => Issue();

    /// <summary>Called by workflow engine when a step rejects.</summary>
    public void WorkflowRejected()
    {
        Status             = ARCreditNoteStatus.Draft;
        WorkflowInstanceId = null;
        SetUpdated();
    }

    /// <summary>Issue the credit note directly (no workflow required).</summary>
    public void Issue()
    {
        if (Status != ARCreditNoteStatus.Draft && Status != ARCreditNoteStatus.PendingApproval)
            throw new InvalidOperationException("Only Draft or PendingApproval credit notes can be issued.");
        Status = ARCreditNoteStatus.Issued;
        SetUpdated();
    }

    /// <summary>Apply credit against an AR invoice balance.</summary>
    public void ApplyCredit(decimal amount)
    {
        if (Status != ARCreditNoteStatus.Issued)
            throw new InvalidOperationException("Credit note must be Issued before applying.");
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        if (amount > AvailableCredit + 0.01m)
            throw new InvalidOperationException($"Cannot apply {amount:C} — only {AvailableCredit:C} available.");

        AppliedAmount += amount;
        if (AvailableCredit <= 0.01m)
            Status = ARCreditNoteStatus.Applied;
        SetUpdated();
    }

    public void Void()
    {
        if (Status == ARCreditNoteStatus.Applied)
            throw new InvalidOperationException("Cannot void a fully-applied credit note.");
        if (Status == ARCreditNoteStatus.Voided)
            throw new InvalidOperationException("Already voided.");
        Status = ARCreditNoteStatus.Voided;
        SetUpdated();
    }
}
