using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.DataManagement;
using DessertERP.Worker.Jobs;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DessertERP.Worker.Workers;

/// <summary>
/// Hosted service that syncs BatchJobConfig records from the database to
/// Hangfire recurring jobs on startup and every 60 seconds thereafter.
///
/// This means: when the user creates/edits/enables/disables a batch job via the API,
/// the Hangfire schedule is updated within 60 seconds without requiring a restart.
/// </summary>
public class BatchJobSchedulerWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BatchJobSchedulerWorker> _logger;
    private static readonly TimeSpan SyncInterval = TimeSpan.FromSeconds(60);

    public BatchJobSchedulerWorker(IServiceScopeFactory scopeFactory,
        ILogger<BatchJobSchedulerWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BatchJobSchedulerWorker starting.");

        // Initial sync immediately on startup
        await SyncJobsAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(SyncInterval, stoppingToken);
            await SyncJobsAsync(stoppingToken);
        }
    }

    private async Task SyncJobsAsync(CancellationToken ct)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            // Load all batch job configs across all orgs (Worker is not org-scoped)
            var configs = await db.BatchJobConfigs
                .IgnoreQueryFilters()
                .Where(j => !j.IsDeleted)
                .ToListAsync(ct);

            foreach (var config in configs)
            {
                var hangfireJobId = $"batch-job-{config.Id}";

                if (!config.IsEnabled)
                {
                    // Remove the recurring job if it was registered
                    RecurringJob.RemoveIfExists(hangfireJobId);
                    continue;
                }

                // Register or update the recurring job with the stored cron expression
                if (IsImportJob(config.JobType))
                {
                    RecurringJob.AddOrUpdate<BatchImportJob>(
                        hangfireJobId,
                        job => job.RunAsync(config.Id),
                        config.CronExpression,
                        new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
                }
                else
                {
                    RecurringJob.AddOrUpdate<BatchExportJob>(
                        hangfireJobId,
                        job => job.RunAsync(config.Id),
                        config.CronExpression,
                        new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
                }
            }

            _logger.LogDebug("Synced {Count} batch job(s) to Hangfire.", configs.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync batch jobs to Hangfire.");
        }
    }

    private static bool IsImportJob(BatchJobType type) => type is
        BatchJobType.ImportSalesOrder or
        BatchJobType.ImportPurchaseOrder or
        BatchJobType.ImportVendor or
        BatchJobType.ImportProduct;
}
