using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.CashBank;

public enum BankTransactionType
{
    Deposit       = 1,   // money in
    Withdrawal    = 2,   // money out
    Transfer      = 3,   // between accounts
    BankFee       = 4,
    Interest      = 5,
    ARReceipt     = 6,   // customer payment applied
    APPayment     = 7,   // vendor payment made
    PayrollRun    = 8,
    Other         = 9
}

public enum BankTransactionStatus
{
    Draft     = 1,   // being entered
    Posted    = 2,   // confirmed, balance updated
    Voided    = 3,
    Reconciled = 4   // matched to bank statement
}

/// <summary>
/// A single debit or credit movement against a BankAccount.
/// Posting the transaction adjusts the BankAccount.CurrentBalance.
/// </summary>
public class BankTransaction : BaseEntity
{
    public Guid   OrganizationId   { get; private set; }
    public Guid   BankAccountId    { get; private set; }

    public string TransactionNumber { get; private set; } = string.Empty;
    public DateTime TransactionDate { get; private set; }
    public BankTransactionType   TransactionType   { get; private set; }
    public BankTransactionStatus TransactionStatus { get; private set; } = BankTransactionStatus.Draft;

    /// <summary>Positive = credit (deposit); Negative = debit (withdrawal).</summary>
    public decimal Amount          { get; private set; }

    public string  Description     { get; private set; } = string.Empty;
    public string? Reference       { get; private set; }  // cheque#, wire ref, etc.
    public string? CounterpartyName { get; private set; }  // payee / payer name

    // Optional linkage to source document
    public Guid?  ARInvoiceId      { get; private set; }
    public Guid?  APInvoiceId      { get; private set; }
    public Guid?  TransferToAccountId { get; private set; }  // for internal transfers

    // Reconciliation
    public Guid?  ReconciliationId { get; private set; }
    public DateTime? ReconciledAt  { get; private set; }

    public DateTime? PostedAt      { get; private set; }
    public string?   PostedBy      { get; private set; }

    public string? Notes { get; private set; }

    public BankAccount? BankAccount { get; private set; }

    private BankTransaction() { }

    public BankTransaction(Guid organizationId, Guid bankAccountId,
        string transactionNumber, DateTime transactionDate,
        BankTransactionType transactionType, decimal amount,
        string description, string? reference = null,
        string? counterpartyName = null,
        Guid? arInvoiceId = null, Guid? apInvoiceId = null,
        Guid? transferToAccountId = null, string? notes = null)
    {
        if (amount == 0)
            throw new ArgumentException("Transaction amount cannot be zero.");

        OrganizationId        = organizationId;
        BankAccountId         = bankAccountId;
        TransactionNumber     = transactionNumber;
        TransactionDate       = transactionDate;
        TransactionType       = transactionType;
        Amount                = amount;
        Description           = description.Trim();
        Reference             = reference;
        CounterpartyName      = counterpartyName;
        ARInvoiceId           = arInvoiceId;
        APInvoiceId           = apInvoiceId;
        TransferToAccountId   = transferToAccountId;
        Notes                 = notes;
    }

    public void Post(string postedBy)
    {
        if (TransactionStatus != BankTransactionStatus.Draft)
            throw new InvalidOperationException("Only Draft transactions can be posted.");
        TransactionStatus = BankTransactionStatus.Posted;
        PostedAt          = DateTime.UtcNow;
        PostedBy          = postedBy;
        SetUpdated();
    }

    public void Void()
    {
        if (TransactionStatus == BankTransactionStatus.Reconciled)
            throw new InvalidOperationException("Cannot void a reconciled transaction.");
        if (TransactionStatus == BankTransactionStatus.Voided)
            throw new InvalidOperationException("Already voided.");
        TransactionStatus = BankTransactionStatus.Voided;
        SetUpdated();
    }

    public void MarkReconciled(Guid reconciliationId)
    {
        if (TransactionStatus != BankTransactionStatus.Posted)
            throw new InvalidOperationException("Only Posted transactions can be reconciled.");
        TransactionStatus  = BankTransactionStatus.Reconciled;
        ReconciliationId   = reconciliationId;
        ReconciledAt       = DateTime.UtcNow;
        SetUpdated();
    }

    public void UpdateDraft(DateTime transactionDate, decimal amount,
        string description, string? reference, string? counterpartyName, string? notes)
    {
        if (TransactionStatus != BankTransactionStatus.Draft)
            throw new InvalidOperationException("Only Draft transactions can be edited.");
        if (amount == 0)
            throw new ArgumentException("Amount cannot be zero.");

        TransactionDate  = transactionDate;
        Amount           = amount;
        Description      = description.Trim();
        Reference        = reference;
        CounterpartyName = counterpartyName;
        Notes            = notes;
        SetUpdated();
    }
}
