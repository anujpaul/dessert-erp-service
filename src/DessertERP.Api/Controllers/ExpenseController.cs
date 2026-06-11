using DessertERP.Application.Modules.Expenses.Services;
using DessertERP.Application.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[Authorize(Policy = PermissionKeys.ExpenseAccess)]
[ApiController]
[Route("api/expenses")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _svc;
    public ExpenseController(IExpenseService svc) => _svc = svc;

    // ── Categories ────────────────────────────────────────────────────────────

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
        => Ok(await _svc.GetCategoriesAsync(ct));

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] SaveExpenseCategoryRequest req, CancellationToken ct)
        => Ok(await _svc.SaveCategoryAsync(null, req, ct));

    [HttpPut("categories/{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] SaveExpenseCategoryRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SaveCategoryAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpDelete("categories/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteCategoryAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    // ── Expense Reports ───────────────────────────────────────────────────────

    /// <summary>List all expense reports. ?status=Submitted|Approved|Draft|Paid</summary>
    [HttpGet("reports")]
    public async Task<IActionResult> GetReports([FromQuery] string? status = null, CancellationToken ct = default)
        => Ok(await _svc.GetReportsAsync(status, ct));

    [HttpGet("reports/{id:guid}")]
    public async Task<IActionResult> GetReport(Guid id, CancellationToken ct)
    {
        try { return Ok(await _svc.GetReportAsync(id, ct)); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost("reports")]
    public async Task<IActionResult> CreateReport([FromBody] CreateExpenseReportRequest req, CancellationToken ct)
        => Ok(await _svc.CreateReportAsync(req, ct));

    [HttpPut("reports/{id:guid}")]
    public async Task<IActionResult> UpdateReport(Guid id, [FromBody] UpdateExpenseReportRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateReportAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("reports/{id:guid}")]
    public async Task<IActionResult> DeleteReport(Guid id, CancellationToken ct)
    {
        try { await _svc.DeleteReportAsync(id, ct); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Lines ─────────────────────────────────────────────────────────────────

    [HttpPost("reports/{reportId:guid}/lines")]
    public async Task<IActionResult> AddLine(Guid reportId, [FromBody] SaveExpenseLineRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.AddLineAsync(reportId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("reports/{reportId:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> UpdateLine(Guid reportId, Guid lineId,
        [FromBody] SaveExpenseLineRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.UpdateLineAsync(reportId, lineId, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("reports/{reportId:guid}/lines/{lineId:guid}")]
    public async Task<IActionResult> DeleteLine(Guid reportId, Guid lineId, CancellationToken ct)
    {
        try { return Ok(await _svc.DeleteLineAsync(reportId, lineId, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Workflow Actions ──────────────────────────────────────────────────────

    [HttpPost("reports/{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id, [FromBody] SubmitExpenseRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.SubmitAsync(id, req.SubmittedBy, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reports/{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ApproveExpenseRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.ApproveAsync(id, req.ApprovedBy, req.Comments, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reports/{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectExpenseRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.RejectAsync(id, req.RejectedBy, req.Reason, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("reports/{id:guid}/mark-paid")]
    public async Task<IActionResult> MarkPaid(Guid id, [FromBody] MarkPaidRequest req, CancellationToken ct)
    {
        try { return Ok(await _svc.MarkPaidAsync(id, req, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }
}

// Simple request records for the controller (not in service layer)
public record SubmitExpenseRequest(string SubmittedBy);
public record ApproveExpenseRequest(string ApprovedBy, string? Comments);
public record RejectExpenseRequest(string RejectedBy, string Reason);
