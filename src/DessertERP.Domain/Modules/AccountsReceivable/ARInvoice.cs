using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum ARInvoiceStatus { Draft, Issued, PartiallyPaid, FullyPaid, Overdue, Voided }

public class ARInvoice : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Guid? SalesOrderId { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal PaidAmount { get; private set; }
    public ARInvoiceStatus Status { get; private set; } = ARInvoiceStatus.Draft;
    public Guid? JournalEntryId      { get; private set; }
    public Guid? WorkflowInstanceId  { get; private set; }

    /// <summary>True when submitted for approval but not yet approved/rejected.</summary>
    public bool IsSubmittedForApproval => WorkflowInstanceId.HasValue && Status == ARInvoiceStatus.Draft;

    public Customer? Customer { get; private set; }
    public SalesOrder? SalesOrder { get; private set; }

    // Computed
    public decimal OutstandingAmount => TotalAmount - PaidAmount;
    public int DaysOutstanding => Status == ARInvoiceStatus.Issued || Status == ARInvoiceStatus.PartiallyPaid || Status == ARInvoiceStatus.Overdue
        ? Math.Max(0, (DateTime.UtcNow.Date - DueDate.Date).Days)
        : 0;

    private ARInvoice() { }

    public ARInvoice(Guid organizationId, string invoiceNumber, Guid customerId, DateTime invoiceDate,
        DateTime dueDate, string description, decimal subTotal, decimal taxAmount,
        decimal discountAmount, Guid? salesOrderId = null)
    {
        OrganizationId = organizationId;
        InvoiceNumber = invoiceNumber;
        CustomerId = customerId;
        SalesOrderId = salesOrderId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        Description = description;
        SubTotal = subTotal;
        TaxAmount = taxAmount;
        DiscountAmount = discountAmount;
        TotalAmount = subTotal - discountAmount + taxAmount;
    }

    /// <summary>Submit for approval workflow. Service creates WorkflowInstance first.</summary>
    public void SubmitForApproval(Guid workflowInstanceId)
    {
        if (Status != ARInvoiceStatus.Draft)
            throw new InvalidOperationException("Only Draft invoices can be submitted for approval.");
        WorkflowInstanceId = workflowInstanceId;
        SetUpdated();
    }

    /// <summary>Called by workflow engine when all steps approve — issues the invoice.</summary>
    public void WorkflowApproved(Guid? journalEntryId = null)
    {
        Issue(journalEntryId);
    }

    /// <summary>Called by workflow engine when a step rejects — keeps invoice as Draft.</summary>
    public void WorkflowRejected()
    {
        WorkflowInstanceId = null;
        SetUpdated();
    }

    public void Issue(Guid? journalEntryId = null)
    {
        if (Status != ARInvoiceStatus.Draft)
            throw new InvalidOperationException("Only a Draft invoice can be issued.");
        Status = ARInvoiceStatus.Issued;
        JournalEntryId = journalEntryId;
        SetUpdated();
    }

    public void ApplyPayment(decimal amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Payment amount must be positive.");
        if (amount > OutstandingAmount) throw new InvalidOperationException("Payment exceeds outstanding balance.");

        PaidAmount += amount;
        Status = PaidAmount >= TotalAmount ? ARInvoiceStatus.FullyPaid : ARInvoiceStatus.PartiallyPaid;
        SetUpdated();
    }

    public void MarkOverdue()
    {
        if (Status == ARInvoiceStatus.Issued || Status == ARInvoiceStatus.PartiallyPaid)
        {
            Status = ARInvoiceStatus.Overdue;
            SetUpdated();
        }
    }

    public void Void()
    {
        if (Status == ARInvoiceStatus.FullyPaid)
            throw new InvalidOperationException("Cannot void a fully paid invoice.");
        Status = ARInvoiceStatus.Voided;
        SetUpdated();
    }
}
