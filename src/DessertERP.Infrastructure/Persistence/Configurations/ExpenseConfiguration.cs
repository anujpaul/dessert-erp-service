using DessertERP.Domain.Modules.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> b)
    {
        b.ToTable("expense_categories");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.LimitPerClaim).HasColumnType("numeric(18,2)");
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class ExpenseReportConfiguration : IEntityTypeConfiguration<ExpenseReport>
{
    public void Configure(EntityTypeBuilder<ExpenseReport> b)
    {
        b.ToTable("expense_reports");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.ReportNumber).HasMaxLength(50).IsRequired();
        b.Property(e => e.EmployeeName).HasMaxLength(200).IsRequired();
        b.Property(e => e.EmployeeEmail).HasMaxLength(200);
        b.Property(e => e.Department).HasMaxLength(100);
        b.Property(e => e.Purpose).HasMaxLength(500).IsRequired();
        b.Property(e => e.Currency).HasMaxLength(10);
        b.Property(e => e.TotalAmount).HasColumnType("numeric(18,2)");
        b.Property(e => e.ApprovedAmount).HasColumnType("numeric(18,2)");
        b.Property(e => e.PaidAmount).HasColumnType("numeric(18,2)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.ApprovalStatus).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.SubmittedBy).HasMaxLength(200);
        b.Property(e => e.ApprovedBy).HasMaxLength(200);
        b.Property(e => e.RejectedReason).HasMaxLength(1000);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.HasMany(e => e.Lines).WithOne(l => l.ExpenseReport)
            .HasForeignKey(l => l.ExpenseReportId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.ReportNumber }).IsUnique();
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class ExpenseLineConfiguration : IEntityTypeConfiguration<ExpenseLine>
{
    public void Configure(EntityTypeBuilder<ExpenseLine> b)
    {
        b.ToTable("expense_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.CategoryName).HasMaxLength(200);
        b.Property(e => e.Amount).HasColumnType("numeric(18,2)").IsRequired();
        b.Property(e => e.Description).HasMaxLength(500).IsRequired();
        b.Property(e => e.Merchant).HasMaxLength(200);
        b.Property(e => e.ReceiptUrl).HasMaxLength(1000);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
