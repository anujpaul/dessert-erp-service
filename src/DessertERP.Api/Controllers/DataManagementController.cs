using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.DataManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/dm")]
[Produces("application/json")]
public class DataManagementController : ControllerBase
{
    private readonly IDataManagementService _svc;
    private readonly ICurrentOrganizationService _org;
    private readonly IWebHostEnvironment _env;

    public DataManagementController(IDataManagementService svc,
        ICurrentOrganizationService org, IWebHostEnvironment env)
    {
        _svc = svc;
        _org = org;
        _env = env;
    }

    // ── Import jobs ────────────────────────────────────────────────────────────

    [HttpGet("import-jobs")]
    public async Task<IActionResult> GetImportJobs(CancellationToken ct)
        => Ok(await _svc.GetImportJobsAsync(_org.OrganizationId, ct));

    [HttpGet("import-jobs/{id:guid}")]
    public async Task<IActionResult> GetImportJob(Guid id, CancellationToken ct)
    {
        var job = await _svc.GetImportJobAsync(id, ct);
        return job is null ? NotFound() : Ok(job);
    }

    /// <summary>
    /// Paginated staged rows for a job — lets UI drill into row-level validation results.
    /// </summary>
    [HttpGet("import-jobs/{id:guid}/rows")]
    public async Task<IActionResult> GetJobRows(
        Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 100, CancellationToken ct = default)
        => Ok(await _svc.GetStagedRowsAsync(id, page, pageSize, ct));

    // ── Upload ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Upload a file and run the full Stage → Validate → Promote pipeline immediately.
    /// For very large files, prefer /upload-only then poll status and call /promote separately.
    /// </summary>
    [HttpPost("import")]
    [RequestSizeLimit(100 * 1024 * 1024)] // 100 MB
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromQuery] string entityType,
        [FromQuery] string fileFormat,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded." });

        var filePath = await SaveUploadedFile(file, ct);
        var job = await _svc.CreateImportJobAsync(
            _org.OrganizationId, entityType, fileFormat,
            file.FileName, filePath, "upload", ct);

        var result = await _svc.ProcessImportJobAsync(job.Id, ct);
        var updated = await _svc.GetImportJobAsync(job.Id, ct);
        return Ok(new { job = updated, rowResults = result.RowResults });
    }

    /// <summary>
    /// Upload file and stage rows only — returns job ID. Client can poll /import-jobs/{id}
    /// to see validation results, then call /import-jobs/{id}/promote to apply.
    /// Use this for large files to give users a chance to review errors before committing.
    /// </summary>
    [HttpPost("import/stage")]
    [RequestSizeLimit(500 * 1024 * 1024)] // 500 MB
    public async Task<IActionResult> UploadAndStage(
        IFormFile file,
        [FromQuery] string entityType,
        [FromQuery] string fileFormat,
        CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded." });

        var filePath = await SaveUploadedFile(file, ct);
        var job = await _svc.CreateImportJobAsync(
            _org.OrganizationId, entityType, fileFormat,
            file.FileName, filePath, "upload", ct);

        // Stage + Validate only — no promotion yet
        await _svc.StageAsync(job.Id, ct);
        await _svc.ValidateAsync(job.Id, ct);

        var updated = await _svc.GetImportJobAsync(job.Id, ct);
        return Accepted(new
        {
            job     = updated,
            message = $"File staged. {updated!.ValidRows} rows valid, {updated.InvalidRows} rows invalid. Call /import-jobs/{job.Id}/promote to apply valid rows."
        });
    }

    /// <summary>
    /// Promote all Valid staging rows to real tables. Called after reviewing staged results.
    /// </summary>
    [HttpPost("import-jobs/{id:guid}/promote")]
    public async Task<IActionResult> Promote(Guid id, CancellationToken ct)
    {
        var job = await _svc.GetImportJobAsync(id, ct);
        if (job is null) return NotFound();

        await _svc.PromoteAsync(id, ct);
        var updated = await _svc.GetImportJobAsync(id, ct);
        return Ok(new { job = updated });
    }

    // ── Export ─────────────────────────────────────────────────────────────────

    [HttpGet("export")]
    public async Task<IActionResult> Export(
        [FromQuery] string entityType,
        [FromQuery] string fileFormat,
        CancellationToken ct)
    {
        try
        {
            var (data, fileName, contentType) =
                await _svc.ExportAsync(_org.OrganizationId, entityType, fileFormat, ct);
            return File(data, contentType, fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // ── Templates ──────────────────────────────────────────────────────────────

    [HttpGet("template")]
    public IActionResult GetTemplate([FromQuery] string entityType, [FromQuery] string fileFormat)
    {
        try
        {
            var (data, fileName, contentType) = _svc.GetTemplate(entityType, fileFormat);
            return File(data, contentType, fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // ── Private helpers ────────────────────────────────────────────────────────

    private async Task<string> SaveUploadedFile(IFormFile file, CancellationToken ct)
    {
        var uploadDir = Path.Combine(_env.ContentRootPath, "uploads", "imports");
        Directory.CreateDirectory(uploadDir);
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadDir, fileName);
        await using var fs = System.IO.File.Create(filePath);
        await file.CopyToAsync(fs, ct);
        return filePath;
    }
}
