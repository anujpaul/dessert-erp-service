using DessertERP.Application.Modules.DataManagement.Services;
using Microsoft.Extensions.Logging;

namespace DessertERP.Worker.Jobs;

/// <summary>
/// Hangfire job: processes a single ImportJob by ID.
/// Called by the folder watcher, scheduler, and manual triggers.
/// </summary>
public class ImportBatchJob
{
    private readonly IDataManagementService _svc;
    private readonly ILogger<ImportBatchJob> _logger;

    public ImportBatchJob(IDataManagementService svc, ILogger<ImportBatchJob> logger)
    {
        _svc = svc;
        _logger = logger;
    }

    public async Task ProcessAsync(Guid jobId)
    {
        _logger.LogInformation("Processing import job {JobId}", jobId);
        try
        {
            var result = await _svc.ProcessImportJobAsync(jobId);
            _logger.LogInformation(
                "Import job {JobId} completed: {Success} success, {Failed} failed out of {Total} rows.",
                jobId, result.SuccessRows, result.FailedRows, result.TotalRows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Import job {JobId} failed with exception.", jobId);
            throw; // let Hangfire handle retry
        }
    }
}
