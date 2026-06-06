using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.GeneralLedger.DTOs;
using DessertERP.Domain.Modules.GeneralLedger;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.GeneralLedger.Services;

public interface IGeneralLedgerService
{
    // Fiscal Calendar
    Task<IEnumerable<FiscalYearDto>> GetFiscalYearsAsync(CancellationToken ct = default);
    Task<FiscalYearDto?> GetFiscalYearAsync(Guid id, CancellationToken ct = default);
    Task<FiscalYearDto> CreateFiscalYearAsync(CreateFiscalYearRequest req, CancellationToken ct = default);
    Task CloseFiscalYearAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<FiscalPeriodDto>> GetPeriodsAsync(Guid fiscalYearId, CancellationToken ct = default);
    Task<FiscalPeriodDto> CreatePeriodAsync(Guid fiscalYearId, CreatePeriodRequest req, CancellationToken ct = default);
    Task<IEnumerable<FiscalPeriodDto>> GeneratePeriodsAsync(Guid fiscalYearId, GeneratePeriodsRequest req, CancellationToken ct = default);
    Task<FiscalPeriodDto> UpdatePeriodAsync(Guid fiscalYearId, Guid periodId, UpdatePeriodRequest req, CancellationToken ct = default);
    Task DeletePeriodAsync(Guid fiscalYearId, Guid periodId, CancellationToken ct = default);
    Task ClosePeriodAsync(Guid periodId, CancellationToken ct = default);
    Task<FiscalPeriodDto?> GetCurrentPeriodAsync(CancellationToken ct = default);

    // Chart of Accounts
    Task<IEnumerable<AccountTypeDto>> GetAccountTypesAsync(CancellationToken ct = default);
    Task<IEnumerable<AccountDto>> GetAccountsAsync(CancellationToken ct = default);
    Task<AccountDto> CreateAccountAsync(CreateAccountRequest req, CancellationToken ct = default);
    Task DeactivateAccountAsync(Guid id, CancellationToken ct = default);

    // Journal Entries
    Task<IEnumerable<JournalEntryDto>> GetJournalEntriesAsync(Guid? fiscalPeriodId = null, CancellationToken ct = default);
    Task<JournalEntryDto?> GetJournalEntryAsync(Guid id, CancellationToken ct = default);
    Task<JournalEntryDto> CreateJournalEntryAsync(CreateJournalEntryRequest req, CancellationToken ct = default);
    Task PostJournalEntryAsync(Guid id, CancellationToken ct = default);
    Task VoidJournalEntryAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<TrialBalanceLineDto>> GetTrialBalanceAsync(Guid fiscalPeriodId, CancellationToken ct = default);
}

public class GeneralLedgerService : IGeneralLedgerService
{
    private readonly IAppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public GeneralLedgerService(IAppDbContext db, ICurrentOrganizationService org)
    {
        _db = db;
        _org = org;
    }

    // ── Fiscal Calendar ───────────────────────────────────────────────────────

    public async Task<IEnumerable<FiscalYearDto>> GetFiscalYearsAsync(CancellationToken ct = default)
    {
        var years = await _db.FiscalYears
            .Where(y => !y.IsDeleted)
            .OrderByDescending(y => y.StartDate)
            .ToListAsync(ct);
        return years.Select(ToFiscalYearDto);
    }

    public async Task<FiscalYearDto?> GetFiscalYearAsync(Guid id, CancellationToken ct = default)
    {
        var y = await _db.FiscalYears.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        return y is null ? null : ToFiscalYearDto(y);
    }

    public async Task<FiscalYearDto> CreateFiscalYearAsync(CreateFiscalYearRequest req, CancellationToken ct = default)
    {
        var fy = new FiscalYear(_org.OrganizationId, req.Name, req.Description, req.StartDate, req.EndDate, req.CalendarType);
        // No auto-generation — user defines periods explicitly after creation
        _db.FiscalYears.Add(fy);
        await _db.SaveChangesAsync(ct);
        return ToFiscalYearDto(fy);
    }

