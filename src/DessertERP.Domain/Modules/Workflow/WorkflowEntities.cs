using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Workflow;

// ─────────────────────────────────────────────────────────────────────────────
// Enums
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Document types that participate in workflow approval.</summary>
public enum WorkflowDocumentType
{
    APInvoice,
    PurchaseOrder,
    ARInvoice,
    SalesOrder,
    JournalEntry,
    ExpenseReport,
}

public enum ApprovalStatus
{
    NotRequired,   // below threshold, auto-approved
    Draft,         // document not yet submitted
    Submitted,     // waiting for first approver
    UnderReview,   // at least one step approved, more required
    Approved,      // all steps approved
    Rejected,      // one step rejected
    Recalled,      // submitter recalled before approval
}

public enum StepDecision
{
    Pending,
    Approved,
    Rejected,
    Skipped,
}

// ─────────────────────────────────────────────────────────────────────────────
// WorkflowTemplate — per org, per document type approval rules
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Defines the approval rule for a given document type within an org.
/// Multiple templates can exist per org/docType with different amount bands.
/// </summary>
public class WorkflowTemplate : BaseEntity
{
    public Guid   OrganizationId  { get; private set; }
    public string Name            { get; private set; } = string.Empty;
    public WorkflowDocumentType DocumentType { get; private set; }

    /// <summary>Minimum document amount that triggers this template. 0 = always.</summary>
    public decimal AmountThreshold { get; private set; }

    public bool IsActive { get; private set; } = true;

    private readonly List<WorkflowTemplateStep> _steps = new();
    public IReadOnlyCollection<WorkflowTemplateStep> Steps => _steps.AsReadOnly();

    private WorkflowTemplate() { }

    public WorkflowTemplate(Guid organizationId, string name,
        WorkflowDocumentType documentType, decimal amountThreshold = 0m)
    {
        OrganizationId   = organizationId;
        Name             = name.Trim();
        DocumentType     = documentType;
        AmountThreshold  = amountThreshold;
    }

    public WorkflowTemplateStep AddStep(int order, string stepName,
        string? approverRole = null, Guid? approverUserId = null, string? description = null)
    {
        var step = new WorkflowTemplateStep(Id, order, stepName, approverRole, approverUserId, description);
        _steps.Add(step);
        SetUpdated();
        return step;
    }

    public void Update(string name, decimal amountThreshold, bool isActive)
    {
        Name            = name.Trim();
        AmountThreshold = amountThreshold;
        IsActive        = isActive;
        SetUpdated();
    }
}

/// <summary>One step in a workflow template (e.g. "Line Manager", "Finance Director").</summary>
public class WorkflowTemplateStep : BaseEntity
{
    public Guid   WorkflowTemplateId { get; private set; }
    public int    StepOrder          { get; private set; }
    public string StepName           { get; private set; } = string.Empty;
    /// <summary>Role name (e.g. "FinanceManager") — any user with this role can approve.</summary>
    public string? ApproverRole      { get; private set; }
    /// <summary>Specific user override — if set, only this user can approve this step.</summary>
    public Guid?  ApproverUserId     { get; private set; }
    public string? Description       { get; private set; }

    public WorkflowTemplate? Template { get; private set; }

    private WorkflowTemplateStep() { }

