using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.DataManagement.DTOs;
using DessertERP.Domain.Modules.DataManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.DataManagement.Services;

// ── DTOs ──────────────────────────────────────────────────────────────────────

public record BatchJobConfigDto(
    Guid   Id,
    string Name,
    string JobType,
    bool   IsEnabled,
    string CronExpression,
    string LocalInboxPath,
    string LocalProcessedPath,
    string LocalErrorPath,
    string? LocalExportPath,
    string FileFormat,
    string? ExportFileNamePattern,
    bool   AutoConfirmSalesOrders,
    string LastRunStatus,
    DateTime? LastRunAt,
    string? LastRunMessage,
    int    LastRunFilesProcessed,
    int    LastRunRowsPromoted,
    DateTime CreatedAt
);

public record CreateBatchJobRequest(
    string Name,
    string JobType,             // e.g. "ImportSalesOrder"
    string LocalInboxPath,
    string LocalProcessedPath,
    string LocalErrorPath,
    string FileFormat,
    string CronExpression,
    bool   AutoConfirmSalesOrders,
    string? LocalExportPath           = null,
    string? ExportFileNamePattern     = null
);

public record UpdateBatchJobRequest(
    string Name,
    string CronExpression,
    bool   IsEnabled,
    string LocalInboxPath,
    string LocalProcessedPath,
    string LocalErrorPath,
    string FileFormat,
    bool   AutoConfirmSalesOrders,
    string? LocalExportPath           = null,
    string? ExportFileNamePattern     = null
);

// ── Interface ─────────────────────────────────────────────────────────────────

public interface IBatchJobService
{
    Task<List<BatchJobConfigDto>> GetJobsAsync(Guid orgId, CancellationToken ct = default);
    Task<BatchJobConfigDto?> GetJobAsync(Guid jobId, CancellationToken ct = default);
    Task<BatchJobConfigDto> CreateJobAsync(Guid orgId, CreateBatchJobRequest req, CancellationToken ct = default);
    Task<BatchJobConfigDto> UpdateJobAsync(Guid jobId, UpdateBatchJobRequest req, CancellationToken ct = default);
    Task EnableAsync(Guid jobId, CancellationToken ct = default);
    Task DisableAsync(Guid jobId, CancellationToken ct = default);
    Task DeleteAsync(Guid jobId, CancellationToken ct = default);

    // Called by Hangfire job
    Task<BatchJobRunResult> RunImportJobAsync(Guid jobConfigId, CancellationToken ct = default);
    Task<BatchJobRunResult> RunExportJobAsync(Guid jobConfigId, CancellationToken ct = default);
}

public record BatchJobRunResult(
    bool   Success,
    int    FilesProcessed,
    int    RowsPromoted,
    int    RowsFailed,
    string? Message
);

// ── Implementation ─────────────────────────────────────────────────────────────

public class BatchJobService : IBatchJobService
{
    private readonly IAppDbContext _db;
    private readonly IDataManagementService _dm;

    public BatchJobService(IAppDbContext db, IDataManagementService dm)
    {
        _db = db;
        _dm = dm;
    }

    // ── CRUD ──────────────────────────────────────────────────────────────────

    public async Task<List<BatchJobConfigDto>> GetJobsAsync(Guid orgId, CancellationToken ct = default)
    {
        var jobs = await _db.BatchJobConfigs
            .Where(j => j.OrganizationId == orgId)
            .OrderBy(j => j.Name)
            .ToListAsync(ct);
        return jobs.Select(ToDto).ToList();
    }

