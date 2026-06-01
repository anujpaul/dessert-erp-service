using DessertERP.Domain.Modules.DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class ImportJobConfiguration : IEntityTypeConfiguration<ImportJob>
{
    public void Configure(EntityTypeBuilder<ImportJob> b)
    {
        b.ToTable("import_jobs");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.EntityType).HasConversion<string>().HasMaxLength(30).IsRequired();
        b.Property(e => e.FileFormat).HasConversion<string>().HasMaxLength(10).IsRequired();
        b.Property(e => e.FileName).HasMaxLength(260).IsRequired();
        b.Property(e => e.FilePath).HasMaxLength(500).IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.ErrorSummary).HasMaxLength(4000);
        b.Property(e => e.TriggeredBy).HasMaxLength(50);
        b.HasIndex(e => e.OrganizationId);
        b.HasIndex(e => e.Status);
    }
}

public class BatchJobConfigConfiguration : IEntityTypeConfiguration<BatchJobConfig>
{
    public void Configure(EntityTypeBuilder<BatchJobConfig> b)
    {
        b.ToTable("batch_job_configs");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(100).IsRequired();
        b.Property(e => e.JobType).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.Property(e => e.CronExpression).HasMaxLength(100).IsRequired();
        b.Property(e => e.LocalInboxPath).HasMaxLength(500).IsRequired();
        b.Property(e => e.LocalProcessedPath).HasMaxLength(500).IsRequired();
        b.Property(e => e.LocalErrorPath).HasMaxLength(500).IsRequired();
        b.Property(e => e.LocalExportPath).HasMaxLength(500);
        b.Property(e => e.FileFormat).HasMaxLength(10).IsRequired();
        b.Property(e => e.ExportFileNamePattern).HasMaxLength(200);
        b.Property(e => e.LastRunStatus).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.LastRunMessage).HasMaxLength(2000);
        b.HasIndex(e => e.OrganizationId);
        b.HasIndex(e => new { e.OrganizationId, e.IsEnabled });
    }
}

public class ExportJobRowConfiguration : IEntityTypeConfiguration<ExportJobRow>
{
    public void Configure(EntityTypeBuilder<ExportJobRow> b)
    {
        b.ToTable("export_job_rows");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.BatchJobConfigId).IsRequired();
        b.Property(e => e.EntityType).HasMaxLength(50).IsRequired();
        b.Property(e => e.EntityId).IsRequired();
        b.Property(e => e.EntityRef).HasMaxLength(100).IsRequired();
        b.Property(e => e.BlobName).HasMaxLength(500).IsRequired(); // stores local file path after migration to local FS
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.ErrorMessage).HasMaxLength(2000);
        b.HasIndex(e => e.OrganizationId);
        b.HasIndex(e => new { e.BatchJobConfigId, e.ExportedAt });
        b.HasIndex(e => new { e.EntityType, e.EntityId });
    }
}

public class ImportJobRowConfiguration : IEntityTypeConfiguration<ImportJobRow>
{
    public void Configure(EntityTypeBuilder<ImportJobRow> b)
    {
        b.ToTable("import_job_rows");
        b.HasKey(e => e.Id);
        b.Property(e => e.ImportJobId).IsRequired();
        b.Property(e => e.RowNumber).IsRequired();
        b.Property(e => e.RawPayload).IsRequired();           // stored as TEXT/JSON — no max length
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.ErrorMessage).HasMaxLength(2000);
        b.Property(e => e.PromotedEntityId);

        b.HasOne(e => e.ImportJob)
         .WithMany()
         .HasForeignKey(e => e.ImportJobId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(e => e.ImportJobId);
        b.HasIndex(e => new { e.ImportJobId, e.Status });
    }
}