    public async Task<FiscalPeriodDto> CreatePeriodAsync(Guid fiscalYearId, CreatePeriodRequest req, CancellationToken ct = default)
    {
        var fy = await _db.FiscalYears
            .Include(y => y.Periods)
            .FirstOrDefaultAsync(y => y.Id == fiscalYearId && !y.IsDeleted, ct)
            ?? throw new InvalidOperationException("Fiscal year not found.");
        var period = fy.AddPeriod(req.Name, req.StartDate, req.EndDate);
        await _db.SaveChangesAsync(ct);
        return ToPeriodDto(period);
    }

    public async Task<IEnumerable<FiscalPeriodDto>> GeneratePeriodsAsync(Guid fiscalYearId, GeneratePeriodsRequest req, CancellationToken ct = default)
    {
        var fy = await _db.FiscalYears
            .Include(y => y.Periods)
            .FirstOrDefaultAsync(y => y.Id == fiscalYearId && !y.IsDeleted, ct)
            ?? throw new InvalidOperationException("Fiscal year not found.");

        // 1. Wipe existing periods from DB
        var existing = await _db.FiscalPeriods.Where(p => p.FiscalYearId == fiscalYearId).ToListAsync(ct);
        _db.FiscalPeriods.RemoveRange(existing);
        await _db.SaveChangesAsync(ct);

        // 2. Build period list using domain helpers (these operate on an in-memory FY)
        var template = new FiscalYear(_org.OrganizationId, fy.Name, fy.Description, fy.StartDate, fy.EndDate, fy.CalendarType);
        switch (req.Type.ToUpperInvariant())
        {
            case "MONTHLY":   template.GenerateMonthlyPeriods();   break;
            case "QUARTERLY": template.GenerateQuarterlyPeriods(); break;
            case "4-4-5":     template.Generate445Periods();       break;
            default: throw new InvalidOperationException(
                $"Unknown period type '{req.Type}'. Valid values: Monthly, Quarterly, 4-4-5.");
        }

        // 3. Persist generated periods with the real FiscalYearId
        var saved = new List<FiscalPeriod>();
        foreach (var p in template.Periods.OrderBy(x => x.PeriodNumber))
        {
            var period = new FiscalPeriod(fiscalYearId, p.PeriodNumber, p.Name, p.StartDate, p.EndDate);
            _db.FiscalPeriods.Add(period);
            saved.Add(period);
        }
        await _db.SaveChangesAsync(ct);

        return saved.Select(ToPeriodDto);
    }

    public async Task<FiscalPeriodDto> UpdatePeriodAsync(Guid fiscalYearId, Guid periodId, UpdatePeriodRequest req, CancellationToken ct = default)
    {
        var period = await _db.FiscalPeriods
            .FirstOrDefaultAsync(p => p.Id == periodId && p.FiscalYearId == fiscalYearId && !p.IsDeleted, ct)
            ?? throw new InvalidOperationException("Period not found.");
        if (period.Status != FiscalPeriodStatus.Open)
            throw new InvalidOperationException("Only Open periods can be edited.");
        period.Update(req.Name, req.StartDate, req.EndDate);
        await _db.SaveChangesAsync(ct);
        return ToPeriodDto(period);
    }

    public async Task DeletePeriodAsync(Guid fiscalYearId, Guid periodId, CancellationToken ct = default)
    {
        var hasEntries = await _db.JournalEntries.AnyAsync(j => j.FiscalPeriodId == periodId && !j.IsDeleted, ct);
        if (hasEntries) throw new InvalidOperationException("Cannot delete a period that has journal entries posted to it.");

        var period = await _db.FiscalPeriods
            .FirstOrDefaultAsync(p => p.Id == periodId && p.FiscalYearId == fiscalYearId && !p.IsDeleted, ct)
            ?? throw new InvalidOperationException("Period not found.");
        if (period.Status != FiscalPeriodStatus.Open)
            throw new InvalidOperationException("Only Open periods can be deleted.");

        _db.FiscalPeriods.Remove(period);
        await _db.SaveChangesAsync(ct);
    }

