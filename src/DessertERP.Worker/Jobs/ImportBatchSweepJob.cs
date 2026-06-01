using DessertERP.Application.Common.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DessertERP.Worker.Jobs;

/// <summary>
/// Hourly sweep: picks up any ImportJobs stuck in Queued status
/// (e.g. API uploaded but server restarted before processing) and re-queues them.
/// </summary>
public class ImportBatchSweepJob
{
    private readonly IAppDbContext _db;
    private readonly ILogger<ImportBatchSweepJob> _logger;

    public ImportBatchSweepJob(IAppDbContext db, ILogger<ImportBatchSweepJob> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SweepAsync()
    {
        var stuckJobs = await _db.ImportJobs
            .IgnoreQueryFilters()
            .Where(j => j.Status == DessertERP.Domain.Modules.DataManagement.ImportJobStatus.Queued
                     && j.CreatedAt < DateTime.UtcNow.AddMinutes(-5))
            .ToListAsync();

        if (!stuckJobs.Any())
        {
            _logger.LogInformation("Sweep: no stuck import jobs found.");
            return;
        }

        _logger.LogInformation("Sweep: re-queuing {Count} stuck import jobs.", stuckJobs.Count);
        foreach (var job in stuckJobs)
            BackgroundJob.Enqueue<ImportBatchJob>(j => j.ProcessAsync(job.Id));
    }
}
