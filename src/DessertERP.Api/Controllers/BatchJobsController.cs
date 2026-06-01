using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.DataManagement.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/batch-jobs")]
[Produces("application/json")]
public class BatchJobsController : ControllerBase
{
    private readonly IBatchJobService _svc;
    private readonly ICurrentOrganizationService _org;
    private readonly IDataManagementService _dm;
    private readonly IAppDbContext _db;

    public BatchJobsController(
        IBatchJobService svc,
        ICurrentOrganizationService org,
        IDataManagementService dm,
        IAppDbContext db)
    {
        _svc = svc;
        _org = org;
        _dm  = dm;
        _db  = db;
    }

    // ── CRUD ──────────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _svc.GetJobsAsync(_org.OrganizationId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var job = await _svc.GetJobAsync(id, ct);
        return job is null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBatchJobRequest req, CancellationToken ct)
    {
        try
        {
            var created = await _svc.CreateJobAsync(_org.OrganizationId, req, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBatchJobRequest req, CancellationToken ct)
    {
        try
        {
            return Ok(await _svc.UpdateJobAsync(id, req, ct));
        }
        catch (Exception ex)
        {
            return ex.Message.Contains("not found") ? NotFound() : BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteAsync(id, ct); return NoContent(); }
        catch { return NotFound(); }
    }

    // ── Enable / Disable ──────────────────────────────────────────────────────

    [HttpPost("{id:guid}/enable")]
    public async Task<IActionResult> Enable(Guid id, CancellationToken ct)
    {
        try { await _svc.EnableAsync(id, ct); return Ok(new { message = "Job enabled." }); }
        catch { return NotFound(); }
    }

    [HttpPost("{id:guid}/disable")]
    public async Task<IActionResult> Disable(Guid id, CancellationToken ct)
    {
        try { await _svc.DisableAsync(id, ct); return Ok(new { message = "Job disabled." }); }
        catch { return NotFound(); }
    }

    // ── Trigger now (fire-and-forget via Hangfire) ────────────────────────────

    /// <summary>
    /// Immediately enqueue the job as a one-off Hangfire fire-and-forget task.
    /// Does NOT wait for completion — returns the Hangfire job ID.
    /// Poll GET /{id} to see LastRunStatus update after completion.
    /// </summary>
    [HttpPost("{id:guid}/trigger")]
    public async Task<IActionResult> TriggerNow(Guid id, CancellationToken ct)
    {
        var job = await _svc.GetJobAsync(id, ct);
        if (job is null) return NotFound();

        var isImport = job.JobType.StartsWith("Import", StringComparison.OrdinalIgnoreCase);

        string hangfireJobId;
        if (isImport)
            hangfireJobId = BackgroundJob.Enqueue<DessertERP.Worker.Jobs.BatchImportJob>(
                j => j.RunAsync(id));
        else
            hangfireJobId = BackgroundJob.Enqueue<DessertERP.Worker.Jobs.BatchExportJob>(
                j => j.RunAsync(id));

        return Ok(new { message = "Job enqueued.", hangfireJobId });
    }

    /// <summary>
    /// Reset the IsExported flag on specific entity IDs so they are picked up on the next export run.
    /// Body: { "entityType": "SalesOrder", "entityIds": ["guid1", "guid2"] }
    /// </summary>
    [HttpPost("reset-export")]
    public async Task<IActionResult> ResetExport(
        [FromBody] ResetExportRequest req, CancellationToken ct)
    {
        var orgId = _org.OrganizationId;
        var count = await _dm.ResetExportAsync(orgId, req.EntityType, req.EntityIds, ct);
        return Ok(new { message = $"{count} record(s) marked for re-export.", count });
    }

    /// <summary>
    /// Get export audit rows — which entities were included in past export runs.
    /// </summary>
    [HttpGet("export-history")]
    public async Task<IActionResult> ExportHistory(
        [FromQuery] string? entityType, CancellationToken ct)
    {
        var orgId = _org.OrganizationId;
        var q = _db.ExportJobRows.Where(r => r.OrganizationId == orgId);
        if (!string.IsNullOrWhiteSpace(entityType))
            q = q.Where(r => r.EntityType == entityType);

        var rows = await q
            .OrderByDescending(r => r.ExportedAt)
            .Take(200)
            .Select(r => new {
                r.Id, r.EntityType, r.EntityId, r.EntityRef,
                r.BlobName, r.Status, r.ExportedAt, r.ErrorMessage
            })
            .ToListAsync(ct);

        return Ok(rows);
    }
}

public record ResetExportRequest(string EntityType, List<Guid> EntityIds);
