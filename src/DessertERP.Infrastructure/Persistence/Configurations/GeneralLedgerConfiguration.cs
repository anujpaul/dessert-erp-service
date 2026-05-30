using DessertERP.Domain.Modules.GeneralLedger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class FiscalYearConfiguration : IEntityTypeConfiguration<FiscalYear>
{
    public void Configure(EntityTypeBuilder<FiscalYear> b)
    {
        b.ToTable("fiscal_years");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(100).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.CalendarType).HasMaxLength(50);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        // Query filter is applied in AppDbContext.OnModelCreating
        b.HasMany(e => e.Periods).WithOne(p => p.FiscalYear)
            .HasForeignKey(p => p.FiscalYearId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.Name }).IsUnique();
    }
}

public class FiscalPeriodConfiguration : IEntityTypeConfiguration<FiscalPeriod>
{
    public void Configure(EntityTypeBuilder<FiscalPeriod> b)
    {
        b.ToTable("fiscal_periods");
        b.HasKey(e => e.Id);
        b.Property(e => e.Name).HasMaxLength(100).IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class AccountTypeConfiguration : IEntityTypeConfiguration<AccountType>
{
    public void Configure(EntityTypeBuilder<AccountType> b)
    {
        b.ToTable("account_types");
        b.HasKey(e => e.Id);
        b.Property(e => e.Code).HasMaxLength(20).IsRequired();
        b.Property(e => e.Name).HasMaxLength(100).IsRequired();
        b.Property(e => e.Nature).HasConversion<string>().HasMaxLength(10);
        // AccountType is shared across orgs (no org filter)
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> b)
    {
        b.ToTable("accounts");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.AccountNumber).HasMaxLength(20).IsRequired();
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Currency).HasMaxLength(3);
        // Unique per org (same number can exist in different orgs)
        b.HasIndex(e => new { e.OrganizationId, e.AccountNumber }).IsUnique();
        b.HasOne(e => e.AccountType).WithMany().HasForeignKey(e => e.AccountTypeId);
        b.HasOne(e => e.ParentAccount).WithMany().HasForeignKey(e => e.ParentAccountId);
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> b)
    {
        b.ToTable("journal_entries");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.EntryNumber).HasMaxLength(20).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.Reference).HasMaxLength(100);
        b.Property(e => e.JournalType).HasMaxLength(50);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.Currency).HasMaxLength(3);
        b.Property(e => e.TotalDebit).HasColumnType("numeric(18,4)");
        b.Property(e => e.TotalCredit).HasColumnType("numeric(18,4)");
        b.HasOne(e => e.FiscalPeriod).WithMany().HasForeignKey(e => e.FiscalPeriodId);
        b.HasMany(e => e.Lines).WithOne(l => l.JournalEntry)
            .HasForeignKey(l => l.JournalEntryId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.OrganizationId, e.EntryNumber }).IsUnique();
        // Query filter applied in AppDbContext.OnModelCreating
    }
}

public class JournalLineConfiguration : IEntityTypeConfiguration<JournalLine>
{
    public void Configure(EntityTypeBuilder<JournalLine> b)
    {
        b.ToTable("journal_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.Description).HasMaxLength(500);
        b.Property(e => e.Debit).HasColumnType("numeric(18,4)");
        b.Property(e => e.Credit).HasColumnType("numeric(18,4)");
        b.HasOne(e => e.Account).WithMany().HasForeignKey(e => e.AccountId);
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}
