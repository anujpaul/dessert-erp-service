using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.Workflow.Services;

// ── DTOs ─────────────────────────────────────────────────────────────────────

public record WorkflowTemplateDto(
    Guid   Id,
    string Name,
    string DocumentType,
    decimal AmountThreshold,
    bool   IsActive,
    List<WorkflowTemplateStepDto> Steps);

public record WorkflowTemplateStepDto(
    Guid   Id,
    int    StepOrder,
    string StepName,
    string? ApproverRole,
    Guid?  ApproverUserId,
    string? Description);

public record WorkflowInstanceDto(
    Guid   Id,
    string DocumentType,
    Guid   DocumentId,
    string DocumentRef,
    decimal DocumentAmount,
    string Status,
    int    CurrentStepIndex,
    int    TotalSteps,
    string SubmittedBy,
    string? RejectedReason,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    List<WorkflowApprovalStepDto> Steps);

public record WorkflowApprovalStepDto(
    Guid   Id,
    int    StepOrder,
    string StepName,
    string? ApproverRole,
    Guid?  ApproverUserId,
    string Decision,
    string? ActedBy,
    string? ActedByComments,
    DateTime? ActedAt);

public record PendingApprovalDto(
    Guid   WorkflowInstanceId,
    Guid   StepId,
    string DocumentType,
    Guid   DocumentId,
    string DocumentRef,
    decimal DocumentAmount,
    string StepName,
    string? ApproverRole,
    string SubmittedBy,
    DateTime SubmittedAt);

// Requests
public record SubmitForApprovalRequest(
    WorkflowDocumentType DocumentType,
    Guid   DocumentId,
    string DocumentRef,
    decimal DocumentAmount,
    string SubmittedBy);

public record ApproveStepRequest(string ApprovedBy, string? Comments);
public record RejectStepRequest(string RejectedBy, string Reason);

public record SaveTemplateRequest(
    string Name,
    WorkflowDocumentType DocumentType,
    decimal AmountThreshold,
    bool IsActive,
    List<SaveTemplateStepRequest> Steps);

public record SaveTemplateStepRequest(
    int StepOrder, string StepName,
    string? ApproverRole, Guid? ApproverUserId, string? Description);

// ── Interface ─────────────────────────────────────────────────────────────────

public interface IWorkflowService
{
    // Templates (admin configuration)
    Task<IEnumerable<WorkflowTemplateDto>> GetTemplatesAsync(CancellationToken ct = default);
    Task<WorkflowTemplateDto>              GetTemplateAsync(Guid id, CancellationToken ct = default);
    Task<WorkflowTemplateDto>              SaveTemplateAsync(Guid? id, SaveTemplateRequest req, CancellationToken ct = default);
    Task                                   DeleteTemplateAsync(Guid id, CancellationToken ct = default);

    // Submit any document for approval
    Task<WorkflowInstanceDto> SubmitAsync(SubmitForApprovalRequest req, CancellationToken ct = default);

    // Approve / Reject a step
    Task<WorkflowInstanceDto> ApproveStepAsync(Guid instanceId, Guid stepId, ApproveStepRequest req, CancellationToken ct = default);
    Task<WorkflowInstanceDto> RejectStepAsync(Guid instanceId, Guid stepId, RejectStepRequest req, CancellationToken ct = default);
    Task<WorkflowInstanceDto> RecallAsync(Guid instanceId, CancellationToken ct = default);

    // Queries
    Task<WorkflowInstanceDto>           GetInstanceAsync(Guid instanceId, CancellationToken ct = default);
    Task<WorkflowInstanceDto?>          GetInstanceForDocumentAsync(WorkflowDocumentType docType, Guid documentId, CancellationToken ct = default);
    Task<IEnumerable<PendingApprovalDto>> GetPendingApprovalsAsync(string? approverRole = null, CancellationToken ct = default);
    Task<IEnumerable<WorkflowInstanceDto>> GetInstancesAsync(string? status = null, string? docType = null, CancellationToken ct = default);
}

// ── Implementation ────────────────────────────────────────────────────────────

public class WorkflowService : IWorkflowService
{
    private readonly IAppDbContext _db;
    public WorkflowService(IAppDbContext db) => _db = db;

    // ── Templates ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<WorkflowTemplateDto>> GetTemplatesAsync(CancellationToken ct = default)
    {
        var templates = await _db.WorkflowTemplates
            .Include(t => t.Steps)
            .AsNoTracking()
            .OrderBy(t => t.DocumentType).ThenBy(t => t.AmountThreshold)
            .ToListAsync(ct);
        return templates.Select(ToTemplateDto);
    }