    public WorkflowTemplateStep(Guid templateId, int order, string name,
        string? approverRole, Guid? approverUserId, string? description)
    {
        WorkflowTemplateId = templateId;
        StepOrder          = order;
        StepName           = name.Trim();
        ApproverRole       = approverRole?.Trim();
        ApproverUserId     = approverUserId;
        Description        = description?.Trim();
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// WorkflowInstance — one live approval for a specific document
// ─────────────────────────────────────────────────────────────────────────────

public class WorkflowInstance : BaseEntity
{
    public Guid   OrganizationId  { get; private set; }
    public Guid?  TemplateId      { get; private set; }
    public WorkflowDocumentType DocumentType { get; private set; }

    /// <summary>The ID of the actual document (invoice, PO, etc.).</summary>
    public Guid   DocumentId      { get; private set; }
    public string DocumentRef     { get; private set; } = string.Empty; // human-readable ref e.g. "INV-0042"

    public decimal DocumentAmount { get; private set; }
    public ApprovalStatus Status  { get; private set; } = ApprovalStatus.Submitted;

    public int  CurrentStepIndex  { get; private set; } = 0;
    public int  TotalSteps        { get; private set; } = 1;

    public string  SubmittedBy    { get; private set; } = string.Empty;
    public string? RejectedReason { get; private set; }
    public string? Comments       { get; private set; }

    public DateTime? CompletedAt  { get; private set; }

    public WorkflowTemplate? Template { get; private set; }

    private readonly List<WorkflowApprovalStep> _approvalSteps = new();
    public IReadOnlyCollection<WorkflowApprovalStep> ApprovalSteps => _approvalSteps.AsReadOnly();

    private WorkflowInstance() { }

    public WorkflowInstance(Guid organizationId, WorkflowDocumentType docType,
        Guid documentId, string documentRef, decimal amount, string submittedBy,
        Guid? templateId = null, int totalSteps = 1)
    {
        OrganizationId = organizationId;
        DocumentType   = docType;
        DocumentId     = documentId;
        DocumentRef    = documentRef;
        DocumentAmount = amount;
        SubmittedBy    = submittedBy;
        TemplateId     = templateId;
        TotalSteps     = totalSteps;
        Status         = ApprovalStatus.Submitted;
    }

    public WorkflowApprovalStep AddApprovalStep(int stepOrder, string stepName,
        string? approverRole, Guid? approverUserId)
    {
        var step = new WorkflowApprovalStep(Id, stepOrder, stepName, approverRole, approverUserId);
        _approvalSteps.Add(step);
        TotalSteps = _approvalSteps.Count;
        SetUpdated();
        return step;
    }

    /// <summary>Record an approve decision on the current step.</summary>
    public void Approve(Guid stepId, string approvedBy, string? comments)
    {
        if (Status != ApprovalStatus.Submitted && Status != ApprovalStatus.UnderReview)
            throw new InvalidOperationException($"Cannot approve — workflow is {Status}.");

        var step = _approvalSteps.FirstOrDefault(s => s.Id == stepId)
            ?? throw new InvalidOperationException("Step not found.");
        step.Decide(StepDecision.Approved, approvedBy, comments);

        CurrentStepIndex++;
        if (CurrentStepIndex >= TotalSteps)
        {
            Status      = ApprovalStatus.Approved;
            CompletedAt = DateTime.UtcNow;
        }
        else
        {
            Status = ApprovalStatus.UnderReview;
        }
        SetUpdated();
    }

    /// <summary>Record a reject decision — stops the whole workflow.</summary>
    public void Reject(Guid stepId, string rejectedBy, string reason)
    {
        if (Status != ApprovalStatus.Submitted && Status != ApprovalStatus.UnderReview)
            throw new InvalidOperationException($"Cannot reject — workflow is {Status}.");

        var step = _approvalSteps.FirstOrDefault(s => s.Id == stepId)
            ?? throw new InvalidOperationException("Step not found.");
        step.Decide(StepDecision.Rejected, rejectedBy, reason);

        Status         = ApprovalStatus.Rejected;
        RejectedReason = reason;
        CompletedAt    = DateTime.UtcNow;
        SetUpdated();
    }

    /// <summary>Submitter recalls before any decision is made.</summary>
    public void Recall()
    {
        if (Status == ApprovalStatus.Approved || Status == ApprovalStatus.Rejected)
            throw new InvalidOperationException("Cannot recall a completed workflow.");
        Status = ApprovalStatus.Recalled;
        SetUpdated();
    }
}

/// <summary>One step inside a live workflow instance — records who acted and when.</summary>
public class WorkflowApprovalStep : BaseEntity
{
    public Guid   WorkflowInstanceId { get; private set; }
    public int    StepOrder          { get; private set; }
    public string StepName           { get; private set; } = string.Empty;
    public string? ApproverRole      { get; private set; }
    public Guid?  ApproverUserId     { get; private set; }

    public StepDecision Decision     { get; private set; } = StepDecision.Pending;
    public string? ActedBy           { get; private set; }
    public string? ActedByComments   { get; private set; }
    public DateTime? ActedAt         { get; private set; }

    public WorkflowInstance? WorkflowInstance { get; private set; }

    private WorkflowApprovalStep() { }

    public WorkflowApprovalStep(Guid instanceId, int order, string name,
        string? approverRole, Guid? approverUserId)
    {
        WorkflowInstanceId = instanceId;
        StepOrder          = order;
        StepName           = name.Trim();
        ApproverRole       = approverRole?.Trim();
        ApproverUserId     = approverUserId;
    }

    public void Decide(StepDecision decision, string actedBy, string? comments)
    {
        Decision        = decision;
        ActedBy         = actedBy;
        ActedByComments = comments;
        ActedAt         = DateTime.UtcNow;
        SetUpdated();
    }
}
