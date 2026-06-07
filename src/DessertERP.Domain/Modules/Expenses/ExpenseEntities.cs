using DessertERP.Domain.Common;
using DessertERP.Domain.Modules.Workflow;

namespace DessertERP.Domain.Modules.Expenses;

public enum ExpenseReportStatus
{
    Draft,
    Submitted,
    UnderReview,
    Approved,
    Rejected,
    Paid,
}

// ─────────────────────────────────────────────────────────────────────────────
// ExpenseCategory
// ─────────────────────────────────────────────────────────────────────────────

public class ExpenseCategory : BaseEntity
{
    public Guid   OrganizationId { get; private set; }
    public string Name           { get; private set; } = string.Empty;
    public string? Description   { get; private set; }
    /// <summary>GL account to debit when expense is approved (optional).</summary>
    public Guid?  GLAccountId    { get; private set; }
    /// <summary>Per-claim limit. Null = no limit.</summary>
    public decimal? LimitPerClaim { get; private set; }
    public bool   IsActive       { get; private set; } = true;

    private ExpenseCategory() { }

    public ExpenseCategory(Guid organizationId, string name,
        string? description = null, Guid? glAccountId = null, decimal? limitPerClaim = null)
    {
        OrganizationId = organizationId;
        Name           = name.Trim();
        Description    = description?.Trim();
        GLAccountId    = glAccountId;
        LimitPerClaim  = limitPerClaim;
    }

