using DessertERP.Application.Modules.DataManagement.Services;
using DessertERP.Domain.Modules.DataManagement;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DessertERP.Worker.Jobs;

namespace DessertERP.Worker.Workers;

/// <summary>
/// Watches configured import folders for new CSV/JSON files and
/// auto-queues them as Hangfire import jobs.
///
/// Folder structure:
///   {WatchFolder}/vendors/       ← drop vendor files here
///   {WatchFolder}/products/
///   {WatchFolder}/sales-orders/
///   {WatchFolder}/purchase-orders/
///
/// Processed files are moved to {ProcessedFolder}/{entityType}/{timestamp}_{file}.
/// Failed files are moved to {ErrorFolder}/{entityType}/{timestamp}_{file}.
/// </summary>
public class FolderWatcherWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FolderWatcherWorker> _logger;
    private readonly string _watchFolder;
    private readonly string _processedFolder;
    private readonly string _errorFolder;

    // entity-type folder name → (EntityType, default format)
    private static readonly Dictionary<string, (string EntityType, string DefaultFormat)> FolderMap = new()
    {
        ["vendors"]         = ("Vendor",        "Csv"),
        ["products"]        = ("Product",       "Csv"),
        ["sales-orders"]    = ("SalesOrder",    "Csv"),
        ["purchase-orders"] = ("PurchaseOrder", "Csv"),
    };

    public FolderWatcherWorker(IServiceScopeFactory scopeFactory,
        ILogger<FolderWatcherWorker> logger, IConfiguration config)
    {
        _scopeFactory    = scopeFactory;
        _logger          = logger;
        _watchFolder     = config["ImportSettings:WatchFolder"]   ?? Path.Combine(AppContext.BaseDirectory, "imports");
        _processedFolder = config["ImportSettings:ProcessedFolder"] ?? Path.Combine(_watchFolder, "Processed");
        _errorFolder     = config["ImportSettings:ErrorFolder"]   ?? Path.Combine(_watchFolder, "Errors");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FolderWatcherWorker starting. Watching: {Folder}", _watchFolder);

        // Ensure directories exist
        foreach (var sub in FolderMap.Keys)
        {
            Directory.CreateDirectory(Path.Combine(_watchFolder, sub));
            Directory.CreateDirectory(Path.Combine(_processedFolder, sub));
            Directory.CreateDirectory(Path.Combine(_errorFolder, sub));
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ScanFoldersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scanning import folders.");
            }
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task ScanFoldersAsync(CancellationToken ct)
    {
        foreach (var (subFolder, (entityType, defaultFormat)) in FolderMap)
        {
            var dir = Path.Combine(_watchFolder, subFolder);
            var files = Directory.GetFiles(dir, "*.csv")
                .Concat(Directory.GetFiles(dir, "*.json"))
                .Concat(Directory.GetFiles(dir, "*.xml"))
                .ToArray();

            foreach (var filePath in files)
            {
                await ProcessFileAsync(filePath, entityType, defaultFormat, subFolder, ct);
            }
        }
    }

    private async Task ProcessFileAsync(string filePath, string entityType,
        string defaultFormat, string subFolder, CancellationToken ct)
    {
        var ext = Path.GetExtension(filePath).TrimStart('.').ToLower();
        var fileFormat = ext == "json" ? "Json" : ext == "xml" ? "Xml" : "Csv";
        var fileName = Path.GetFileName(filePath);

        _logger.LogInformation("Detected new file: {File} ({EntityType}, {Format})",
            fileName, entityType, fileFormat);

        try
        {
            // Move to a staging location so the watcher doesn't pick it up again
            var stagingPath = filePath + ".processing";
            File.Move(filePath, stagingPath);

            // We need an orgId — for folder-drop imports we use the default org
            // (In production you'd embed org info in the filename or a sidecar .json)
            Guid orgId;
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DessertERP.Application.Common.Interfaces.IAppDbContext>();
                var org = await db.Organizations.FirstOrDefaultAsync(ct);
                if (org is null)
                {
                    _logger.LogWarning("No organization found — skipping file {File}.", fileName);
                    File.Move(stagingPath, filePath); // restore
                    return;
                }
                orgId = org.Id;
            }

            // Create the import job record
            Guid jobId;
            using (var scope = _scopeFactory.CreateScope())
            {
                var svc = scope.ServiceProvider.GetRequiredService<IDataManagementService>();
                var job = await svc.CreateImportJobAsync(orgId, entityType, fileFormat,
                    fileName, stagingPath, "folder-watcher", ct);
                jobId = job.Id;
            }

            // Queue in Hangfire
            BackgroundJob.Enqueue<ImportBatchJob>(j => j.ProcessAsync(jobId));

            // Move to Processed
            var processedPath = Path.Combine(_processedFolder, subFolder,
                $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{fileName}");
            File.Move(stagingPath, processedPath);

            _logger.LogInformation("Queued import job {JobId} for {File}.", jobId, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to queue file {File}.", fileName);
            // Move to error folder
            try
            {
                var errorPath = Path.Combine(_errorFolder, subFolder,
                    $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{fileName}");
                if (File.Exists(filePath)) File.Move(filePath, errorPath);
            }
            catch { /* best effort */ }
        }
    }
}

// Extension so the using statement compiles without extra imports
file static class DbSetExtensions
{
    public static async Task<T?> FirstOrDefaultAsync<T>(
        this Microsoft.EntityFrameworkCore.DbSet<T> set, CancellationToken ct)
        where T : class
        => await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
            .FirstOrDefaultAsync(set, ct);
}
