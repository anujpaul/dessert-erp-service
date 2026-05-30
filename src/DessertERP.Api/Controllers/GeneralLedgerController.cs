using DessertERP.Application.Modules.GeneralLedger.DTOs;
using DessertERP.Application.Modules.GeneralLedger.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/gl")]
[Produces("application/json")]
public class GeneralLedgerController : ControllerBase
{
    private readonly IGeneralLedgerService _svc;
    public GeneralLedgerController(IGeneralLedgerService svc) => _svc = svc;

    // ── Fiscal Calendar ───────────────────────────────────────────────────────

    [HttpGet("fiscal-years")]
    public async Task<IActionResult> GetFiscalYears(CancellationToken ct)
        => Ok(await _svc.GetFiscalYearsAsync(ct));

    [HttpGet("fiscal-years/{id:guid}")]
    public async Task<IActionResult> GetFiscalYear(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetFiscalYearAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("fiscal-years")]
    public async Task<IActionResult> CreateFiscalYear([FromBody] CreateFiscalYearRequest req, CancellationToken ct)
        => StatusCode(201, await _svc.CreateFiscalYearAsync(req, ct));

    [HttpPost("fiscal-years/{id:guid}/close")]
    public async Task<IActionResult> CloseFiscalYear(Guid id, CancellationToken ct)
    {
        try { await _svc.CloseFiscalYearAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("fiscal-years/{id:guid}/periods")]
    public async Task<IActionResult> GetPeriods(Guid id, CancellationToken ct)
        => Ok(await _svc.GetPeriodsAsync(id, ct));

    [HttpPost("periods/{id:guid}/close")]
    public async Task<IActionResult> ClosePeriod(Guid id, CancellationToken ct)
    {
        try { await _svc.ClosePeriodAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("periods/current")]
    public async Task<IActionResult> GetCurrentPeriod(CancellationToken ct)
    {
        var dto = await _svc.GetCurrentPeriodAsync(ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    // ── Chart of Accounts ─────────────────────────────────────────────────────

    [HttpGet("account-types")]
    public async Task<IActionResult> GetAccountTypes(CancellationToken ct)
        => Ok(await _svc.GetAccountTypesAsync(ct));

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts(CancellationToken ct)
        => Ok(await _svc.GetAccountsAsync(ct));

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateAccountAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("accounts/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateAccount(Guid id, CancellationToken ct)
    {
        try { await _svc.DeactivateAccountAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Journal Entries ───────────────────────────────────────────────────────

    [HttpGet("journal-entries")]
    public async Task<IActionResult> GetJournalEntries([FromQuery] Guid? fiscalPeriodId, CancellationToken ct)
        => Ok(await _svc.GetJournalEntriesAsync(fiscalPeriodId, ct));

    [HttpGet("journal-entries/{id:guid}")]
    public async Task<IActionResult> GetJournalEntry(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetJournalEntryAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("journal-entries")]
    public async Task<IActionResult> CreateJournalEntry([FromBody] CreateJournalEntryRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateJournalEntryAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("journal-entries/{id:guid}/post")]
    public async Task<IActionResult> PostJournalEntry(Guid id, CancellationToken ct)
    {
        try { await _svc.PostJournalEntryAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("journal-entries/{id:guid}/void")]
    public async Task<IActionResult> VoidJournalEntry(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidJournalEntryAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("reports/trial-balance")]
    public async Task<IActionResult> GetTrialBalance([FromQuery] Guid fiscalPeriodId, CancellationToken ct)
        => Ok(await _svc.GetTrialBalanceAsync(fiscalPeriodId, ct));
}
