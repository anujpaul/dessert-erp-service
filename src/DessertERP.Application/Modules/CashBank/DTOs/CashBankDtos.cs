namespace DessertERP.Application.Modules.CashBank.DTOs;

// ── Bank Account ─────────────────────────────────────────────────────────────

public record BankAccountDto(
    Guid   Id,
    string AccountCode,
    string AccountName,
    string AccountType,
    string AccountStatus,
    string Currency,
    Guid?  GLAccountId,
    string? BankName,
    string? BankBranch,
    string? RoutingNumber,
    string? AccountNumberMasked,   // last-4 only
    string? IBAN,
    string? SwiftCode,
    decimal CurrentBalance,
    decimal LastReconciledBalance,
    DateTime? LastReconciledAt,
    string? Notes,
    DateTime CreatedAt);

public record BankAccountSummaryDto(
    Guid Id, string AccountCode, string AccountName,
    string AccountType, string AccountStatus,
    string Currency, decimal CurrentBalance,
    string? BankName, DateTime CreatedAt);

public record CreateBankAccountRequest(
    string AccountCode,
    string AccountName,
    string AccountType,         // "Checking" | "Savings" | "CreditCard" | "PettyCash" | "Other"
    string Currency,
    Guid?  GLAccountId   = null,
    string? BankName     = null,
    string? BankBranch   = null,
    string? RoutingNumber= null,
    string? AccountNumber= null,
    string? IBAN         = null,
    string? SwiftCode    = null,
    string? Notes        = null);

public record UpdateBankAccountRequest(
    string AccountName,
    string AccountType,
    Guid?  GLAccountId,
    string? BankName,
    string? BankBranch,
    string? RoutingNumber,
    string? AccountNumber,
    string? IBAN,
    string? SwiftCode,
    string? Notes);

// ── Bank Transaction ─────────────────────────────────────────────────────────

public record BankTransactionDto(
    Guid   Id,
    Guid   BankAccountId,
    string AccountName,
    string TransactionNumber,
    DateTime TransactionDate,
    string TransactionType,
    string TransactionStatus,
    decimal Amount,
    string Description,
    string? Reference,
    string? CounterpartyName,
    Guid?  ARInvoiceId,
    Guid?  APInvoiceId,
    Guid?  TransferToAccountId,
    Guid?  ReconciliationId,
    DateTime? ReconciledAt,
    DateTime? PostedAt,
    string? PostedBy,
    string? Notes,
    DateTime CreatedAt);

public record BankTransactionSummaryDto(
    Guid Id, string TransactionNumber, DateTime TransactionDate,
    string TransactionType, string TransactionStatus,
    decimal Amount, string Description,
    string? Reference, string? CounterpartyName, DateTime CreatedAt);

public record CreateBankTransactionRequest(
    Guid   BankAccountId,
    DateTime TransactionDate,
    string TransactionType,     // "Deposit" | "Withdrawal" | "Transfer" | etc.
    decimal Amount,
    string Description,
    string? Reference        = null,
    string? CounterpartyName = null,
    Guid?  ARInvoiceId       = null,
    Guid?  APInvoiceId       = null,
    Guid?  TransferToAccountId = null,
    string? Notes            = null);

public record PostTransactionRequest(string PostedBy);

// ── Bank Reconciliation ───────────────────────────────────────────────────────

public record BankReconciliationDto(
    Guid   Id,
    Guid   BankAccountId,
    string AccountName,
    string ReconciliationNumber,
    DateTime StatementStartDate,
    DateTime StatementEndDate,
    decimal StatementOpeningBalance,
    decimal StatementClosingBalance,
    decimal SystemOpeningBalance,
    decimal ReconciledAmount,
    decimal Difference,
    string Status,
    string? Notes,
    DateTime? CompletedAt,
    string? CompletedBy,
    DateTime CreatedAt,
    List<BankTransactionSummaryDto> ReconciledTransactions);

public record BankReconciliationSummaryDto(
    Guid Id, string ReconciliationNumber,
    Guid BankAccountId, string AccountName,
    DateTime StatementStartDate, DateTime StatementEndDate,
    decimal StatementClosingBalance, decimal Difference,
    string Status, DateTime? CompletedAt, DateTime CreatedAt);

public record CreateReconciliationRequest(
    Guid   BankAccountId,
    DateTime StatementStartDate,
    DateTime StatementEndDate,
    decimal StatementOpeningBalance,
    decimal StatementClosingBalance,
    string? Notes = null);

public record ReconcileTransactionRequest(Guid TransactionId, bool IsReconciled);

public record CompleteReconciliationRequest(string CompletedBy);

// ── Cash Journal ──────────────────────────────────────────────────────────────

public record CashJournalDto(
    Guid   Id,
    Guid   BankAccountId,
    string AccountName,
    string JournalNumber,
    DateTime JournalDate,
    string Description,
    string Status,
    decimal TotalDebits,
    decimal TotalCredits,
    DateTime? PostedAt,
    string? PostedBy,
    string? Notes,
    DateTime CreatedAt,
    List<CashJournalLineDto> Lines);

public record CashJournalSummaryDto(
    Guid Id, string JournalNumber, DateTime JournalDate,
    string Description, string Status,
    decimal TotalDebits, decimal TotalCredits,
    DateTime? PostedAt, DateTime CreatedAt);

public record CashJournalLineDto(
    Guid   Id,
    Guid   GLAccountId,
    string Description,
    decimal Debit,
    decimal Credit,
    string? Reference);

public record CreateCashJournalRequest(
    Guid   BankAccountId,
    DateTime JournalDate,
    string Description,
    string? Notes = null,
    List<CreateCashJournalLineRequest>? Lines = null);

public record CreateCashJournalLineRequest(
    Guid   GLAccountId,
    string Description,
    decimal Debit,
    decimal Credit,
    string? Reference = null);

public record PostCashJournalRequest(string PostedBy);
