using DessertERP.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class WorkflowTemplateConfiguration : IEntityTypeConfiguration<WorkflowTemplate>
{
    public void Configure(EntityTypeBuilder<WorkflowTemplate> b)
    {
        b.ToTable("workflow_templates");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.DocumentType).HasConversion<string>().HasMaxLength(50);
        b.Property(e => e.AmountThreshold).HasColumnType("numeric(18,2)");
        b.HasMany(e => e.Steps).WithOne(s => s.Template)
            .HasForeignKey(s => s.WorkflowTemplateId).OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class WorkflowTemplateStepConfiguration : IEntityTypeConfiguration<WorkflowTemplateStep>
{
    public void Configure(EntityTypeBuilder<WorkflowTemplateStep> b)
    {
        b.ToTable("workflow_template_steps");
        b.HasKey(e => e.Id);
        b.Property(e => e.StepName).HasMaxLength(200).IsRequired();
        b.Property(e => e.ApproverRole).HasMaxLength(100);
        b.Property(e => e.Description).HasMaxLength(500);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> b)
    {
        b.ToTable("workflow_instances");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.DocumentType).HasConversion<string>().HasMaxLength(50);
        b.Property(e => e.DocumentRef).HasMaxLength(100);
        b.Property(e => e.DocumentAmount).HasColumnType("numeric(18,2)");
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(30);
        b.Property(e => e.SubmittedBy).HasMaxLength(200);
        b.Property(e => e.RejectedReason).HasMaxLength(1000);
        b.Property(e => e.Comments).HasMaxLength(1000);
        b.HasMany(e => e.ApprovalSteps).WithOne(s => s.WorkflowInstance)
            .HasForeignKey(s => s.WorkflowInstanceId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.DocumentType, e.DocumentId });
        b.HasIndex(e => new { e.OrganizationId, e.Status });
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class WorkflowApprovalStepConfiguration : IEntityTypeConfiguration<WorkflowApprovalStep>
{
    public void Configure(EntityTypeBuilder<WorkflowApprovalStep> b)
    {
        b.ToTable("workflow_approval_steps");
        b.HasKey(e => e.Id);
        b.Property(e => e.StepName).HasMaxLength(200).IsRequired();
        b.Property(e => e.ApproverRole).HasMaxLength(100);
        b.Property(e => e.Decision).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.ActedBy).HasMaxLength(200);
        b.Property(e => e.ActedByComments).HasMaxLength(1000);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
