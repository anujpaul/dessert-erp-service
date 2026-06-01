using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.DataManagement;

public enum ImportJobStatus { Queued, Processing, Completed, Failed, PartialSuccess }
public enum EntityType { Vendor, Product, SalesOrder, PurchaseOrder }
public enum FileFormat { Csv, Json, Xml }

public class ImportJob : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public EntityType EntityType { get; private set; }
    public FileFormat FileFormat { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public ImportJobStatus Status { get; private set; } = ImportJobStatus.Queued;
    public int TotalRows { get; private set; }
    public int SuccessRows { get; private set; }
    public int FailedRows { get; private set; }
    public string? ErrorSummary { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? TriggeredBy { get; private set; } // "upload", "folder-watcher", "scheduler"

    private ImportJob() { }

    public ImportJob(Guid organizationId, EntityType entityType, FileFormat fileFormat,
        string fileName, string filePath, string? triggeredBy = "upload")
    {
        OrganizationId = organizationId;
        EntityType = entityType;
        FileFormat = fileFormat;
        FileName = fileName;
        FilePath = filePath;
        TriggeredBy = triggeredBy;
    }

    public void Start()
    {
        Status = ImportJobStatus.Processing;
        StartedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void Complete(int totalRows, int successRows, int failedRows, string? errorSummary = null)
    {
        TotalRows = totalRows;
        SuccessRows = successRows;
        FailedRows = failedRows;
        ErrorSummary = errorSummary;
        CompletedAt = DateTime.UtcNow;
        Status = failedRows == 0 ? ImportJobStatus.Completed
               : successRows == 0 ? ImportJobStatus.Failed
               : ImportJobStatus.PartialSuccess;
        SetUpdated();
    }

    public void Fail(string reason)
    {
        Status = ImportJobStatus.Failed;
        ErrorSummary = reason;
        CompletedAt = DateTime.UtcNow;
        SetUpdated();
    }

    /// <summary>Called after the file is parsed and rows staged — before validation/promotion.</summary>
    public void MarkStaged(int totalRows)
    {
        TotalRows = totalRows;
        Status = ImportJobStatus.Processing;
        StartedAt ??= DateTime.UtcNow;
        SetUpdated();
    }
}
