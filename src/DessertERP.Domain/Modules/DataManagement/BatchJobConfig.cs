using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.DataManagement;

public enum BatchJobType
{
    ImportSalesOrder,
    ImportPurchaseOrder,
    ImportVendor,
    ImportProduct,
    ExportSalesOrder,
    ExportPurchaseOrder,
    ExportVendor,
    ExportProduct,
}

public enum BatchJobRunStatus { Never, Running, Success, PartialSuccess, Failed, NoFilesFound }

/// <summary>
/// Persisted configuration for a recurring batch job.
/// Hangfire reads these records to register/update recurring jobs dynamically.
/// Files are read from / written to local file system paths (no cloud storage).
/// </summary>
public class BatchJobConfig : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public BatchJobType JobType { get; private set; }
    public bool IsEnabled { get; private set; } = true;

    // Schedule
    public string CronExpression { get; private set; } = "*/5 * * * *";

    // Local file system paths
    /// <summary>Folder to pick up incoming files, e.g. C:\ERP\Imports\SalesOrders\Inbox</summary>
    public string LocalInboxPath { get; private set; } = string.Empty;
    /// <summary>Folder to move successfully processed files into.</summary>
    public string LocalProcessedPath { get; private set; } = string.Empty;
    /// <summary>Folder to move failed files into.</summary>
    public string LocalErrorPath { get; private set; } = string.Empty;
    /// <summary>Folder where export files are written (export jobs only).</summary>
    public string? LocalExportPath { get; private set; }

    public string FileFormat { get; private set; } = "Xml"; // Csv | Json | Xml

    /// <summary>File name pattern for export jobs, e.g. "sales-orders-{date}.csv".</summary>
    public string? ExportFileNamePattern { get; private set; }

    // Auto-confirm imported sales orders
    public bool AutoConfirmSalesOrders { get; private set; } = false;

    // Last run tracking
    public DateTime? LastRunAt { get; private set; }
    public BatchJobRunStatus LastRunStatus { get; private set; } = BatchJobRunStatus.Never;
    public string? LastRunMessage { get; private set; }
    public int LastRunFilesProcessed { get; private set; }
    public int LastRunRowsPromoted { get; private set; }

    private BatchJobConfig() { }

    public BatchJobConfig(Guid organizationId, string name, BatchJobType jobType,
        string localInboxPath, string localProcessedPath, string localErrorPath,
        string fileFormat = "Xml", string cronExpression = "*/5 * * * *",
        bool autoConfirmSalesOrders = false,
        string? localExportPath = null, string? exportFileNamePattern = null)
    {
        OrganizationId         = organizationId;
        Name                   = name.Trim();
        JobType                = jobType;
        LocalInboxPath         = localInboxPath.Trim();
        LocalProcessedPath     = localProcessedPath.Trim();
        LocalErrorPath         = localErrorPath.Trim();
        LocalExportPath        = localExportPath?.Trim();
        FileFormat             = fileFormat;
        CronExpression         = cronExpression;
        AutoConfirmSalesOrders = autoConfirmSalesOrders;
        ExportFileNamePattern  = exportFileNamePattern;
    }

    public void Update(string name, string cronExpression, bool isEnabled,
        string localInboxPath, string localProcessedPath, string localErrorPath,
        string fileFormat, bool autoConfirmSalesOrders,
        string? localExportPath = null, string? exportFileNamePattern = null)
    {
        Name                   = name.Trim();
        CronExpression         = cronExpression;
        IsEnabled              = isEnabled;
        LocalInboxPath         = localInboxPath.Trim();
        LocalProcessedPath     = localProcessedPath.Trim();
        LocalErrorPath         = localErrorPath.Trim();
        LocalExportPath        = localExportPath?.Trim();
        FileFormat             = fileFormat;
        AutoConfirmSalesOrders = autoConfirmSalesOrders;
        ExportFileNamePattern  = exportFileNamePattern;
        SetUpdated();
    }

    public void Enable()  { IsEnabled = true;  SetUpdated(); }
    public void Disable() { IsEnabled = false; SetUpdated(); }

    public void RecordRun(BatchJobRunStatus status, string? message,
        int filesProcessed = 0, int rowsPromoted = 0)
    {
        LastRunAt             = DateTime.UtcNow;
        LastRunStatus         = status;
        LastRunMessage        = message;
        LastRunFilesProcessed = filesProcessed;
        LastRunRowsPromoted   = rowsPromoted;
        SetUpdated();
    }
}