    public async Task<BatchJobConfigDto?> GetJobAsync(Guid jobId, CancellationToken ct = default)
    {
        var job = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobId, ct);
        return job is null ? null : ToDto(job);
    }

    public async Task<BatchJobConfigDto> CreateJobAsync(Guid orgId, CreateBatchJobRequest req, CancellationToken ct = default)
    {
        var jobType = Enum.Parse<BatchJobType>(req.JobType, true);

        var config = new BatchJobConfig(orgId, req.Name, jobType,
            req.LocalInboxPath, req.LocalProcessedPath, req.LocalErrorPath,
            req.FileFormat, req.CronExpression, req.AutoConfirmSalesOrders,
            req.LocalExportPath, req.ExportFileNamePattern);

        _db.BatchJobConfigs.Add(config);
        await _db.SaveChangesAsync(ct);
        return ToDto(config);
    }

    public async Task<BatchJobConfigDto> UpdateJobAsync(Guid jobId, UpdateBatchJobRequest req, CancellationToken ct = default)
    {
        var config = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Batch job not found.");

        config.Update(req.Name, req.CronExpression, req.IsEnabled,
            req.LocalInboxPath, req.LocalProcessedPath, req.LocalErrorPath,
            req.FileFormat, req.AutoConfirmSalesOrders,
            req.LocalExportPath, req.ExportFileNamePattern);

        await _db.SaveChangesAsync(ct);
        return ToDto(config);
    }

    public async Task EnableAsync(Guid jobId, CancellationToken ct = default)
    {
        var config = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Batch job not found.");
        config.Enable();
        await _db.SaveChangesAsync(ct);
    }

    public async Task DisableAsync(Guid jobId, CancellationToken ct = default)
    {
        var config = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Batch job not found.");
        config.Disable();
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid jobId, CancellationToken ct = default)
    {
        var config = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobId, ct)
            ?? throw new InvalidOperationException("Batch job not found.");
        config.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Import batch run ───────────────────────────────────────────────────────

    public async Task<BatchJobRunResult> RunImportJobAsync(Guid jobConfigId, CancellationToken ct = default)
    {
        var config = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobConfigId, ct)
            ?? throw new InvalidOperationException("Batch job config not found.");

        if (!config.IsEnabled)
            return new BatchJobRunResult(true, 0, 0, 0, "Job is disabled.");

        var inboxPath = config.LocalInboxPath;
        if (!Directory.Exists(inboxPath))
        {
            var msg = $"Inbox folder does not exist: {inboxPath}";
            config.RecordRun(BatchJobRunStatus.Failed, msg);
            await _db.SaveChangesAsync(ct);
            return new BatchJobRunResult(false, 0, 0, 0, msg);
        }

        var ext   = "*." + config.FileFormat.ToLower();
        var files = Directory.GetFiles(inboxPath, ext);

        if (files.Length == 0)
        {
            config.RecordRun(BatchJobRunStatus.NoFilesFound, "No files found in inbox.", 0, 0);
            await _db.SaveChangesAsync(ct);
            return new BatchJobRunResult(true, 0, 0, 0, "No files found.");
        }

        var entityType = config.JobType switch
        {
            BatchJobType.ImportSalesOrder    => "SalesOrder",
            BatchJobType.ImportPurchaseOrder => "PurchaseOrder",
            BatchJobType.ImportVendor        => "Vendor",
            BatchJobType.ImportProduct       => "Product",
            _ => throw new InvalidOperationException($"Job type {config.JobType} is not an import job.")
        };

        int totalFiles = 0, totalPromoted = 0, totalFailed = 0;
        var errors = new List<string>();

        var org = await _db.Organizations.FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException("No organization found.");

        // Ensure processed / error folders exist
        Directory.CreateDirectory(config.LocalProcessedPath);
        Directory.CreateDirectory(config.LocalErrorPath);

        foreach (var filePath in files)
        {
            var fileName = Path.GetFileName(filePath);
            var stamp    = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            try
            {
                // Create import job using the file path directly (no temp copy needed)
                var importJobDto = await _dm.CreateImportJobAsync(
                    org.Id, entityType, config.FileFormat, fileName, filePath, "batch-job", ct);

                await _dm.StageAsync(importJobDto.Id, ct);
                await _dm.ValidateAsync(importJobDto.Id, ct);
                await _dm.PromoteAsync(importJobDto.Id, ct);

                var importJob = await _dm.GetImportJobAsync(importJobDto.Id, ct);

                if (config.AutoConfirmSalesOrders && config.JobType == BatchJobType.ImportSalesOrder)
                    await _dm.AutoConfirmSalesOrdersAsync(importJobDto.Id, ct);

                totalFiles++;
                totalPromoted += importJob?.PromotedRows ?? 0;
                totalFailed   += importJob?.InvalidRows  ?? 0;

                // Move to processed
                var dest = Path.Combine(config.LocalProcessedPath, $"{stamp}_{fileName}");
                File.Move(filePath, dest, overwrite: true);
            }
            catch (Exception ex)
            {
                errors.Add($"{fileName}: {ex.Message}");
                try
                {
                    var errDest = Path.Combine(config.LocalErrorPath, $"{stamp}_{fileName}");
                    File.Move(filePath, errDest, overwrite: true);
                }
                catch { /* best effort */ }
            }
        }

        var status = errors.Any()
            ? (totalFiles > 0 ? BatchJobRunStatus.PartialSuccess : BatchJobRunStatus.Failed)
            : BatchJobRunStatus.Success;

        var message = errors.Any()
            ? $"Processed {totalFiles} file(s). Errors: {string.Join("; ", errors.Take(3))}"
            : $"Processed {totalFiles} file(s), {totalPromoted} rows promoted.";

        config.RecordRun(status, message, totalFiles, totalPromoted);
        await _db.SaveChangesAsync(ct);

        return new BatchJobRunResult(!errors.Any() || totalFiles > 0, totalFiles, totalPromoted, totalFailed, message);
    }

    // ── Export batch run ───────────────────────────────────────────────────────

    public async Task<BatchJobRunResult> RunExportJobAsync(Guid jobConfigId, CancellationToken ct = default)
    {
        var config = await _db.BatchJobConfigs.FirstOrDefaultAsync(j => j.Id == jobConfigId, ct)
            ?? throw new InvalidOperationException("Batch job config not found.");

        if (!config.IsEnabled)
            return new BatchJobRunResult(true, 0, 0, 0, "Job is disabled.");

        var entityType = config.JobType switch
        {
            BatchJobType.ExportSalesOrder    => "SalesOrder",
            BatchJobType.ExportPurchaseOrder => "PurchaseOrder",
            BatchJobType.ExportVendor        => "Vendor",
            BatchJobType.ExportProduct       => "Product",
            _ => throw new InvalidOperationException($"Job type {config.JobType} is not an export job.")
        };

        var org = await _db.Organizations.FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException("No organization found.");

        try
        {
            var result = await _dm.ExportUnexportedAsync(org.Id, entityType, config.FileFormat, ct);

            if (result.EntityCount == 0)
            {
                config.RecordRun(BatchJobRunStatus.NoFilesFound, "No new records to export.", 0, 0);
                await _db.SaveChangesAsync(ct);
                return new BatchJobRunResult(true, 0, 0, 0, "No new records to export.");
            }

            var exportFolder = config.LocalExportPath?.Trim();
            if (string.IsNullOrEmpty(exportFolder))
                exportFolder = Path.Combine(config.LocalInboxPath, "..", "Export");

            Directory.CreateDirectory(exportFolder);

            var pattern  = config.ExportFileNamePattern
                ?? $"{entityType.ToLower()}-export-{{date}}.{config.FileFormat.ToLower()}";
            var outName  = pattern.Replace("{date}", DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"));
            var outPath  = Path.Combine(exportFolder, outName);

            await File.WriteAllBytesAsync(outPath, result.Data, ct);

            // Write audit rows for each exported entity
            foreach (var (entityId, entityRef) in result.ExportedEntities)
            {
                _db.ExportJobRows.Add(new ExportJobRow(
                    org.Id, config.Id, entityType, entityId, entityRef, outPath));
            }

            config.RecordRun(BatchJobRunStatus.Success,
                $"Exported {result.EntityCount} record(s) to {outPath}",
                filesProcessed: 1, rowsPromoted: result.EntityCount);
            await _db.SaveChangesAsync(ct);

            return new BatchJobRunResult(true, 1, result.EntityCount, 0,
                $"Exported {result.EntityCount} record(s) to: {outPath}");
        }
        catch (Exception ex)
        {
            config.RecordRun(BatchJobRunStatus.Failed, ex.Message);
            await _db.SaveChangesAsync(ct);
            return new BatchJobRunResult(false, 0, 0, 0, ex.Message);
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static BatchJobConfigDto ToDto(BatchJobConfig j) => new(
        j.Id, j.Name, j.JobType.ToString(), j.IsEnabled, j.CronExpression,
        j.LocalInboxPath, j.LocalProcessedPath, j.LocalErrorPath, j.LocalExportPath,
        j.FileFormat, j.ExportFileNamePattern,
        j.AutoConfirmSalesOrders, j.LastRunStatus.ToString(),
        j.LastRunAt, j.LastRunMessage, j.LastRunFilesProcessed, j.LastRunRowsPromoted,
        j.CreatedAt);
}
