using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum APPaymentStatus { Draft, Posted, Voided }

public class APPayment : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string PaymentNumber { get; private set; } = string.Empty;
    public Guid VendorId { get; private set; }
    public Guid APInvoiceId { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public decimal Amount { get; private set; }
    public string PaymentMethod { get; private set; } = "BankTransfer";
    public string? Reference { get; private set; }
    public APPaymentStatus Status { get; private set; } = APPaymentStatus.Draft;
    public Guid? JournalEntryId { get; private set; }

    public Vendor? Vendor { get; private set; }
    public APInvoice? APInvoice { get; private set; }

    private APPayment() { }

    public APPayment(Guid organizationId, string paymentNumber, Guid vendorId, Guid apInvoiceId,
        DateTime paymentDate, decimal amount, string paymentMethod, string? reference = null)
    {
        OrganizationId = organizationId;
        PaymentNumber = paymentNumber;
        VendorId = vendorId;
        APInvoiceId = apInvoiceId;
        PaymentDate = paymentDate;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Reference = reference;
    }

    public void Post(Guid? journalEntryId = null)
    {
        if (Status != APPaymentStatus.Draft)
            throw new InvalidOperationException("Only a draft payment can be posted.");
        Status = APPaymentStatus.Posted;
        JournalEntryId = journalEntryId;
        SetUpdated();
    }

    public void Void()
    {
        if (Status != APPaymentStatus.Posted)
            throw new InvalidOperationException("Only a posted payment can be voided.");
        Status = APPaymentStatus.Voided;
        SetUpdated();
    }
}
