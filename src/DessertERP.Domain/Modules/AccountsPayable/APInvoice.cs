using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum APInvoiceStatus { Draft, Approved, Scheduled, Paid, Overdue, Voided }

public class APInvoice : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid VendorId { get; private set; }
    public Guid? PurchaseOrderId { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string VendorInvoiceRef { get; private set; } = string.Empty;
    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal PaidAmount { get; private set; }
    public APInvoiceStatus Status { get; private set; } = APInvoiceStatus.Draft;
    public Guid? JournalEntryId { get; private set; }

    public Vendor? Vendor { get; private set; }
    public PurchaseOrder? PurchaseOrder { get; private set; }

    public decimal OutstandingAmount => TotalAmount - PaidAmount;
    public int DaysOutstanding => (Status == APInvoiceStatus.Approved || Status == APInvoiceStatus.Scheduled || Status == APInvoiceStatus.Overdue)
        ? Math.Max(0, (DateTime.UtcNow.Date - DueDate.Date).Days)
        : 0;

    private APInvoice() { }

    public APInvoice(Guid organizationId, string invoiceNumber, Guid vendorId, DateTime invoiceDate,
        DateTime dueDate, string description, string vendorInvoiceRef,
        decimal subTotal, decimal taxAmount, Guid? purchaseOrderId = null)
    {
        OrganizationId = organizationId;
        InvoiceNumber = invoiceNumber;
        VendorId = vendorId;
        PurchaseOrderId = purchaseOrderId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        Description = description;
        VendorInvoiceRef = vendorInvoiceRef;
        SubTotal = subTotal;
        TaxAmount = taxAmount;
        TotalAmount = subTotal + taxAmount;
    }

    public void Approve(Guid? journalEntryId = null)
    {
        if (Status != APInvoiceStatus.Draft)
            throw new InvalidOperationException("Only a Draft invoice can be approved.");
        Status = APInvoiceStatus.Approved;
        JournalEntryId = journalEntryId;
        SetUpdated();
    }

    public void SchedulePayment()
    {
        if (Status != APInvoiceStatus.Approved)
            throw new InvalidOperationException("Invoice must be approved before scheduling payment.");
        Status = APInvoiceStatus.Scheduled;
        SetUpdated();
    }

    public void ApplyPayment(decimal amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Payment amount must be positive.");
        if (amount > OutstandingAmount) throw new InvalidOperationException("Payment exceeds outstanding balance.");

        PaidAmount += amount;
        if (PaidAmount >= TotalAmount)
            Status = APInvoiceStatus.Paid;
        SetUpdated();
    }

    public void MarkOverdue()
    {
        if (Status == APInvoiceStatus.Approved || Status == APInvoiceStatus.Scheduled)
        {
            Status = APInvoiceStatus.Overdue;
            SetUpdated();
        }
    }

    public void Void()
    {
        if (Status == APInvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot void a fully paid invoice.");
        Status = APInvoiceStatus.Voided;
        SetUpdated();
    }
}
