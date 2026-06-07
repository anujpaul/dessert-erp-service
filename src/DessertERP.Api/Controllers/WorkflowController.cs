using DessertERP.Application.Modules.Workflow.Services;
using DessertERP.Domain.Modules.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/workflow")]
public class WorkflowController : ControllerBase
{
    private readonly IWorkflowService _svc;
    public WorkflowController(IWorkflowService svc) => _svc = svc;

    // ── Templates ─────────────────────────────────────────────────────────────

    [HttpGet("templates")]
    public async Task<IActionResult> GetTemplates(CancellationToken ct)
        => Ok(await _svc.GetTemplatesAsync(ct));

    [HttpGet("templates/{id:guid}")]
    public async Task<IActionResult> GetTemplate(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetTemplateAsync(id, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("templates")]
    public async Task<IActionResult> CreateTemplate([FromBody] SaveTemplateRequest req, CancellationToken ct)
        => Ok(await _svc.SaveTemplateAsync(null, req, ct));

    [HttpPut("templates/{id:guid}")]
    public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] SaveTemplateRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveTemplateAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpDelete("templates/{id:guid}")]
    public async Task<IActionResult> DeleteTemplate(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteTemplateAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Approval Inbox ────────────────────────────────────────────────────────

    /// <summary>All pending approval items. ?role=FinanceManager to filter by approver role.</summary>
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending([FromQuery] string? role = null, CancellationToken ct = default)
        => Ok(await _svc.GetPendingApprovalsAsync(role, ct));

    // ── Instances ─────────────────────────────────────────────────────────────

    /// <summary>All instances. ?status=Submitted&amp;docType=APInvoice</summary>
    [HttpGet("instances")]
    public async Task<IActionResult> GetInstances(
        [FromQuery] string? status = null,
        [FromQuery] string? docType = null,
        CancellationToken ct = default)
        => Ok(await _svc.GetInstancesAsync(status, docType, ct));

    [HttpGet("instances/{id:guid}")]
    public async Task<IActionResult> GetInstance(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetInstanceAsync(id, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    /// <summary>Get the latest workflow instance for a specific document.</summary>
    [HttpGet("instances/for-document/{docType}/{documentId:guid}")]
    public async Task<IActionResult> GetInstanceForDocument(
        string docType, Guid documentId, CancellationToken ct)
    {
        if (!Enum.TryParse<WorkflowDocumentType>(docType, out var dt))
            return BadRequest(new { error = "Invalid document type." });
        var inst = await _svc.GetInstanceForDocumentAsync(dt, documentId, ct);
        return inst == null ? NotFound() : Ok(inst);
    }

    // ── Submit / Approve / Reject ─────────────────────────────────────────────

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitForApprovalRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SubmitAsync(req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("instances/{instanceId:guid}/steps/{stepId:guid}/approve")]
    public async Task<IActionResult> ApproveStep(Guid instanceId, Guid stepId,
        [FromBody] ApproveStepRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ApproveStepAsync(instanceId, stepId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("instances/{instanceId:guid}/steps/{stepId:guid}/reject")]
    public async Task<IActionResult> RejectStep(Guid instanceId, Guid stepId,
        [FromBody] RejectStepRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.RejectStepAsync(instanceId, stepId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("instances/{instanceId:guid}/recall")]
    public async Task<IActionResult> Recall(Guid instanceId, CancellationToken ct)
    {
        try { return Ok(await _svc.RecallAsync(instanceId, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}
