using DessertERP.Application.Modules.DataManagement.Services;
using Microsoft.Extensions.Logging;

namespace DessertERP.Worker.Jobs;

/// <summary>
/// Hangfire job invoked by the recurring scheduler.
/// Reads a BatchJobConfig by ID, downloads files from Azure Blob,
/// runs Stage → Validate → Promote, optionally auto-confirms sales orders.
/// </summary>
public class BatchImportJob
{
    private readonly IBatchJobService _batchSvc;
    private readonly ILogger<BatchImportJob> _logger;

    public BatchImportJob(IBatchJobService batchSvc, ILogger<BatchImportJob> logger)
    {
        _batchSvc = batchSvc;
        _logger   = logger;
    }

    public async Task RunAsync(Guid jobConfigId)
    {
        _logger.LogInformation("BatchImportJob starting for config {JobConfigId}", jobConfigId);
        try
        {
            var result = await _batchSvc.RunImportJobAsync(jobConfigId);
            if (result.FilesProcessed == 0)
                _logger.LogInformation("BatchImportJob: no files found for config {JobConfigId}", jobConfigId);
            else
                _logger.LogInformation(
                    "BatchImportJob: processed {Files} file(s), {Promoted} promoted, {Failed} failed. {Message}",
                    result.FilesProcessed, result.RowsPromoted, result.RowsFailed, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BatchImportJob failed for config {JobConfigId}", jobConfigId);
            throw; // Hangfire will retry
        }
    }
}

public class BatchExportJob
{
    private readonly IBatchJobService _batchSvc;
    private readonly ILogger<BatchExportJob> _logger;

    public BatchExportJob(IBatchJobService batchSvc, ILogger<BatchExportJob> logger)
    {
        _batchSvc = batchSvc;
        _logger   = logger;
    }

    public async Task RunAsync(Guid jobConfigId)
    {
        _logger.LogInformation("BatchExportJob starting for config {JobConfigId}", jobConfigId);
        try
        {
            var result = await _batchSvc.RunExportJobAsync(jobConfigId);
            _logger.LogInformation("BatchExportJob: {Message}", result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BatchExportJob failed for config {JobConfigId}", jobConfigId);
            throw;
        }
    }
}
