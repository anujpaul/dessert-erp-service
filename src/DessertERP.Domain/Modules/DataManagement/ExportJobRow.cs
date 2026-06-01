using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.DataManagement;

public enum ExportRowStatus { Exported, Failed }

/// <summary>
/// Audit record for each entity exported in a batch export run.
/// </summary>
public class ExportJobRow : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public Guid   BatchJobConfigId { get; private set; }
    public string EntityType      { get; private set; } = string.Empty;  // "SalesOrder", "Vendor", etc.
    public Guid   EntityId        { get; private set; }
    public string EntityRef       { get; private set; } = string.Empty;  // e.g. order number
    public ExportRowStatus Status { get; private set; } = ExportRowStatus.Exported;
    public string  BlobName       { get; private set; } = string.Empty;  // full blob path
    public DateTime ExportedAt    { get; private set; }
    public string? ErrorMessage   { get; private set; }

    private ExportJobRow() { }

    public ExportJobRow(Guid organizationId, Guid batchJobConfigId,
        string entityType, Guid entityId, string entityRef,
        string blobName, ExportRowStatus status = ExportRowStatus.Exported,
        string? errorMessage = null)
    {
        OrganizationId   = organizationId;
        BatchJobConfigId = batchJobConfigId;
        EntityType       = entityType;
        EntityId         = entityId;
        EntityRef        = entityRef;
        BlobName         = blobName;
        Status           = status;
        ExportedAt       = DateTime.UtcNow;
        ErrorMessage     = errorMessage;
    }
}