    public async Task CloseFiscalYearAsync(Guid id, CancellationToken ct = default)
    {
        var fy = await _db.FiscalYears.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new InvalidOperationException("Fiscal year not found.");
        fy.Close();
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<FiscalPeriodDto>> GetPeriodsAsync(Guid fiscalYearId, CancellationToken ct = default)
    {
        var periods = await _db.FiscalPeriods
            .Where(p => p.FiscalYearId == fiscalYearId && !p.IsDeleted)
            .OrderBy(p => p.PeriodNumber)
            .ToListAsync(ct);
        return periods.Select(ToPeriodDto);
    }

    public async Task ClosePeriodAsync(Guid periodId, CancellationToken ct = default)
    {
        var period = await _db.FiscalPeriods.FirstOrDefaultAsync(p => p.Id == periodId && !p.IsDeleted, ct)
            ?? throw new InvalidOperationException("Period not found.");
        period.Close();
        await _db.SaveChangesAsync(ct);
    }

    public async Task<FiscalPeriodDto?> GetCurrentPeriodAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var period = await _db.FiscalPeriods
            .Where(p => !p.IsDeleted && p.Status == FiscalPeriodStatus.Open
                        && p.StartDate <= today && p.EndDate >= today)
            .OrderBy(p => p.StartDate)
            .FirstOrDefaultAsync(ct);
        return period is null ? null : ToPeriodDto(period);
    }

    // ── Chart of Accounts ─────────────────────────────────────────────────────

    public async Task<IEnumerable<AccountTypeDto>> GetAccountTypesAsync(CancellationToken ct = default)
    {
        var types = await _db.AccountTypes.Where(t => !t.IsDeleted).OrderBy(t => t.DisplayOrder).ToListAsync(ct);
        return types.Select(t => new AccountTypeDto(t.Id, t.Code, t.Name, t.Nature.ToString(), t.DisplayOrder));
    }

    public async Task<IEnumerable<AccountDto>> GetAccountsAsync(CancellationToken ct = default)
    {
        var accounts = await _db.Accounts
            .Include(a => a.AccountType)
            .Include(a => a.ParentAccount)
            .Where(a => !a.IsDeleted)
            .OrderBy(a => a.AccountNumber)
            .ToListAsync(ct);
        return accounts.Select(ToAccountDto);
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountRequest req, CancellationToken ct = default)
    {
        var account = new Account(_org.OrganizationId, req.AccountNumber, req.Name, req.AccountTypeId,
            req.IsHeaderAccount, req.ParentAccountId, req.Description, req.Currency);
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync(ct);

        var created = await _db.Accounts
            .Include(a => a.AccountType)
            .Include(a => a.ParentAccount)
            .FirstAsync(a => a.Id == account.Id, ct);
        return ToAccountDto(created);
    }

