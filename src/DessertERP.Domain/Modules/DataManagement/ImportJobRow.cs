using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.DataManagement;

public enum RowStatus { Pending, Valid, Invalid, Promoted, Skipped }

/// <summary>
/// Staging row — one record per source row in the uploaded file.
/// RawPayload stores the raw field values as JSON so we can re-validate,
/// inspect, or reprocess without re-parsing the original file.
/// </summary>
public class ImportJobRow : BaseEntity
{
    public Guid ImportJobId { get; private set; }
    public int RowNumber { get; private set; }           // 1-based row in source file
    public string RawPayload { get; private set; } = string.Empty;   // JSON dict of raw field values
    public RowStatus Status { get; private set; } = RowStatus.Pending;
    public string? ErrorMessage { get; private set; }
    public DateTime? PromotedAt { get; private set; }
    public Guid? PromotedEntityId { get; private set; } // FK to the real table row after promotion

    // Navigation
    public ImportJob? ImportJob { get; private set; }

    private ImportJobRow() { }

    public ImportJobRow(Guid importJobId, int rowNumber, string rawPayload)
    {
        ImportJobId = importJobId;
        RowNumber = rowNumber;
        RawPayload = rawPayload;
    }

    public void MarkValid()
    {
        Status = RowStatus.Valid;
        ErrorMessage = null;
        SetUpdated();
    }

    public void MarkInvalid(string reason)
    {
        Status = RowStatus.Invalid;
        ErrorMessage = reason;
        SetUpdated();
    }

    public void MarkPromoted(Guid promotedEntityId)
    {
        Status = RowStatus.Promoted;
        PromotedEntityId = promotedEntityId;
        PromotedAt = DateTime.UtcNow;
        ErrorMessage = null;
        SetUpdated();
    }

    public void MarkSkipped(string reason)
    {
        Status = RowStatus.Skipped;
        ErrorMessage = reason;
        SetUpdated();
    }
}
