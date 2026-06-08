using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum PaymentProposalStatus { Draft, Approved, Processed, Cancelled }

/// <summary>
/// Payment Proposal (Payment Run) — groups outstanding AP invoices for batch payment.
/// Lifecycle: Draft → Approved → Processed (generates APPayment records).
/// </summary>
public class PaymentProposal : BaseEntity
{
    public Guid   OrganizationId  { get; private set; }
    public string ProposalNumber  { get; private set; } = string.Empty;
    public DateTime ProposalDate  { get; private set; }
    public DateTime PaymentDate   { get; private set; }
    public string   PaymentMethod { get; private set; } = "BankTransfer";
    public string?  BankAccount   { get; private set; }
    public string?  Notes         { get; private set; }
    public PaymentProposalStatus Status { get; private set; } = PaymentProposalStatus.Draft;
    public DateTime? ProcessedAt  { get; private set; }
    public string?   ProcessedBy  { get; private set; }

    public decimal TotalAmount => Lines.Sum(l => l.ProposedAmount);

    private readonly List<PaymentProposalLine> _lines = new();
    public IReadOnlyCollection<PaymentProposalLine> Lines => _lines.AsReadOnly();

    private PaymentProposal() { }

    public PaymentProposal(Guid organizationId, string proposalNumber,
        DateTime proposalDate, DateTime paymentDate,
        string paymentMethod = "BankTransfer", string? bankAccount = null, string? notes = null)
    {
        OrganizationId = organizationId;
        ProposalNumber = proposalNumber.Trim();
        ProposalDate   = proposalDate;
        PaymentDate    = paymentDate;
        PaymentMethod  = paymentMethod;
        BankAccount    = bankAccount;
        Notes          = notes;
    }

    public PaymentProposalLine AddLine(Guid invoiceId, string invoiceNumber,
        Guid vendorId, string vendorName, decimal proposedAmount, DateTime invoiceDueDate)
    {
        if (Status != PaymentProposalStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to a Draft proposal.");
        if (_lines.Any(l => l.APInvoiceId == invoiceId))
            throw new InvalidOperationException("Invoice already included in this proposal.");

        var line = new PaymentProposalLine(
            Id, invoiceId, invoiceNumber, vendorId, vendorName, proposedAmount, invoiceDueDate);
        _lines.Add(line);
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != PaymentProposalStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft proposal.");
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        SetUpdated();
    }

    public void Approve()
    {
        if (Status != PaymentProposalStatus.Draft)
            throw new InvalidOperationException("Only Draft proposals can be approved.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot approve a proposal with no lines.");
        Status = PaymentProposalStatus.Approved;
        SetUpdated();
    }

    /// <summary>
    /// Marks the proposal as processed. The service layer is responsible for
    /// creating APPayment records for each line before calling this.
    /// </summary>
    public void MarkProcessed(string processedBy)
    {
        if (Status != PaymentProposalStatus.Approved)
            throw new InvalidOperationException("Only Approved proposals can be processed.");
        Status      = PaymentProposalStatus.Processed;
        ProcessedAt = DateTime.UtcNow;
        ProcessedBy = processedBy;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == PaymentProposalStatus.Processed)
            throw new InvalidOperationException("Processed proposals cannot be cancelled.");
        Status = PaymentProposalStatus.Cancelled;
        SetUpdated();
    }
}

public class PaymentProposalLine : BaseEntity
{
    public Guid    ProposalId     { get; private set; }
    public Guid    APInvoiceId    { get; private set; }
    public string  InvoiceNumber  { get; private set; } = string.Empty;
    public Guid    VendorId       { get; private set; }
    public string  VendorName     { get; private set; } = string.Empty;
    public decimal ProposedAmount { get; private set; }
    public DateTime InvoiceDueDate { get; private set; }

    /// <summary>Set after the payment run creates the APPayment record.</summary>
    public Guid? APPaymentId      { get; private set; }

    public PaymentProposal? Proposal  { get; private set; }
    public APInvoice?       APInvoice { get; private set; }
    public APPayment?       APPayment { get; private set; }

    private PaymentProposalLine() { }

    public PaymentProposalLine(Guid proposalId, Guid apInvoiceId, string invoiceNumber,
        Guid vendorId, string vendorName, decimal proposedAmount, DateTime invoiceDueDate)
    {
        ProposalId     = proposalId;
        APInvoiceId    = apInvoiceId;
        InvoiceNumber  = invoiceNumber;
        VendorId       = vendorId;
        VendorName     = vendorName;
        ProposedAmount = proposedAmount;
        InvoiceDueDate = invoiceDueDate;
    }

    public void SetPayment(Guid apPaymentId)
    {
        APPaymentId = apPaymentId;
        SetUpdated();
    }
}
