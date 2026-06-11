using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Retail;

public enum RetailStatementStatus { Open, Posted, Failed }
public enum RetailSettlementStatus { Pending, Settled, NotRequired }

public class RetailStatement : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid StoreId { get; private set; }
    public string StatementNumber { get; private set; } = string.Empty;
    public DateTime BusinessDate { get; private set; }
    public string Currency { get; private set; } = "USD";
    public RetailStatementStatus Status { get; private set; } = RetailStatementStatus.Open;
    public int TransactionCount { get; private set; }
    public decimal NetSales { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal TaxTotal { get; private set; }
    public decimal GrandTotal { get; private set; }
    public decimal CostTotal { get; private set; }
    public Guid? ARInvoiceId { get; private set; }
    public Guid? ARCreditNoteId { get; private set; }
    public Guid? JournalEntryId { get; private set; }
    public DateTime? PostedAt { get; private set; }
    public string? PostingError { get; private set; }

    private RetailStatement() { }

    public RetailStatement(Guid organizationId, Guid storeId, string statementNumber,
        DateTime businessDate, string currency)
    {
        OrganizationId = organizationId;
        StoreId = storeId;
        StatementNumber = statementNumber.Trim();
        BusinessDate = businessDate.Date;
        Currency = currency.Trim().ToUpperInvariant();
    }

    public void AddTransaction(decimal netSales, decimal discount, decimal tax,
        decimal grandTotal, decimal cost)
    {
        if (Status != RetailStatementStatus.Open)
            throw new InvalidOperationException("Transactions can only be added to an open retail statement.");

        TransactionCount++;
        NetSales += netSales;
        DiscountTotal += discount;
        TaxTotal += tax;
        GrandTotal += grandTotal;
        CostTotal += cost;
        SetUpdated();
    }

    public void MarkPosted(Guid? arInvoiceId, Guid? arCreditNoteId, Guid journalEntryId)
    {
        Status = RetailStatementStatus.Posted;
        ARInvoiceId = arInvoiceId;
        ARCreditNoteId = arCreditNoteId;
        JournalEntryId = journalEntryId;
        PostedAt = DateTime.UtcNow;
        PostingError = null;
        SetUpdated();
    }

    public void MarkFailed(string error)
    {
        Status = RetailStatementStatus.Failed;
        PostingError = error;
        SetUpdated();
    }
}

public class RetailTenderSettlement : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid RetailStatementId { get; private set; }
    public POSPaymentMethod PaymentMethod { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";
    public RetailSettlementStatus Status { get; private set; }
    public string? ProcessorReference { get; private set; }
    public Guid? BankTransactionId { get; private set; }
    public DateTime? SettledAt { get; private set; }

    private RetailTenderSettlement() { }

    public RetailTenderSettlement(Guid organizationId, Guid statementId,
        POSPaymentMethod paymentMethod, decimal amount, string currency,
        string? processorReference = null)
    {
        OrganizationId = organizationId;
        RetailStatementId = statementId;
        PaymentMethod = paymentMethod;
        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
        ProcessorReference = processorReference?.Trim();
        Status = paymentMethod is POSPaymentMethod.CreditCard or POSPaymentMethod.DebitCard
            ? RetailSettlementStatus.Pending
            : RetailSettlementStatus.NotRequired;
    }

    public void Settle(Guid bankTransactionId)
    {
        if (Status != RetailSettlementStatus.Pending)
            throw new InvalidOperationException("Only pending card settlements can be settled.");
        Status = RetailSettlementStatus.Settled;
        BankTransactionId = bankTransactionId;
        SettledAt = DateTime.UtcNow;
        SetUpdated();
    }
}