    public void Update(string name, string? description, Guid? glAccountId,
        decimal? limitPerClaim, bool isActive)
    {
        Name          = name.Trim();
        Description   = description?.Trim();
        GLAccountId   = glAccountId;
        LimitPerClaim = limitPerClaim;
        IsActive      = isActive;
        SetUpdated();
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// ExpenseReport  (header)
// ─────────────────────────────────────────────────────────────────────────────

public class ExpenseReport : BaseEntity
{
    public Guid   OrganizationId   { get; private set; }
    public string ReportNumber     { get; private set; } = string.Empty;

    /// <summary>Employee name / user who is claiming.</summary>
    public string EmployeeName     { get; private set; } = string.Empty;
    public string? EmployeeEmail   { get; private set; }
    public string? Department      { get; private set; }

    /// <summary>Purpose of the trip / event this report covers.</summary>
    public string Purpose          { get; private set; } = string.Empty;
    public DateTime PeriodStart    { get; private set; }
    public DateTime PeriodEnd      { get; private set; }

    public string Currency         { get; private set; } = "USD";
    public decimal TotalAmount     { get; private set; }
    public decimal ApprovedAmount  { get; private set; }  // may differ if some lines rejected
    public decimal PaidAmount      { get; private set; }

    public ExpenseReportStatus Status { get; private set; } = ExpenseReportStatus.Draft;
    public ApprovalStatus ApprovalStatus { get; private set; } = ApprovalStatus.Draft;
    public Guid? WorkflowInstanceId { get; private set; }

    public string? SubmittedBy     { get; private set; }
    public DateTime? SubmittedAt   { get; private set; }
    public string? ApprovedBy      { get; private set; }
    public DateTime? ApprovedAt    { get; private set; }
    public string? RejectedReason  { get; private set; }
    public string? Notes           { get; private set; }

    private readonly List<ExpenseLine> _lines = new();
    public IReadOnlyCollection<ExpenseLine> Lines => _lines.AsReadOnly();

    private ExpenseReport() { }

    public ExpenseReport(Guid organizationId, string reportNumber, string employeeName,
        string purpose, DateTime periodStart, DateTime periodEnd,
        string? employeeEmail = null, string? department = null, string currency = "USD")
    {
        OrganizationId = organizationId;
        ReportNumber   = reportNumber;
        EmployeeName   = employeeName.Trim();
        EmployeeEmail  = employeeEmail?.Trim();
        Department     = department?.Trim();
        Purpose        = purpose.Trim();
        PeriodStart    = periodStart;
        PeriodEnd      = periodEnd;
        Currency       = currency;
    }

    public ExpenseLine AddLine(Guid categoryId, string categoryName, DateTime expenseDate,
        decimal amount, string description, string? merchant = null, string? receiptUrl = null)
    {
        if (Status != ExpenseReportStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to Draft reports.");
        var line = new ExpenseLine(Id, categoryId, categoryName, expenseDate, amount,
            description, merchant, receiptUrl);
        _lines.Add(line);
        Recalculate();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        if (Status != ExpenseReportStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from Draft reports.");
        line.SoftDelete();
        Recalculate();
    }

    public void Submit(string submittedBy)
    {
        if (Status != ExpenseReportStatus.Draft)
            throw new InvalidOperationException("Only Draft reports can be submitted.");
        if (!_lines.Any(l => !l.IsDeleted))
            throw new InvalidOperationException("Cannot submit a report with no expense lines.");
        Status          = ExpenseReportStatus.Submitted;
        ApprovalStatus  = ApprovalStatus.Submitted;
        SubmittedBy     = submittedBy;
        SubmittedAt     = DateTime.UtcNow;
        SetUpdated();
    }

    public void SetWorkflowInstance(Guid instanceId)
    {
        WorkflowInstanceId = instanceId;
        SetUpdated();
    }

    public void SetApprovalStatus(ApprovalStatus status)
    {
        ApprovalStatus = status;
        if (status == ApprovalStatus.Approved)
        {
            Status         = ExpenseReportStatus.Approved;
            ApprovedAmount = TotalAmount;
            ApprovedAt     = DateTime.UtcNow;
        }
        else if (status == ApprovalStatus.Rejected)
        {
            Status = ExpenseReportStatus.Rejected;
        }
        SetUpdated();
    }

    public void MarkPaid(decimal amount)
    {
        if (Status != ExpenseReportStatus.Approved)
            throw new InvalidOperationException("Only Approved reports can be marked as paid.");
        PaidAmount = amount;
        Status     = ExpenseReportStatus.Paid;
        SetUpdated();
    }

    public void UpdateHeader(string purpose, DateTime periodStart, DateTime periodEnd,
        string? department, string? notes)
    {
        if (Status != ExpenseReportStatus.Draft)
            throw new InvalidOperationException("Only Draft reports can be edited.");
        Purpose     = purpose.Trim();
        PeriodStart = periodStart;
        PeriodEnd   = periodEnd;
        Department  = department?.Trim();
        Notes       = notes?.Trim();
        SetUpdated();
    }

    private void Recalculate()
    {
        TotalAmount = _lines.Where(l => !l.IsDeleted).Sum(l => l.Amount);
        SetUpdated();
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// ExpenseLine  (one receipt / line item)
// ─────────────────────────────────────────────────────────────────────────────

public class ExpenseLine : BaseEntity
{
    public Guid   ExpenseReportId { get; private set; }
    public Guid   CategoryId      { get; private set; }
    public string CategoryName    { get; private set; } = string.Empty; // denormalised
    public DateTime ExpenseDate   { get; private set; }
    public decimal Amount         { get; private set; }
    public string Description     { get; private set; } = string.Empty;
    public string? Merchant       { get; private set; }
    public string? ReceiptUrl     { get; private set; }  // uploaded receipt link
    public bool   IsReimbursable  { get; private set; } = true;

    public ExpenseReport? ExpenseReport { get; private set; }

    private ExpenseLine() { }

    public ExpenseLine(Guid reportId, Guid categoryId, string categoryName,
        DateTime expenseDate, decimal amount, string description,
        string? merchant = null, string? receiptUrl = null)
    {
        ExpenseReportId = reportId;
        CategoryId      = categoryId;
        CategoryName    = categoryName.Trim();
        ExpenseDate     = expenseDate;
        Amount          = amount;
        Description     = description.Trim();
        Merchant        = merchant?.Trim();
        ReceiptUrl      = receiptUrl?.Trim();
    }

    public void Update(Guid categoryId, string categoryName, DateTime expenseDate,
        decimal amount, string description, string? merchant, string? receiptUrl)
    {
        CategoryId   = categoryId;
        CategoryName = categoryName.Trim();
        ExpenseDate  = expenseDate;
        Amount       = amount;
        Description  = description.Trim();
        Merchant     = merchant?.Trim();
        ReceiptUrl   = receiptUrl?.Trim();
        SetUpdated();
    }
}
