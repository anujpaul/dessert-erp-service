using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum PaymentMethod { Cash, BankTransfer, CreditCard, Check, Other }
public enum ARPaymentStatus { Draft, Posted, Voided }

public class ARPayment : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string PaymentNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Guid ARInvoiceId { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public string? Reference { get; private set; }
    public ARPaymentStatus Status { get; private set; } = ARPaymentStatus.Draft;
    public Guid? JournalEntryId { get; private set; }

    public Customer? Customer { get; private set; }
    public ARInvoice? ARInvoice { get; private set; }

    private ARPayment() { }

    public ARPayment(Guid organizationId, string paymentNumber, Guid customerId, Guid arInvoiceId,
        DateTime paymentDate, decimal amount, PaymentMethod paymentMethod, string? reference = null)
    {
        OrganizationId = organizationId;
        PaymentNumber = paymentNumber;
        CustomerId = customerId;
        ARInvoiceId = arInvoiceId;
        PaymentDate = paymentDate;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Reference = reference;
    }

    public void Post(Guid? journalEntryId = null)
    {
        if (Status != ARPaymentStatus.Draft)
            throw new InvalidOperationException("Only a draft payment can be posted.");
        Status = ARPaymentStatus.Posted;
        JournalEntryId = journalEntryId;
        SetUpdated();
    }

    public void Void()
    {
        if (Status != ARPaymentStatus.Posted)
            throw new InvalidOperationException("Only a posted payment can be voided.");
        Status = ARPaymentStatus.Voided;
        SetUpdated();
    }
}
