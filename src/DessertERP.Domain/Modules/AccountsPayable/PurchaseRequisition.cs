using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum PRStatus
{
    Draft     = 1,
    Submitted = 2,   // sent to approver
    Approved  = 3,   // workflow approved — ready for PO conversion
    Rejected  = 4,   // workflow rejected
    Converted = 5,   // PO has been raised from this PR
    Cancelled = 6
}

/// <summary>
/// Purchase Requisition — internal request to buy goods/services.
/// Lifecycle: Draft → Submitted → Approved → Converted (to PO).
/// A WorkflowInstance is created when the PR is submitted.
/// </summary>
public class PurchaseRequisition : BaseEntity
{
    public Guid   OrganizationId   { get; private set; }
    public string RequisitionNumber { get; private set; } = string.Empty;
    public string RequestedBy      { get; private set; } = string.Empty;  // username / employee
    public string? DepartmentCode  { get; private set; }
    public string? CostCenterCode  { get; private set; }
    public DateTime NeededByDate   { get; private set; }
    public string? Notes           { get; private set; }
    public PRStatus Status         { get; private set; } = PRStatus.Draft;

    // Workflow linkage
    public Guid? WorkflowInstanceId { get; private set; }

    // Set when converted to PO
    public Guid? ConvertedToPOId    { get; private set; }
    public DateTime? ConvertedAt    { get; private set; }

    // Rejection reason from workflow
    public string? RejectionReason  { get; private set; }

    public decimal TotalEstimatedCost => Lines.Sum(l => l.EstimatedTotalCost);

    private readonly List<PurchaseRequisitionLine> _lines = new();
    public IReadOnlyCollection<PurchaseRequisitionLine> Lines => _lines.AsReadOnly();

    private PurchaseRequisition() { }

    public PurchaseRequisition(Guid organizationId, string reqNumber,
        string requestedBy, DateTime neededByDate,
        string? departmentCode = null, string? costCenterCode = null, string? notes = null)
    {
        OrganizationId    = organizationId;
        RequisitionNumber = reqNumber.Trim();
        RequestedBy       = requestedBy.Trim();
        NeededByDate      = neededByDate;
        DepartmentCode    = departmentCode;
        CostCenterCode    = costCenterCode;
        Notes             = notes;
    }

    public PurchaseRequisitionLine AddLine(Guid? productId, string description,
        decimal qty, string uom, decimal estimatedUnitCost,
        Guid? suggestedVendorId = null, string? glAccountCode = null)
    {
        if (Status != PRStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to a Draft PR.");
        var line = new PurchaseRequisitionLine(
            Id, productId, description, qty, uom, estimatedUnitCost, suggestedVendorId, glAccountCode);
        _lines.Add(line);
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != PRStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft PR.");
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        SetUpdated();
    }

    /// <summary>Submit PR for approval — creates a workflow instance externally.</summary>
    public void Submit()
    {
        if (Status != PRStatus.Draft)
            throw new InvalidOperationException("Only Draft PRs can be submitted.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot submit a PR with no lines.");
        Status = PRStatus.Submitted;
        SetUpdated();
    }

    /// <summary>Called by the workflow engine when all approval steps pass.</summary>
    public void WorkflowApprove(Guid workflowInstanceId)
    {
        if (Status != PRStatus.Submitted)
            throw new InvalidOperationException("PR must be Submitted to be approved.");
        Status             = PRStatus.Approved;
        WorkflowInstanceId = workflowInstanceId;
        SetUpdated();
    }

    /// <summary>Called by the workflow engine when any step rejects.</summary>
    public void WorkflowReject(Guid workflowInstanceId, string reason)
    {
        if (Status != PRStatus.Submitted)
            throw new InvalidOperationException("PR must be Submitted to be rejected.");
        Status             = PRStatus.Rejected;
        WorkflowInstanceId = workflowInstanceId;
        RejectionReason    = reason;
        SetUpdated();
    }

    /// <summary>Mark as converted once a PO is created from this PR.</summary>
    public void MarkConverted(Guid purchaseOrderId)
    {
        if (Status != PRStatus.Approved)
            throw new InvalidOperationException("Only Approved PRs can be converted to a PO.");
        Status          = PRStatus.Converted;
        ConvertedToPOId = purchaseOrderId;
        ConvertedAt     = DateTime.UtcNow;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == PRStatus.Converted)
            throw new InvalidOperationException("A converted PR cannot be cancelled.");
        if (Status == PRStatus.Rejected)
            throw new InvalidOperationException("A rejected PR is already closed.");
        Status = PRStatus.Cancelled;
        SetUpdated();
    }

    public void UpdateNotes(string? notes) { Notes = notes; SetUpdated(); }
}

public class PurchaseRequisitionLine : BaseEntity
{
    public Guid    RequisitionId      { get; private set; }
    public int     LineNumber         { get; private set; }
    public Guid?   ProductId          { get; private set; }
    public string  Description        { get; private set; } = string.Empty;
    public decimal Quantity           { get; private set; }
    public string  UnitOfMeasure      { get; private set; } = "EA";
    public decimal EstimatedUnitCost  { get; private set; }
    public decimal EstimatedTotalCost => Math.Round(Quantity * EstimatedUnitCost, 4);
    public Guid?   SuggestedVendorId  { get; private set; }
    public string? GlAccountCode      { get; private set; }
    public string? Notes              { get; private set; }

    public PurchaseRequisition? Requisition { get; private set; }

    private PurchaseRequisitionLine() { }

    public PurchaseRequisitionLine(Guid requisitionId, Guid? productId, string description,
        decimal quantity, string uom, decimal estimatedUnitCost,
        Guid? suggestedVendorId = null, string? glAccountCode = null, string? notes = null)
    {
        RequisitionId     = requisitionId;
        ProductId         = productId;
        Description       = description.Trim();
        Quantity          = quantity;
        UnitOfMeasure     = uom;
        EstimatedUnitCost = estimatedUnitCost;
        SuggestedVendorId = suggestedVendorId;
        GlAccountCode     = glAccountCode;
        Notes             = notes;
    }

    public void Update(decimal quantity, decimal estimatedUnitCost,
        Guid? suggestedVendorId, string? glAccountCode, string? notes)
    {
        Quantity          = quantity;
        EstimatedUnitCost = estimatedUnitCost;
        SuggestedVendorId = suggestedVendorId;
        GlAccountCode     = glAccountCode;
        Notes             = notes;
        SetUpdated();
    }
}
