using DessertERP.Application.Modules.FixedAssets;
using DessertERP.Application.Modules.FixedAssets.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[ApiController]
[Route("api/fixed-assets")]
[Authorize]
public class FixedAssetsController : ControllerBase
{
    private readonly IFixedAssetService _svc;
    public FixedAssetsController(IFixedAssetService svc) => _svc = svc;

    // ── Assets ────────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetAssets(
        [FromQuery] string? category, [FromQuery] string? status, CancellationToken ct)
        => Ok(await _svc.GetAssetsAsync(category, status, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsset(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetAssetAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsset(
        [FromBody] CreateFixedAssetRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateAssetAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsset(
        Guid id, [FromBody] UpdateFixedAssetRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateAssetAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("{id:guid}/status")]
    public async Task<IActionResult> SetStatus(
        Guid id, [FromBody] SetAssetStatusRequest req, CancellationToken ct)
    {
        try { await _svc.SetAssetStatusAsync(id, req.Status, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
        => Ok(await _svc.GetSummaryStatsAsync(ct));

    // ── Depreciation ──────────────────────────────────────────────────────────

    [HttpPost("{id:guid}/depreciate")]
    public async Task<IActionResult> RunDepreciation(
        Guid id, [FromBody] RunDepreciationRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.RunDepreciationAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("depreciate/bulk")]
    public async Task<IActionResult> RunBulkDepreciation(
        [FromBody] RunBulkDepreciationRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.RunBulkDepreciationAsync(req, ct)); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("{id:guid}/depreciation-schedule")]
    public async Task<IActionResult> GetSchedule(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetDepreciationScheduleAsync(id, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("{id:guid}/depreciation-history")]
    public async Task<IActionResult> GetDepreciationHistory(Guid id, CancellationToken ct)
        => Ok(await _svc.GetDepreciationHistoryAsync(id, ct));

    // ── Disposal ──────────────────────────────────────────────────────────────

    [HttpPost("{id:guid}/dispose")]
    public async Task<IActionResult> DisposeAsset(
        Guid id, [FromBody] DisposeAssetRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.DisposeAssetAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("disposals")]
    public async Task<IActionResult> GetDisposals(CancellationToken ct)
        => Ok(await _svc.GetDisposalsAsync(ct));

    [HttpGet("disposals/{id:guid}")]
    public async Task<IActionResult> GetDisposal(Guid id, CancellationToken ct)
    {
        var dto = await _svc.GetDisposalAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    // ── Transfers ─────────────────────────────────────────────────────────────

    [HttpPost("transfers")]
    public async Task<IActionResult> CreateTransfer(
        [FromBody] CreateTransferRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.CreateTransferAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("transfers")]
    public async Task<IActionResult> GetTransfers(
        [FromQuery] Guid? assetId, CancellationToken ct)
        => Ok(await _svc.GetTransfersAsync(assetId, ct));

    // ── Maintenance ───────────────────────────────────────────────────────────

    [HttpPost("maintenance")]
    public async Task<IActionResult> AddMaintenance(
        [FromBody] CreateMaintenanceRequest req, CancellationToken ct)
    {
        try { return StatusCode(201, await _svc.AddMaintenanceAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("{id:guid}/maintenance")]
    public async Task<IActionResult> GetMaintenance(Guid id, CancellationToken ct)
        => Ok(await _svc.GetMaintenanceAsync(id, ct));

    // ── Impairment ────────────────────────────────────────────────────────────

    [HttpPost("{id:guid}/impair")]
    public async Task<IActionResult> ImpairAsset(
        Guid id, [FromBody] ImpairAssetRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ImpairAssetAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}

// ── Inline request record (used only at API layer) ────────────────────────────
public record SetAssetStatusRequest(string Status);