    public async Task<WorkflowTemplateDto> GetTemplateAsync(Guid id, CancellationToken ct = default)
    {
        var t = await _db.WorkflowTemplates.Include(t => t.Steps).AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct)
            ?? throw new InvalidOperationException("Template not found.");
        return ToTemplateDto(t);
    }

    public async Task<WorkflowTemplateDto> SaveTemplateAsync(Guid? id, SaveTemplateRequest req, CancellationToken ct = default)
    {
        WorkflowTemplate template;
        if (id.HasValue)
        {
            template = await _db.WorkflowTemplates.Include(t => t.Steps)
                .FirstOrDefaultAsync(t => t.Id == id.Value, ct)
                ?? throw new InvalidOperationException("Template not found.");
            template.Update(req.Name, req.AmountThreshold, req.IsActive);
        }
        else
        {
            template = new WorkflowTemplate(GetCurrentOrgId(), req.Name, req.DocumentType, req.AmountThreshold);
            _db.WorkflowTemplates.Add(template);
        }
        // Re-add steps (simple replace strategy for now)
        foreach (var s in req.Steps.OrderBy(s => s.StepOrder))
            template.AddStep(s.StepOrder, s.StepName, s.ApproverRole, s.ApproverUserId, s.Description);

        await _db.SaveChangesAsync(ct);
        return await GetTemplateAsync(template.Id, ct);
    }

    public async Task DeleteTemplateAsync(Guid id, CancellationToken ct = default)
    {
        var t = await _db.WorkflowTemplates.FirstOrDefaultAsync(t => t.Id == id, ct)
            ?? throw new InvalidOperationException("Template not found.");
        t.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Submit ────────────────────────────────────────────────────────────────

    public async Task<WorkflowInstanceDto> SubmitAsync(SubmitForApprovalRequest req, CancellationToken ct = default)
    {
        // Find matching template (highest threshold that still applies to this amount)
        var template = await _db.WorkflowTemplates
            .Include(t => t.Steps)
            .AsNoTracking()
            .Where(t => t.IsActive && t.DocumentType == req.DocumentType
                        && t.AmountThreshold <= req.DocumentAmount)
            .OrderByDescending(t => t.AmountThreshold)
            .FirstOrDefaultAsync(ct);

        var instance = new WorkflowInstance(
            GetCurrentOrgId(),
            req.DocumentType,
            req.DocumentId,
            req.DocumentRef,
            req.DocumentAmount,
            req.SubmittedBy,
            template?.Id,
            template?.Steps.Count ?? 1);

        _db.WorkflowInstances.Add(instance);

        if (template != null)
        {
            foreach (var step in template.Steps.OrderBy(s => s.StepOrder))
                instance.AddApprovalStep(step.StepOrder, step.StepName,
                    step.ApproverRole, step.ApproverUserId);
        }
        else
        {
            // No template found — auto-approve (no approval needed)
            instance.AddApprovalStep(1, "Auto-Approved", null, null);
            // We'll immediately approve it below after save
        }

        await _db.SaveChangesAsync(ct);

        // Auto-approve if no template
        if (template == null)
        {
            var step = instance.ApprovalSteps.First();
            instance.Approve(step.Id, "System", "Auto-approved: no matching workflow rule.");
            await _db.SaveChangesAsync(ct);
        }

        return await GetInstanceAsync(instance.Id, ct);
    }

    // ── Approve / Reject ──────────────────────────────────────────────────────

    public async Task<WorkflowInstanceDto> ApproveStepAsync(Guid instanceId, Guid stepId,
        ApproveStepRequest req, CancellationToken ct = default)
    {
        var instance = await LoadInstance(instanceId, ct);
        instance.Approve(stepId, req.ApprovedBy, req.Comments);
        await _db.SaveChangesAsync(ct);
        return await GetInstanceAsync(instanceId, ct);
    }

    public async Task<WorkflowInstanceDto> RejectStepAsync(Guid instanceId, Guid stepId,
        RejectStepRequest req, CancellationToken ct = default)
    {
        var instance = await LoadInstance(instanceId, ct);
        instance.Reject(stepId, req.RejectedBy, req.Reason);
        await _db.SaveChangesAsync(ct);
        return await GetInstanceAsync(instanceId, ct);
    }

    public async Task<WorkflowInstanceDto> RecallAsync(Guid instanceId, CancellationToken ct = default)
    {
        var instance = await LoadInstance(instanceId, ct);
        instance.Recall();
        await _db.SaveChangesAsync(ct);
        return await GetInstanceAsync(instanceId, ct);
    }

    // ── Queries ───────────────────────────────────────────────────────────────

    public async Task<WorkflowInstanceDto> GetInstanceAsync(Guid instanceId, CancellationToken ct = default)
    {
        var i = await _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == instanceId, ct)
            ?? throw new InvalidOperationException("Workflow instance not found.");
        return ToInstanceDto(i);
    }

    public async Task<WorkflowInstanceDto?> GetInstanceForDocumentAsync(
        WorkflowDocumentType docType, Guid documentId, CancellationToken ct = default)
    {
        var i = await _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .AsNoTracking()
            .Where(i => i.DocumentType == docType && i.DocumentId == documentId)
            .OrderByDescending(i => i.CreatedAt)
            .FirstOrDefaultAsync(ct);
        return i == null ? null : ToInstanceDto(i);
    }

    public async Task<IEnumerable<PendingApprovalDto>> GetPendingApprovalsAsync(
        string? approverRole = null, CancellationToken ct = default)
    {
        var query = _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .AsNoTracking()
            .Where(i => i.Status == ApprovalStatus.Submitted
                     || i.Status == ApprovalStatus.UnderReview);

        var instances = await query.ToListAsync(ct);

        var pending = new List<PendingApprovalDto>();
        foreach (var inst in instances)
        {
            var currentStep = inst.ApprovalSteps
                .OrderBy(s => s.StepOrder)
                .Skip(inst.CurrentStepIndex)
                .FirstOrDefault(s => s.Decision == StepDecision.Pending);

            if (currentStep == null) continue;
            if (approverRole != null &&
                currentStep.ApproverRole != null &&
                !currentStep.ApproverRole.Equals(approverRole, StringComparison.OrdinalIgnoreCase))
                continue;

            pending.Add(new PendingApprovalDto(
                inst.Id, currentStep.Id,
                inst.DocumentType.ToString(),
                inst.DocumentId,
                inst.DocumentRef,
                inst.DocumentAmount,
                currentStep.StepName,
                currentStep.ApproverRole,
                inst.SubmittedBy,
                inst.CreatedAt));
        }
        return pending.OrderBy(p => p.SubmittedAt);
    }

    public async Task<IEnumerable<WorkflowInstanceDto>> GetInstancesAsync(
        string? status = null, string? docType = null, CancellationToken ct = default)
    {
        var query = _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(i => i.Status.ToString() == status);
        if (!string.IsNullOrWhiteSpace(docType))
            query = query.Where(i => i.DocumentType == Enum.Parse<WorkflowDocumentType>(docType));

        var list = await query.OrderByDescending(i => i.CreatedAt).ToListAsync(ct);
        return list.Select(ToInstanceDto);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<WorkflowInstance> LoadInstance(Guid id, CancellationToken ct)
        => await _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .FirstOrDefaultAsync(i => i.Id == id, ct)
           ?? throw new InvalidOperationException("Workflow instance not found.");

    // Temp: org from context — in real app comes from ICurrentOrganizationService
    private Guid GetCurrentOrgId() => Guid.Empty;

    private static WorkflowTemplateDto ToTemplateDto(WorkflowTemplate t) =>
        new(t.Id, t.Name, t.DocumentType.ToString(), t.AmountThreshold, t.IsActive,
            t.Steps.OrderBy(s => s.StepOrder)
                   .Select(s => new WorkflowTemplateStepDto(s.Id, s.StepOrder, s.StepName,
                       s.ApproverRole, s.ApproverUserId, s.Description))
                   .ToList());

    private static WorkflowInstanceDto ToInstanceDto(WorkflowInstance i) =>
        new(i.Id, i.DocumentType.ToString(), i.DocumentId, i.DocumentRef,
            i.DocumentAmount, i.Status.ToString(), i.CurrentStepIndex, i.TotalSteps,
            i.SubmittedBy, i.RejectedReason, i.CreatedAt, i.CompletedAt,
            i.ApprovalSteps.OrderBy(s => s.StepOrder)
                           .Select(s => new WorkflowApprovalStepDto(s.Id, s.StepOrder, s.StepName,
                               s.ApproverRole, s.ApproverUserId,
                               s.Decision.ToString(), s.ActedBy, s.ActedByComments, s.ActedAt))
                           .ToList());
}
