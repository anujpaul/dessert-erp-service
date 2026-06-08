using DessertERP.Domain.Modules.CashBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> b)
    {
        b.ToTable("bank_accounts");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.AccountCode).HasMaxLength(30).IsRequired();
        b.Property(e => e.AccountName).HasMaxLength(150).IsRequired();
        b.Property(e => e.AccountType).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.AccountStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.Currency).HasMaxLength(3).IsRequired();
        b.Property(e => e.BankName).HasMaxLength(150);
        b.Property(e => e.BankBranch).HasMaxLength(100);
        b.Property(e => e.RoutingNumber).HasMaxLength(20);
        b.Property(e => e.AccountNumber).HasMaxLength(50);
        b.Property(e => e.IBAN).HasMaxLength(34);
        b.Property(e => e.SwiftCode).HasMaxLength(11);
        b.Property(e => e.CurrentBalance).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.LastReconciledBalance).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.Notes).HasMaxLength(500);
        b.HasIndex(e => new { e.OrganizationId, e.AccountCode }).IsUnique();
    }
}

public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
{
    public void Configure(EntityTypeBuilder<BankTransaction> b)
    {
        b.ToTable("bank_transactions");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.BankAccountId).IsRequired();
        b.Property(e => e.TransactionNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.TransactionType).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.TransactionStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.Amount).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.Description).HasMaxLength(300).IsRequired();
        b.Property(e => e.Reference).HasMaxLength(100);
        b.Property(e => e.CounterpartyName).HasMaxLength(150);
        b.Property(e => e.PostedBy).HasMaxLength(100);
        b.Property(e => e.Notes).HasMaxLength(500);

        b.HasOne(e => e.BankAccount)
         .WithMany()
         .HasForeignKey(e => e.BankAccountId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(e => new { e.OrganizationId, e.TransactionNumber }).IsUnique();
        b.HasIndex(e => new { e.BankAccountId, e.TransactionDate });
        b.HasIndex(e => e.TransactionStatus);
    }
}

public class BankReconciliationConfiguration : IEntityTypeConfiguration<BankReconciliation>
{
    public void Configure(EntityTypeBuilder<BankReconciliation> b)
    {
        b.ToTable("bank_reconciliations");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.BankAccountId).IsRequired();
        b.Property(e => e.ReconciliationNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.StatementOpeningBalance).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.StatementClosingBalance).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.SystemOpeningBalance).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.ReconciledAmount).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.Notes).HasMaxLength(500);
        b.Property(e => e.CompletedBy).HasMaxLength(100);
        // Difference is computed — not mapped
        b.Ignore(e => e.Difference);

        b.HasOne(e => e.BankAccount)
         .WithMany()
         .HasForeignKey(e => e.BankAccountId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(e => new { e.OrganizationId, e.ReconciliationNumber }).IsUnique();
    }
}

public class CashJournalConfiguration : IEntityTypeConfiguration<CashJournal>
{
    public void Configure(EntityTypeBuilder<CashJournal> b)
    {
        b.ToTable("cash_journals");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.BankAccountId).IsRequired();
        b.Property(e => e.JournalNumber).HasMaxLength(30).IsRequired();
        b.Property(e => e.Description).HasMaxLength(300).IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(e => e.TotalDebits).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.TotalCredits).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.PostedBy).HasMaxLength(100);
        b.Property(e => e.Notes).HasMaxLength(500);

        b.HasOne(e => e.BankAccount)
         .WithMany()
         .HasForeignKey(e => e.BankAccountId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(e => new { e.OrganizationId, e.JournalNumber }).IsUnique();
        b.HasIndex(e => e.Status);
    }
}

public class CashJournalLineConfiguration : IEntityTypeConfiguration<CashJournalLine>
{
    public void Configure(EntityTypeBuilder<CashJournalLine> b)
    {
        b.ToTable("cash_journal_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.JournalId).IsRequired();
        b.Property(e => e.GLAccountId).IsRequired();
        b.Property(e => e.Description).HasMaxLength(300).IsRequired();
        b.Property(e => e.Debit).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.Credit).HasColumnType("numeric(18,4)").IsRequired();
        b.Property(e => e.Reference).HasMaxLength(100);

        b.HasOne(e => e.Journal)
         .WithMany(j => j.Lines)
         .HasForeignKey(e => e.JournalId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
