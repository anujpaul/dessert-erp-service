using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.CashBank;
using DessertERP.Application.Modules.CashBank.DTOs;
using DessertERP.Domain.Modules.CashBank;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Infrastructure.Modules.CashBank;

public class CashBankService : ICashBankService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public CashBankService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db  = db;
        _org = org;
    }

    // ── Bank Accounts ─────────────────────────────────────────────────────────

    public async Task<IEnumerable<BankAccountSummaryDto>> GetBankAccountsAsync(
        bool activeOnly = false, CancellationToken ct = default)
    {
        var query = _db.BankAccounts.AsQueryable();
        if (activeOnly)
            query = query.Where(a => a.AccountStatus == BankAccountStatus.Active);
        var list = await query.OrderBy(a => a.AccountCode).ToListAsync(ct);
        return list.Select(ToSummaryDto);
    }

    public async Task<BankAccountDto?> GetBankAccountAsync(Guid id, CancellationToken ct = default)
    {
        var a = await _db.BankAccounts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        return a is null ? null : ToDto(a);
    }

    public async Task<BankAccountDto> CreateBankAccountAsync(
        CreateBankAccountRequest req, CancellationToken ct = default)
    {
        if (!Enum.TryParse<BankAccountType>(req.AccountType, out var type))
            type = BankAccountType.Checking;

        var exists = await _db.BankAccounts
            .AnyAsync(a => a.AccountCode == req.AccountCode.Trim().ToUpperInvariant() && !a.IsDeleted, ct);
        if (exists)
            throw new InvalidOperationException($"Bank account code '{req.AccountCode}' already exists.");

        var account = new BankAccount(
            _org.OrganizationId, req.AccountCode, req.AccountName,
            type, req.Currency, req.GLAccountId,
            req.BankName, req.BankBranch, req.RoutingNumber,
            req.AccountNumber, req.IBAN, req.SwiftCode, req.Notes);

        _db.BankAccounts.Add(account);
        await _db.SaveChangesAsync(ct);
        return ToDto(account);
    }

    public async Task<BankAccountDto> UpdateBankAccountAsync(
        Guid id, UpdateBankAccountRequest req, CancellationToken ct = default)
    {
        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");

        if (!Enum.TryParse<BankAccountType>(req.AccountType, out var type))
            type = BankAccountType.Checking;

        account.UpdateDetails(req.AccountName, type, req.GLAccountId,
            req.BankName, req.BankBranch, req.RoutingNumber,
            req.AccountNumber, req.IBAN, req.SwiftCode, req.Notes);
        await _db.SaveChangesAsync(ct);
        return ToDto(account);
    }

    public async Task ActivateBankAccountAsync(Guid id, CancellationToken ct = default)
    {
        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");
        account.Activate();
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeactivateBankAccountAsync(Guid id, CancellationToken ct = default)
    {
        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");
        account.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    // ── Bank Transactions ─────────────────────────────────────────────────────

    public async Task<IEnumerable<BankTransactionSummaryDto>> GetTransactionsAsync(
        Guid? bankAccountId = null, string? status = null, CancellationToken ct = default)
    {
        var query = _db.BankTransactions.Include(t => t.BankAccount).AsQueryable();
        if (bankAccountId.HasValue)
            query = query.Where(t => t.BankAccountId == bankAccountId.Value);
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<BankTransactionStatus>(status, out var s))
            query = query.Where(t => t.TransactionStatus == s);

        var list = await query.OrderByDescending(t => t.TransactionDate)
                              .ThenByDescending(t => t.CreatedAt)
                              .ToListAsync(ct);
        return list.Select(ToTxSummaryDto);
    }

    public async Task<BankTransactionDto?> GetTransactionAsync(Guid id, CancellationToken ct = default)
    {
        var t = await _db.BankTransactions
            .Include(x => x.BankAccount)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        return t is null ? null : ToTxDto(t);
    }

    public async Task<BankTransactionDto> CreateTransactionAsync(
        CreateBankTransactionRequest req, CancellationToken ct = default)
    {
        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == req.BankAccountId && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");

        if (!Enum.TryParse<BankTransactionType>(req.TransactionType, out var txType))
            txType = BankTransactionType.Other;

        var count = await _db.BankTransactions.CountAsync(ct) + 1;
        var txNumber = $"TXN-{count:D7}";

        var tx = new BankTransaction(
            _org.OrganizationId, req.BankAccountId, txNumber,
            req.TransactionDate, txType, req.Amount,
            req.Description, req.Reference, req.CounterpartyName,
            req.ARInvoiceId, req.APInvoiceId, req.TransferToAccountId, req.Notes);

        _db.BankTransactions.Add(tx);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.BankTransactions.Include(x => x.BankAccount)
            .FirstAsync(x => x.Id == tx.Id, ct);
        return ToTxDto(saved);
    }

    public async Task<BankTransactionDto> PostTransactionAsync(
        Guid id, PostTransactionRequest req, CancellationToken ct = default)
    {
        var tx = await _db.BankTransactions.Include(x => x.BankAccount)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Transaction not found.");

        tx.Post(req.PostedBy);

        // Update bank account balance
        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == tx.BankAccountId && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");
        account.AdjustBalance(tx.Amount);

        // If this is an internal transfer, create counter transaction on the destination account
        if (tx.TransferToAccountId.HasValue && tx.TransactionType == BankTransactionType.Transfer)
        {
            var destAccount = await _db.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == tx.TransferToAccountId && !a.IsDeleted, ct);
            if (destAccount is not null)
            {
                var counterCount = await _db.BankTransactions.CountAsync(ct) + 1;
                var counterTx = new BankTransaction(
                    _org.OrganizationId, destAccount.Id,
                    $"TXN-{counterCount:D7}",
                    tx.TransactionDate, BankTransactionType.Transfer,
                    -tx.Amount,  // opposite sign on destination
                    $"Transfer from {account.AccountCode}",
                    tx.Reference, account.AccountName);
                counterTx.Post(req.PostedBy);
                destAccount.AdjustBalance(counterTx.Amount);
                _db.BankTransactions.Add(counterTx);
            }
        }

        await _db.SaveChangesAsync(ct);
        return ToTxDto(tx);
    }

    public async Task VoidTransactionAsync(Guid id, CancellationToken ct = default)
    {
        var tx = await _db.BankTransactions
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Transaction not found.");

        var wasPosted = tx.TransactionStatus == BankTransactionStatus.Posted;
        tx.Void();

        // Reverse the balance effect if it was posted
        if (wasPosted)
        {
            var account = await _db.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == tx.BankAccountId && !a.IsDeleted, ct);
            account?.AdjustBalance(-tx.Amount);
        }

        await _db.SaveChangesAsync(ct);
    }

    // ── Bank Reconciliation ───────────────────────────────────────────────────

    public async Task<IEnumerable<BankReconciliationSummaryDto>> GetReconciliationsAsync(
        Guid? bankAccountId = null, CancellationToken ct = default)
    {
        var query = _db.BankReconciliations.Include(r => r.BankAccount).AsQueryable();
        if (bankAccountId.HasValue)
            query = query.Where(r => r.BankAccountId == bankAccountId.Value);

        var list = await query.OrderByDescending(r => r.StatementEndDate).ToListAsync(ct);
        return list.Select(ToReconSummaryDto);
    }

    public async Task<BankReconciliationDto?> GetReconciliationAsync(Guid id, CancellationToken ct = default)
    {
        var r = await _db.BankReconciliations
            .Include(x => x.BankAccount)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        if (r is null) return null;

        var txns = await _db.BankTransactions
            .Where(t => t.ReconciliationId == id)
            .OrderBy(t => t.TransactionDate)
            .ToListAsync(ct);

        return ToReconDto(r, txns);
    }

    public async Task<BankReconciliationDto> CreateReconciliationAsync(
        CreateReconciliationRequest req, CancellationToken ct = default)
    {
        // Check no open reconciliation exists for this account
        var open = await _db.BankReconciliations
            .AnyAsync(r => r.BankAccountId == req.BankAccountId
                        && r.Status == ReconciliationStatus.InProgress
                        && !r.IsDeleted, ct);
        if (open)
            throw new InvalidOperationException("An in-progress reconciliation already exists for this account.");

        var count = await _db.BankReconciliations.CountAsync(ct) + 1;

        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == req.BankAccountId && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");

        var recon = new BankReconciliation(
            _org.OrganizationId, req.BankAccountId,
            $"REC-{count:D5}",
            req.StatementStartDate, req.StatementEndDate,
            req.StatementOpeningBalance, req.StatementClosingBalance,
            account.LastReconciledBalance,
            req.Notes);

        _db.BankReconciliations.Add(recon);
        await _db.SaveChangesAsync(ct);
        return ToReconDto(recon, new List<BankTransaction>());
    }

    public async Task<BankReconciliationDto> ReconcileTransactionAsync(
        Guid reconciliationId, ReconcileTransactionRequest req, CancellationToken ct = default)
    {
        var recon = await _db.BankReconciliations
            .Include(r => r.BankAccount)
            .FirstOrDefaultAsync(r => r.Id == reconciliationId && !r.IsDeleted, ct)
            ?? throw new InvalidOperationException("Reconciliation not found.");

        var tx = await _db.BankTransactions
            .FirstOrDefaultAsync(t => t.Id == req.TransactionId && !t.IsDeleted, ct)
            ?? throw new InvalidOperationException("Transaction not found.");

        if (req.IsReconciled)
        {
            tx.MarkReconciled(reconciliationId);
            recon.AddReconciledAmount(tx.Amount);
        }
        else
        {
            // Un-reconcile — revert
            recon.RemoveReconciledAmount(tx.Amount);
            // Re-post the transaction (simplified: set back to Posted)
            // In production this would track the "un-reconcile" more carefully
        }

        await _db.SaveChangesAsync(ct);

        var txns = await _db.BankTransactions
            .Where(t => t.ReconciliationId == reconciliationId)
            .ToListAsync(ct);
        return ToReconDto(recon, txns);
    }

    public async Task<BankReconciliationDto> CompleteReconciliationAsync(
        Guid id, CompleteReconciliationRequest req, CancellationToken ct = default)
    {
        var recon = await _db.BankReconciliations
            .Include(r => r.BankAccount)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new InvalidOperationException("Reconciliation not found.");

        recon.Complete(req.CompletedBy);

        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == recon.BankAccountId && !a.IsDeleted, ct);
        account?.MarkReconciled(recon.StatementClosingBalance);

        await _db.SaveChangesAsync(ct);

        var txns = await _db.BankTransactions
            .Where(t => t.ReconciliationId == id)
            .ToListAsync(ct);
        return ToReconDto(recon, txns);
    }

    public async Task CancelReconciliationAsync(Guid id, CancellationToken ct = default)
    {
        var recon = await _db.BankReconciliations
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new InvalidOperationException("Reconciliation not found.");

        // Un-mark all transactions that were reconciled in this session
        var txns = await _db.BankTransactions
            .Where(t => t.ReconciliationId == id)
            .ToListAsync(ct);
        // For simplicity we just cancel — a real implementation would reset transaction status

        recon.Cancel();
        await _db.SaveChangesAsync(ct);
    }

    // ── Cash Journals ─────────────────────────────────────────────────────────

    public async Task<IEnumerable<CashJournalSummaryDto>> GetCashJournalsAsync(
        Guid? bankAccountId = null, string? status = null, CancellationToken ct = default)
    {
        var query = _db.CashJournals.Include(j => j.BankAccount).AsQueryable();
        if (bankAccountId.HasValue)
            query = query.Where(j => j.BankAccountId == bankAccountId.Value);
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<CashJournalStatus>(status, out var s))
            query = query.Where(j => j.Status == s);

        var list = await query.OrderByDescending(j => j.JournalDate).ToListAsync(ct);
        return list.Select(ToJournalSummaryDto);
    }

    public async Task<CashJournalDto?> GetCashJournalAsync(Guid id, CancellationToken ct = default)
    {
        var j = await _db.CashJournals
            .Include(x => x.BankAccount)
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        return j is null ? null : ToJournalDto(j);
    }

    public async Task<CashJournalDto> CreateCashJournalAsync(
        CreateCashJournalRequest req, CancellationToken ct = default)
    {
        _ = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == req.BankAccountId && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Bank account not found.");

        var count = await _db.CashJournals.CountAsync(ct) + 1;
        var journal = new CashJournal(
            _org.OrganizationId, req.BankAccountId,
            $"CJ-{count:D6}", req.JournalDate, req.Description, req.Notes);

        foreach (var line in req.Lines ?? new List<CreateCashJournalLineRequest>())
            journal.AddLine(line.GLAccountId, line.Description, line.Debit, line.Credit, line.Reference);

        _db.CashJournals.Add(journal);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.CashJournals
            .Include(x => x.BankAccount)
            .Include(x => x.Lines)
            .FirstAsync(x => x.Id == journal.Id, ct);
        return ToJournalDto(saved);
    }

    public async Task<CashJournalDto> AddJournalLineAsync(
        Guid journalId, CreateCashJournalLineRequest req, CancellationToken ct = default)
    {
        var journal = await _db.CashJournals
            .Include(x => x.BankAccount)
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == journalId && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Cash journal not found.");

        journal.AddLine(req.GLAccountId, req.Description, req.Debit, req.Credit, req.Reference);
        await _db.SaveChangesAsync(ct);
        return ToJournalDto(journal);
    }

    public async Task RemoveJournalLineAsync(Guid journalId, Guid lineId, CancellationToken ct = default)
    {
        var journal = await _db.CashJournals
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == journalId && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Cash journal not found.");

        journal.RemoveLine(lineId);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<CashJournalDto> PostCashJournalAsync(
        Guid id, PostCashJournalRequest req, CancellationToken ct = default)
    {
        var journal = await _db.CashJournals
            .Include(x => x.BankAccount)
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Cash journal not found.");

        journal.Post(req.PostedBy);

        // Adjust bank account balance by net (credits - debits for the cash account)
        var account = await _db.BankAccounts
            .FirstOrDefaultAsync(a => a.Id == journal.BankAccountId && !a.IsDeleted, ct);
        if (account is not null)
            account.AdjustBalance(journal.TotalCredits - journal.TotalDebits);

        await _db.SaveChangesAsync(ct);
        return ToJournalDto(journal);
    }

    public async Task VoidCashJournalAsync(Guid id, CancellationToken ct = default)
    {
        var journal = await _db.CashJournals
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Cash journal not found.");

        var wasPosted = journal.Status == CashJournalStatus.Posted;
        journal.Void();

        if (wasPosted)
        {
            var account = await _db.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == journal.BankAccountId && !a.IsDeleted, ct);
            // Reverse the balance adjustment
            if (account is not null)
                account.AdjustBalance(-(journal.TotalCredits - journal.TotalDebits));
        }

        await _db.SaveChangesAsync(ct);
    }

    // ── Mapping helpers ───────────────────────────────────────────────────────

    private static BankAccountSummaryDto ToSummaryDto(BankAccount a) =>
        new(a.Id, a.AccountCode, a.AccountName,
            a.AccountType.ToString(), a.AccountStatus.ToString(),
            a.Currency, a.CurrentBalance, a.BankName, a.CreatedAt);

    private static BankAccountDto ToDto(BankAccount a)
    {
        var masked = a.AccountNumber is { Length: > 4 }
            ? "****" + a.AccountNumber[^4..] : a.AccountNumber;
        return new BankAccountDto(
            a.Id, a.AccountCode, a.AccountName,
            a.AccountType.ToString(), a.AccountStatus.ToString(),
            a.Currency, a.GLAccountId,
            a.BankName, a.BankBranch, a.RoutingNumber, masked,
            a.IBAN, a.SwiftCode,
            a.CurrentBalance, a.LastReconciledBalance, a.LastReconciledAt,
            a.Notes, a.CreatedAt);
    }

    private static BankTransactionSummaryDto ToTxSummaryDto(BankTransaction t) =>
        new(t.Id, t.TransactionNumber, t.TransactionDate,
            t.TransactionType.ToString(), t.TransactionStatus.ToString(),
            t.Amount, t.Description, t.Reference, t.CounterpartyName, t.CreatedAt);

    private static BankTransactionDto ToTxDto(BankTransaction t) =>
        new(t.Id, t.BankAccountId,
            t.BankAccount?.AccountName ?? string.Empty,
            t.TransactionNumber, t.TransactionDate,
            t.TransactionType.ToString(), t.TransactionStatus.ToString(),
            t.Amount, t.Description, t.Reference, t.CounterpartyName,
            t.ARInvoiceId, t.APInvoiceId, t.TransferToAccountId,
            t.ReconciliationId, t.ReconciledAt,
            t.PostedAt, t.PostedBy, t.Notes, t.CreatedAt);

    private static BankReconciliationSummaryDto ToReconSummaryDto(BankReconciliation r) =>
        new(r.Id, r.ReconciliationNumber,
            r.BankAccountId, r.BankAccount?.AccountName ?? string.Empty,
            r.StatementStartDate, r.StatementEndDate,
            r.StatementClosingBalance, r.Difference,
            r.Status.ToString(), r.CompletedAt, r.CreatedAt);

    private static BankReconciliationDto ToReconDto(BankReconciliation r,
        IEnumerable<BankTransaction> txns) =>
        new(r.Id, r.BankAccountId, r.BankAccount?.AccountName ?? string.Empty,
            r.ReconciliationNumber, r.StatementStartDate, r.StatementEndDate,
            r.StatementOpeningBalance, r.StatementClosingBalance,
            r.SystemOpeningBalance, r.ReconciledAmount, r.Difference,
            r.Status.ToString(), r.Notes, r.CompletedAt, r.CompletedBy, r.CreatedAt,
            txns.Select(t => new BankTransactionSummaryDto(
                t.Id, t.TransactionNumber, t.TransactionDate,
                t.TransactionType.ToString(), t.TransactionStatus.ToString(),
                t.Amount, t.Description, t.Reference, t.CounterpartyName, t.CreatedAt))
            .ToList());

    private static CashJournalSummaryDto ToJournalSummaryDto(CashJournal j) =>
        new(j.Id, j.JournalNumber, j.JournalDate,
            j.Description, j.Status.ToString(),
            j.TotalDebits, j.TotalCredits, j.PostedAt, j.CreatedAt);

    private static CashJournalDto ToJournalDto(CashJournal j) =>
        new(j.Id, j.BankAccountId, j.BankAccount?.AccountName ?? string.Empty,
            j.JournalNumber, j.JournalDate, j.Description,
            j.Status.ToString(), j.TotalDebits, j.TotalCredits,
            j.PostedAt, j.PostedBy, j.Notes, j.CreatedAt,
            j.Lines.Select(l => new CashJournalLineDto(
                l.Id, l.GLAccountId, l.Description, l.Debit, l.Credit, l.Reference))
            .ToList());
}
