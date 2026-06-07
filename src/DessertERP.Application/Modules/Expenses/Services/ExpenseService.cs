using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Modules.Workflow.Services;
using DessertERP.Domain.Modules.Expenses;
using DessertERP.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.Expenses.Services;

// ── DTOs ─────────────────────────────────────────────────────────────────────

public record ExpenseCategoryDto(
    Guid Id, string Name, string? Description,
    Guid? GLAccountId, decimal? LimitPerClaim, bool IsActive);

public record ExpenseLineDto(
    Guid Id, Guid CategoryId, string CategoryName,
    string ExpenseDate, decimal Amount, string Description,
    string? Merchant, string? ReceiptUrl, bool IsReimbursable);

public record ExpenseReportDto(
    Guid Id, string ReportNumber, string EmployeeName,
    string? EmployeeEmail, string? Department, string Purpose,
    string PeriodStart, string PeriodEnd, string Currency,
    decimal TotalAmount, decimal ApprovedAmount, decimal PaidAmount,
    string Status, string ApprovalStatus,
    Guid? WorkflowInstanceId,
    string? SubmittedBy, string? SubmittedAt,
    string? ApprovedBy, string? ApprovedAt,
    string? RejectedReason, string? Notes,
    string CreatedAt,
    List<ExpenseLineDto> Lines);

public record ExpenseReportSummaryDto(
    Guid Id, string ReportNumber, string EmployeeName,
    string? Department, string Purpose,
    string PeriodStart, string PeriodEnd,
    decimal TotalAmount, string Status, string ApprovalStatus,
    string CreatedAt);

// Requests
public record SaveExpenseCategoryRequest(
    string Name, string? Description, Guid? GLAccountId, decimal? LimitPerClaim, bool IsActive);

public record CreateExpenseReportRequest(
    string EmployeeName, string? EmployeeEmail, string? Department,
    string Purpose, DateTime PeriodStart, DateTime PeriodEnd,
    string Currency = "USD");

public record UpdateExpenseReportRequest(
    string Purpose, DateTime PeriodStart, DateTime PeriodEnd,
    string? Department, string? Notes);

public record SaveExpenseLineRequest(
    Guid CategoryId, DateTime ExpenseDate, decimal Amount,
    string Description, string? Merchant, string? ReceiptUrl);

public record MarkPaidRequest(decimal Amount);

// ── Interface ─────────────────────────────────────────────────────────────────

public interface IExpenseService
{
    // Categories
    Task<IEnumerable<ExpenseCategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
    Task<ExpenseCategoryDto>  SaveCategoryAsync(Guid? id, SaveExpenseCategoryRequest req, CancellationToken ct = default);
    Task                      DeleteCategoryAsync(Guid id, CancellationToken ct = default);

    // Reports
    Task<IEnumerable<ExpenseReportSummaryDto>> GetReportsAsync(string? status = null, CancellationToken ct = default);
    Task<ExpenseReportDto>    GetReportAsync(Guid id, CancellationToken ct = default);
    Task<ExpenseReportDto>    CreateReportAsync(CreateExpenseReportRequest req, CancellationToken ct = default);
    Task<ExpenseReportDto>    UpdateReportAsync(Guid id, UpdateExpenseReportRequest req, CancellationToken ct = default);
    Task                      DeleteReportAsync(Guid id, CancellationToken ct = default);

    // Lines
    Task<ExpenseReportDto>    AddLineAsync(Guid reportId, SaveExpenseLineRequest req, CancellationToken ct = default);
    Task<ExpenseReportDto>    UpdateLineAsync(Guid reportId, Guid lineId, SaveExpenseLineRequest req, CancellationToken ct = default);
    Task<ExpenseReportDto>    DeleteLineAsync(Guid reportId, Guid lineId, CancellationToken ct = default);

    // Workflow
    Task<ExpenseReportDto>    SubmitAsync(Guid reportId, string submittedBy, CancellationToken ct = default);
    Task<ExpenseReportDto>    ApproveAsync(Guid reportId, string approvedBy, string? comments, CancellationToken ct = default);
    Task<ExpenseReportDto>    RejectAsync(Guid reportId, string rejectedBy, string reason, CancellationToken ct = default);
    Task<ExpenseReportDto>    MarkPaidAsync(Guid reportId, MarkPaidRequest req, CancellationToken ct = default);
}

// ── Implementation ────────────────────────────────────────────────────────────

public class ExpenseService : IExpenseService
{
    private readonly IAppDbContext       _db;
    private readonly IWorkflowService    _workflow;

    public ExpenseService(IAppDbContext db, IWorkflowService workflow)
    {
        _db       = db;
        _workflow = workflow;
    }

