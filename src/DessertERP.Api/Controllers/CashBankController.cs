using DessertERP.Application.Modules.CashBank;
using DessertERP.Application.Modules.CashBank.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[ApiController]
[Route("api/cash-bank")]
[Authorize]
public class CashBankController : ControllerBase
{
    private readonly ICashBankService _svc;
    public CashBankController(ICashBankService svc) => _svc = svc;

    // ── Bank Accounts ─────────────────────────────────────────────────────────

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts(
        [FromQuery] bool activeOnly = false, CancellationToken ct = default)
        => Ok(await _svc.GetBankAccountsAsync(activeOnly, ct));

    [HttpGet("accounts/{id:guid}")]
    public async Task<IActionResult> GetAccount(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetBankAccountAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount(
        [FromBody] CreateBankAccountRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateBankAccountAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("accounts/{id:guid}")]
    public async Task<IActionResult> UpdateAccount(
        Guid id, [FromBody] UpdateBankAccountRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateBankAccountAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("accounts/{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        try { await _svc.ActivateBankAccountAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("accounts/{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        try { await _svc.DeactivateBankAccountAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Bank Transactions ─────────────────────────────────────────────────────

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] Guid? bankAccountId, [FromQuery] string? status, CancellationToken ct)
        => Ok(await _svc.GetTransactionsAsync(bankAccountId, status, ct));

    [HttpGet("transactions/{id:guid}")]
    public async Task<IActionResult> GetTransaction(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetTransactionAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("transactions")]
    public async Task<IActionResult> CreateTransaction(
        [FromBody] CreateBankTransactionRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateTransactionAsync(req, ct)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("transactions/{id:guid}/post")]
    public async Task<IActionResult> PostTransaction(
        Guid id, [FromBody] PostTransactionRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.PostTransactionAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("transactions/{id:guid}/void")]
    public async Task<IActionResult> VoidTransaction(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidTransactionAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Bank Reconciliation ───────────────────────────────────────────────────

    [HttpGet("reconciliations")]
    public async Task<IActionResult> GetReconciliations(
        [FromQuery] Guid? bankAccountId, CancellationToken ct)
        => Ok(await _svc.GetReconciliationsAsync(bankAccountId, ct));

    [HttpGet("reconciliations/{id:guid}")]
    public async Task<IActionResult> GetReconciliation(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetReconciliationAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("reconciliations")]
    public async Task<IActionResult> CreateReconciliation(
        [FromBody] CreateReconciliationRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateReconciliationAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reconciliations/{id:guid}/reconcile-transaction")]
    public async Task<IActionResult> ReconcileTransaction(
        Guid id, [FromBody] ReconcileTransactionRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ReconcileTransactionAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reconciliations/{id:guid}/complete")]
    public async Task<IActionResult> CompleteReconciliation(
        Guid id, [FromBody] CompleteReconciliationRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.CompleteReconciliationAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reconciliations/{id:guid}/cancel")]
    public async Task<IActionResult> CancelReconciliation(Guid id, CancellationToken ct)
    {
        try { await _svc.CancelReconciliationAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Cash Journals ─────────────────────────────────────────────────────────

    [HttpGet("journals")]
    public async Task<IActionResult> GetJournals(
        [FromQuery] Guid? bankAccountId, [FromQuery] string? status, CancellationToken ct)
        => Ok(await _svc.GetCashJournalsAsync(bankAccountId, status, ct));

    [HttpGet("journals/{id:guid}")]
    public async Task<IActionResult> GetJournal(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetCashJournalAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost("journals")]
    public async Task<IActionResult> CreateJournal(
        [FromBody] CreateCashJournalRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateCashJournalAsync(req, ct)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("journals/{id:guid}/lines")]
    public async Task<IActionResult> AddLine(
        Guid id, [FromBody] CreateCashJournalLineRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.AddJournalLineAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("journals/{id:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> RemoveLine(Guid id, Guid lineId, CancellationToken ct)
    {
        try { await _svc.RemoveJournalLineAsync(id, lineId, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("journals/{id:guid}/post")]
    public async Task<IActionResult> PostJournal(
        Guid id, [FromBody] PostCashJournalRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.PostCashJournalAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("journals/{id:guid}/void")]
    public async Task<IActionResult> VoidJournal(Guid id, CancellationToken ct)
    {
        try { await _svc.VoidCashJournalAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}
