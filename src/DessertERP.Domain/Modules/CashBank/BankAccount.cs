using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.CashBank;

public enum BankAccountType  { Checking, Savings, CreditCard, PettyCash, Other }
public enum BankAccountStatus { Active, Inactive, Frozen, Closed }

/// <summary>
/// Represents a company bank or cash account used for receipts and payments.
/// One account per currency is the standard D365 pattern — enforced at application layer.
/// </summary>
public class BankAccount : BaseEntity
{
    public Guid   OrganizationId { get; private set; }

    /// <summary>Internal reference code, e.g. "BANK-USD-01".</summary>
    public string AccountCode    { get; private set; } = string.Empty;

    /// <summary>Display name, e.g. "Chase Checking — USD".</summary>
    public string AccountName    { get; private set; } = string.Empty;

    public BankAccountType   AccountType   { get; private set; } = BankAccountType.Checking;
    public BankAccountStatus AccountStatus { get; private set; } = BankAccountStatus.Active;

    // Bank details
    public string? BankName      { get; private set; }
    public string? BankBranch    { get; private set; }
    public string? RoutingNumber { get; private set; }
    public string? AccountNumber { get; private set; }  // masked at API layer
    public string? IBAN          { get; private set; }
    public string? SwiftCode     { get; private set; }

    /// <summary>ISO 4217 currency code this account operates in.</summary>
    public string Currency       { get; private set; } = "USD";

    /// <summary>Linked GL account for automatic journal posting.</summary>
    public Guid?  GLAccountId    { get; private set; }

    /// <summary>Running ledger balance (updated on every posted transaction).</summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>Balance at last completed reconciliation.</summary>
    public decimal LastReconciledBalance { get; private set; }
    public DateTime? LastReconciledAt    { get; private set; }

    public string? Notes { get; private set; }

    private BankAccount() { }

    public BankAccount(Guid organizationId, string accountCode, string accountName,
        BankAccountType accountType, string currency,
        Guid? glAccountId = null,
        string? bankName = null, string? bankBranch = null,
        string? routingNumber = null, string? accountNumber = null,
        string? iban = null, string? swiftCode = null,
        string? notes = null)
    {
        OrganizationId = organizationId;
        AccountCode    = accountCode.Trim().ToUpperInvariant();
        AccountName    = accountName.Trim();
        AccountType    = accountType;
        Currency       = currency.ToUpperInvariant();
        GLAccountId    = glAccountId;
        BankName       = bankName;
        BankBranch     = bankBranch;
        RoutingNumber  = routingNumber;
        AccountNumber  = accountNumber;
        IBAN           = iban;
        SwiftCode      = swiftCode;
        Notes          = notes;
    }

    public void UpdateDetails(string accountName, BankAccountType accountType,
        Guid? glAccountId, string? bankName, string? bankBranch,
        string? routingNumber, string? accountNumber,
        string? iban, string? swiftCode, string? notes)
    {
        AccountName   = accountName.Trim();
        AccountType   = accountType;
        GLAccountId   = glAccountId;
        BankName      = bankName;
        BankBranch    = bankBranch;
        RoutingNumber = routingNumber;
        AccountNumber = accountNumber;
        IBAN          = iban;
        SwiftCode     = swiftCode;
        Notes         = notes;
        SetUpdated();
    }

    public void Activate()   { AccountStatus = BankAccountStatus.Active;   SetUpdated(); }
    public void Deactivate() { AccountStatus = BankAccountStatus.Inactive; SetUpdated(); }
    public void Freeze()     { AccountStatus = BankAccountStatus.Frozen;   SetUpdated(); }

    public void Close()
    {
        if (CurrentBalance != 0)
            throw new InvalidOperationException("Cannot close a bank account with a non-zero balance.");
        AccountStatus = BankAccountStatus.Closed;
        SetUpdated();
    }

    /// <summary>Called by the application layer after posting a transaction.</summary>
    public void AdjustBalance(decimal amount)
    {
        CurrentBalance += amount;
        SetUpdated();
    }

    /// <summary>Called when a reconciliation is completed.</summary>
    public void MarkReconciled(decimal reconciledBalance)
    {
        LastReconciledBalance = reconciledBalance;
        LastReconciledAt      = DateTime.UtcNow;
        SetUpdated();
    }
}
