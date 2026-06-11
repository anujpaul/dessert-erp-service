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

    [HttpGet("fiscal-years/{fyId:guid}/periods")]
    public async Task<IActionResult> GetPeriods(Guid fyId, CancellationToken ct)
        => Ok(await _svc.GetPeriodsAsync(fyId, ct));

    [HttpPost("fiscal-years/{fyId:guid}/periods")]
    public async Task<IActionResult> CreatePeriod(Guid fyId, [FromBody] CreatePeriodRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreatePeriodAsync(fyId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("fiscal-years/{fyId:guid}/periods/generate")]
    public async Task<IActionResult> GeneratePeriods(Guid fyId, [FromBody] GeneratePeriodsRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.GeneratePeriodsAsync(fyId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("fiscal-years/{fyId:guid}/periods/{periodId:guid}")]
    public async Task<IActionResult> UpdatePeriod(Guid fyId, Guid periodId, [FromBody] UpdatePeriodRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdatePeriodAsync(fyId, periodId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("fiscal-years/{fyId:guid}/periods/{periodId:guid}")]
    public async Task<IActionResult> DeletePeriod(Guid fyId, Guid periodId, CancellationToken ct)
    {
        try { await _svc.DeletePeriodAsync(fyId, periodId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

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

    // ── Currencies ────────────────────────────────────────────────────────────

    [HttpGet("currencies")]
    public async Task<IActionResult> GetCurrencies([FromQuery] bool activeOnly = false, CancellationToken ct = default)
        => Ok(await _svc.GetCurrenciesAsync(activeOnly, ct));

    [HttpGet("currencies/base")]
    public async Task<IActionResult> GetBaseCurrency(CancellationToken ct)
    {
        var dto = await _svc.GetBaseCurrencyAsync(ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("currencies/{id:guid}")]
    public async Task<IActionResult> GetCurrency(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetCurrencyAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("currencies")]
    public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateCurrencyAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("currencies/{id:guid}")]
    public async Task<IActionResult> UpdateCurrency(Guid id, [FromBody] UpdateCurrencyRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateCurrencyAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPatch("currencies/{id:guid}/exchange-rate")]
    public async Task<IActionResult> UpdateExchangeRate(Guid id, [FromBody] UpdateExchangeRateRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateExchangeRateAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("currencies/{id:guid}/set-base")]
    public async Task<IActionResult> SetBaseCurrency(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.SetBaseCurrencyAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("currencies/{id:guid}/activate")]
    public async Task<IActionResult> ActivateCurrency(Guid id, CancellationToken ct)
    {
        try { await _svc.ActivateCurrencyAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("currencies/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCurrency(Guid id, CancellationToken ct)
    {
        try { await _svc.DeactivateCurrencyAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}