    public async Task DeactivateAccountAsync(Guid id, CancellationToken ct = default)
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new InvalidOperationException("Account not found.");
        account.Deactivate();
        await _db.SaveChangesAsync(ct);
    }

    // ── Journal Entries ───────────────────────────────────────────────────────

    public async Task<IEnumerable<JournalEntryDto>> GetJournalEntriesAsync(Guid? fiscalPeriodId = null, CancellationToken ct = default)
    {
        var query = _db.JournalEntries
            .Include(e => e.FiscalPeriod)
            .Include(e => e.Lines).ThenInclude(l => l.Account)
            .Where(e => !e.IsDeleted);

        if (fiscalPeriodId.HasValue)
            query = query.Where(e => e.FiscalPeriodId == fiscalPeriodId.Value);

        var entries = await query.OrderByDescending(e => e.EntryDate).ToListAsync(ct);
        return entries.Select(ToJournalEntryDto);
    }

    public async Task<JournalEntryDto?> GetJournalEntryAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await _db.JournalEntries
            .Include(e => e.FiscalPeriod)
            .Include(e => e.Lines).ThenInclude(l => l.Account)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, ct);
        return entry is null ? null : ToJournalEntryDto(entry);
    }

    public async Task<JournalEntryDto> CreateJournalEntryAsync(CreateJournalEntryRequest req, CancellationToken ct = default)
    {
        var count = await _db.JournalEntries.CountAsync(ct) + 1;
        var entry = new JournalEntry(_org.OrganizationId, $"JE-{count:D6}", req.EntryDate, req.FiscalPeriodId,
            req.Description, req.Reference, req.JournalType, req.Currency);

        if (req.Lines != null)
            foreach (var l in req.Lines)
                entry.AddLine(l.AccountId, l.Description, l.Debit, l.Credit);

        _db.JournalEntries.Add(entry);
        await _db.SaveChangesAsync(ct);
        return (await GetJournalEntryAsync(entry.Id, ct))!;
    }

    public async Task PostJournalEntryAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await _db.JournalEntries
            .Include(e => e.Lines)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, ct)
            ?? throw new InvalidOperationException("Journal entry not found.");
        entry.Post();
        await _db.SaveChangesAsync(ct);
    }

    public async Task VoidJournalEntryAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await _db.JournalEntries
            .Include(e => e.Lines)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, ct)
            ?? throw new InvalidOperationException("Journal entry not found.");
        entry.Void();
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<TrialBalanceLineDto>> GetTrialBalanceAsync(Guid fiscalPeriodId, CancellationToken ct = default)
    {
        var lines = await _db.JournalLines
            .Include(l => l.Account).ThenInclude(a => a!.AccountType)
            .Include(l => l.JournalEntry)
            .Where(l => l.JournalEntry!.FiscalPeriodId == fiscalPeriodId
                        && l.JournalEntry.Status == JournalEntryStatus.Posted
                        && !l.JournalEntry.IsDeleted)
            .ToListAsync(ct);

        return lines
            .GroupBy(l => new { l.Account!.AccountNumber, l.Account.Name, TypeName = l.Account.AccountType!.Name })
            .Select(g => new TrialBalanceLineDto(
                g.Key.AccountNumber, g.Key.Name, g.Key.TypeName,
                g.Sum(l => l.Debit), g.Sum(l => l.Credit),
                g.Sum(l => l.Debit) - g.Sum(l => l.Credit)))
            .OrderBy(t => t.AccountNumber);
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static FiscalYearDto ToFiscalYearDto(FiscalYear y) => new(
        y.Id, y.Name, y.Description, y.StartDate, y.EndDate,
        y.CalendarType, y.Status.ToString(), y.PeriodCount, y.CreatedAt);

    private static FiscalPeriodDto ToPeriodDto(FiscalPeriod p) => new(
        p.Id, p.FiscalYearId, p.PeriodNumber, p.Name,
        p.StartDate, p.EndDate, p.Status.ToString());

    private static AccountDto ToAccountDto(Account a) => new(
        a.Id, a.AccountNumber, a.Name, a.Description,
        a.AccountTypeId, a.AccountType?.Name ?? string.Empty,
        a.ParentAccountId, a.ParentAccount?.Name,
        a.IsHeaderAccount, a.AllowManualEntry,
        a.Status.ToString(), a.Currency, a.Level);

    private static JournalEntryDto ToJournalEntryDto(JournalEntry e) => new(
        e.Id, e.EntryNumber, e.EntryDate, e.FiscalPeriodId,
        e.FiscalPeriod?.Name ?? string.Empty,
        e.Description, e.Reference, e.JournalType,
        e.Status.ToString(), e.Currency,
        e.TotalDebit, e.TotalCredit, e.CreatedAt,
        e.Lines.OrderBy(l => l.LineOrder).Select(l => new JournalLineDto(
            l.Id, l.AccountId,
            l.Account?.AccountNumber ?? string.Empty,
            l.Account?.Name ?? string.Empty,
            l.Description, l.Debit, l.Credit, l.LineOrder)).ToList());
}
