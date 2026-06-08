using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.CashBank;

public enum ReconciliationStatus { InProgress, Completed, Cancelled }

/// <summary>
/// Bank Reconciliation — matches system transactions against the bank statement.
/// Completed when system balance + unreconciled items equals the statement closing balance.
/// </summary>
public class BankReconciliation : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public Guid   BankAccountId  { get; private set; }

    public string ReconciliationNumber { get; private set; } = string.Empty;

    /// <summary>The statement period start date.</summary>
    public DateTime StatementStartDate { get; private set; }

    /// <summary>The statement period end date.</summary>
    public DateTime StatementEndDate   { get; private set; }

    /// <summary>Opening balance as shown on the bank statement.</summary>
    public decimal StatementOpeningBalance { get; private set; }

    /// <summary>Closing balance as shown on the bank statement.</summary>
    public decimal StatementClosingBalance { get; private set; }

    /// <summary>System balance at start of period.</summary>
    public decimal SystemOpeningBalance    { get; private set; }

    /// <summary>Sum of all reconciled transactions in this session.</summary>
    public decimal ReconciledAmount        { get; private set; }

    /// <summary>Difference = StatementClosingBalance - (SystemOpeningBalance + ReconciledAmount).
    /// Must equal 0 to complete.</summary>
    public decimal Difference => StatementClosingBalance - (SystemOpeningBalance + ReconciledAmount);

    public ReconciliationStatus Status { get; private set; } = ReconciliationStatus.InProgress;

    public string? Notes       { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string?   CompletedBy { get; private set; }

    public BankAccount? BankAccount { get; private set; }

    private BankReconciliation() { }

    public BankReconciliation(Guid organizationId, Guid bankAccountId,
        string reconciliationNumber,
        DateTime statementStartDate, DateTime statementEndDate,
        decimal statementOpeningBalance, decimal statementClosingBalance,
        decimal systemOpeningBalance, string? notes = null)
    {
        OrganizationId          = organizationId;
        BankAccountId           = bankAccountId;
        ReconciliationNumber    = reconciliationNumber;
        StatementStartDate      = statementStartDate;
        StatementEndDate        = statementEndDate;
        StatementOpeningBalance = statementOpeningBalance;
        StatementClosingBalance = statementClosingBalance;
        SystemOpeningBalance    = systemOpeningBalance;
        Notes                   = notes;
    }

    /// <summary>Called each time a transaction is marked reconciled in this session.</summary>
    public void AddReconciledAmount(decimal amount)
    {
        if (Status != ReconciliationStatus.InProgress)
            throw new InvalidOperationException("Reconciliation is not in progress.");
        ReconciledAmount += amount;
        SetUpdated();
    }

    /// <summary>Remove a previously reconciled transaction from this session.</summary>
    public void RemoveReconciledAmount(decimal amount)
    {
        if (Status != ReconciliationStatus.InProgress)
            throw new InvalidOperationException("Reconciliation is not in progress.");
        ReconciledAmount -= amount;
        SetUpdated();
    }

    public void Complete(string completedBy)
    {
        if (Status != ReconciliationStatus.InProgress)
            throw new InvalidOperationException("Reconciliation is not in progress.");
        if (Math.Abs(Difference) > 0.01m)
            throw new InvalidOperationException(
                $"Cannot complete — reconciliation has a difference of {Difference:C}. " +
                "All transactions must be matched before completing.");
        Status      = ReconciliationStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        CompletedBy = completedBy;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == ReconciliationStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed reconciliation.");
        Status = ReconciliationStatus.Cancelled;
        SetUpdated();
    }
}