    // ── Categories ────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ExpenseCategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var cats = await _db.ExpenseCategories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
        return cats.Select(ToCatDto);
    }

    public async Task<ExpenseCategoryDto> SaveCategoryAsync(Guid? id, SaveExpenseCategoryRequest req, CancellationToken ct = default)
    {
        ExpenseCategory cat;
        if (id.HasValue)
        {
            cat = await _db.ExpenseCategories.FirstOrDefaultAsync(c => c.Id == id.Value, ct)
                ?? throw new InvalidOperationException("Category not found.");
            cat.Update(req.Name, req.Description, req.GLAccountId, req.LimitPerClaim, req.IsActive);
        }
        else
        {
            cat = new ExpenseCategory(GetOrgId(), req.Name, req.Description, req.GLAccountId, req.LimitPerClaim);
            _db.ExpenseCategories.Add(cat);
        }
        await _db.SaveChangesAsync(ct);
        return ToCatDto(cat);
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken ct = default)
    {
        var cat = await _db.ExpenseCategories.FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new InvalidOperationException("Category not found.");
        cat.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Reports ───────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ExpenseReportSummaryDto>> GetReportsAsync(
        string? status = null, CancellationToken ct = default)
    {
        var query = _db.ExpenseReports.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status == Enum.Parse<ExpenseReportStatus>(status));
        var list = await query.OrderByDescending(r => r.CreatedAt).ToListAsync(ct);
        return list.Select(ToSummaryDto);
    }

    public async Task<ExpenseReportDto> GetReportAsync(Guid id, CancellationToken ct = default)
    {
        var r = await _db.ExpenseReports
            .Include(r => r.Lines)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct)
            ?? throw new InvalidOperationException("Expense report not found.");
        return ToDto(r);
    }

    public async Task<ExpenseReportDto> CreateReportAsync(CreateExpenseReportRequest req, CancellationToken ct = default)
    {
        var reportNumber = await GenerateReportNumber(ct);
        var report = new ExpenseReport(GetOrgId(), reportNumber,
            req.EmployeeName, req.Purpose, req.PeriodStart, req.PeriodEnd,
            req.EmployeeEmail, req.Department, req.Currency);
        _db.ExpenseReports.Add(report);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(report.Id, ct);
    }

    public async Task<ExpenseReportDto> UpdateReportAsync(Guid id, UpdateExpenseReportRequest req, CancellationToken ct = default)
    {
        var report = await LoadReport(id, ct);
        report.UpdateHeader(req.Purpose, req.PeriodStart, req.PeriodEnd, req.Department, req.Notes);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(id, ct);
    }

    public async Task DeleteReportAsync(Guid id, CancellationToken ct = default)
    {
        var report = await _db.ExpenseReports.FirstOrDefaultAsync(r => r.Id == id, ct)
            ?? throw new InvalidOperationException("Report not found.");
        if (report.Status != ExpenseReportStatus.Draft)
            throw new InvalidOperationException("Only Draft reports can be deleted.");
        report.SoftDelete();
        await _db.SaveChangesAsync(ct);
    }

    // ── Lines ─────────────────────────────────────────────────────────────────

    public async Task<ExpenseReportDto> AddLineAsync(Guid reportId, SaveExpenseLineRequest req, CancellationToken ct = default)
    {
        var report = await LoadReport(reportId, ct);
        var cat = await _db.ExpenseCategories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == req.CategoryId, ct);
        report.AddLine(req.CategoryId, cat?.Name ?? "Unknown",
            req.ExpenseDate, req.Amount, req.Description, req.Merchant, req.ReceiptUrl);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(reportId, ct);
    }

    public async Task<ExpenseReportDto> UpdateLineAsync(Guid reportId, Guid lineId, SaveExpenseLineRequest req, CancellationToken ct = default)
    {
        await LoadReport(reportId, ct); // validates Draft status
        var line = await _db.ExpenseLines.FirstOrDefaultAsync(l => l.Id == lineId, ct)
            ?? throw new InvalidOperationException("Line not found.");
        var cat = await _db.ExpenseCategories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == req.CategoryId, ct);
        line.Update(req.CategoryId, cat?.Name ?? "Unknown",
            req.ExpenseDate, req.Amount, req.Description, req.Merchant, req.ReceiptUrl);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(reportId, ct);
    }

    public async Task<ExpenseReportDto> DeleteLineAsync(Guid reportId, Guid lineId, CancellationToken ct = default)
    {
        var report = await LoadReport(reportId, ct);
        report.RemoveLine(lineId);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(reportId, ct);
    }

    // ── Workflow ──────────────────────────────────────────────────────────────

    public async Task<ExpenseReportDto> SubmitAsync(Guid reportId, string submittedBy, CancellationToken ct = default)
    {
        var report = await LoadReport(reportId, ct);
        report.Submit(submittedBy);
        await _db.SaveChangesAsync(ct);

        // Kick off workflow
        var instance = await _workflow.SubmitAsync(new SubmitForApprovalRequest(
            WorkflowDocumentType.ExpenseReport,
            reportId,
            report.ReportNumber,
            report.TotalAmount,
            submittedBy), ct);

        report.SetWorkflowInstance(instance.Id);

        // If auto-approved (no rule), update status immediately
        if (instance.Status == ApprovalStatus.Approved.ToString())
            report.SetApprovalStatus(ApprovalStatus.Approved);
        await _db.SaveChangesAsync(ct);

        return await GetReportAsync(reportId, ct);
    }

    public async Task<ExpenseReportDto> ApproveAsync(Guid reportId, string approvedBy, string? comments, CancellationToken ct = default)
    {
        var report = await _db.ExpenseReports.FirstOrDefaultAsync(r => r.Id == reportId, ct)
            ?? throw new InvalidOperationException("Report not found.");

        if (report.WorkflowInstanceId == null)
            throw new InvalidOperationException("No workflow instance associated.");

        var instance = await _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .FirstOrDefaultAsync(i => i.Id == report.WorkflowInstanceId, ct)
            ?? throw new InvalidOperationException("Workflow instance not found.");

        var pendingStep = instance.ApprovalSteps
            .OrderBy(s => s.StepOrder)
            .FirstOrDefault(s => s.Decision == StepDecision.Pending)
            ?? throw new InvalidOperationException("No pending step to approve.");

        instance.Approve(pendingStep.Id, approvedBy, comments);
        await _db.SaveChangesAsync(ct);

        if (instance.Status == ApprovalStatus.Approved)
            report.SetApprovalStatus(ApprovalStatus.Approved);
        else
            report.SetApprovalStatus(ApprovalStatus.UnderReview);

        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(reportId, ct);
    }

    public async Task<ExpenseReportDto> RejectAsync(Guid reportId, string rejectedBy, string reason, CancellationToken ct = default)
    {
        var report = await _db.ExpenseReports.FirstOrDefaultAsync(r => r.Id == reportId, ct)
            ?? throw new InvalidOperationException("Report not found.");

        if (report.WorkflowInstanceId == null)
            throw new InvalidOperationException("No workflow instance associated.");

        var instance = await _db.WorkflowInstances
            .Include(i => i.ApprovalSteps)
            .FirstOrDefaultAsync(i => i.Id == report.WorkflowInstanceId, ct)
            ?? throw new InvalidOperationException("Workflow instance not found.");

        var pendingStep = instance.ApprovalSteps
            .OrderBy(s => s.StepOrder)
            .FirstOrDefault(s => s.Decision == StepDecision.Pending)
            ?? throw new InvalidOperationException("No pending step to reject.");

        instance.Reject(pendingStep.Id, rejectedBy, reason);
        report.SetApprovalStatus(ApprovalStatus.Rejected);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(reportId, ct);
    }

    public async Task<ExpenseReportDto> MarkPaidAsync(Guid reportId, MarkPaidRequest req, CancellationToken ct = default)
    {
        var report = await LoadReport(reportId, ct);
        report.MarkPaid(req.Amount);
        await _db.SaveChangesAsync(ct);
        return await GetReportAsync(reportId, ct);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task<ExpenseReport> LoadReport(Guid id, CancellationToken ct)
    {
        return await _db.ExpenseReports
            .Include(r => r.Lines)
            .FirstOrDefaultAsync(r => r.Id == id, ct)
            ?? throw new InvalidOperationException("Expense report not found.");
    }

    private async Task<string> GenerateReportNumber(CancellationToken ct)
    {
        var count = await _db.ExpenseReports.CountAsync(ct);
        return $"EXP-{(count + 1):D4}";
    }

    private static Guid GetOrgId() => Guid.Empty;

    private static ExpenseCategoryDto ToCatDto(ExpenseCategory c) =>
        new(c.Id, c.Name, c.Description, c.GLAccountId, c.LimitPerClaim, c.IsActive);

    private static ExpenseReportSummaryDto ToSummaryDto(ExpenseReport r) =>
        new(r.Id, r.ReportNumber, r.EmployeeName, r.Department, r.Purpose,
            r.PeriodStart.ToString("yyyy-MM-dd"), r.PeriodEnd.ToString("yyyy-MM-dd"),
            r.TotalAmount, r.Status.ToString(), r.ApprovalStatus.ToString(),
            r.CreatedAt.ToString("o"));

    private static ExpenseReportDto ToDto(ExpenseReport r) =>
        new(r.Id, r.ReportNumber, r.EmployeeName, r.EmployeeEmail, r.Department,
            r.Purpose, r.PeriodStart.ToString("yyyy-MM-dd"), r.PeriodEnd.ToString("yyyy-MM-dd"),
            r.Currency, r.TotalAmount, r.ApprovedAmount, r.PaidAmount,
            r.Status.ToString(), r.ApprovalStatus.ToString(),
            r.WorkflowInstanceId, r.SubmittedBy, r.SubmittedAt?.ToString("o"),
            r.ApprovedBy, r.ApprovedAt?.ToString("o"), r.RejectedReason, r.Notes,
            r.CreatedAt.ToString("o"),
            r.Lines.Where(l => !l.IsDeleted).OrderBy(l => l.ExpenseDate)
                .Select(l => new ExpenseLineDto(l.Id, l.CategoryId, l.CategoryName,
                    l.ExpenseDate.ToString("yyyy-MM-dd"), l.Amount, l.Description,
                    l.Merchant, l.ReceiptUrl, l.IsReimbursable))
                .ToList());
}
