using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.DataManagement;
using DessertERP.Domain.Modules.GeneralLedger;
using DessertERP.Domain.Modules.Organization;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Domain.Modules.Marketing;
using DessertERP.Domain.Modules.Retail;
using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.EntityFrameworkCore;
using PMProduct = DessertERP.Domain.Modules.ProductManagement.Product;

namespace DessertERP.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly ICurrentOrganizationService? _orgService;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        ICurrentOrganizationService? orgService = null)
        : base(options)
    {
        _orgService = orgService;
    }

    // Organizations
    public DbSet<Organization> Organizations => Set<Organization>();

    // Product Management
    public DbSet<Category>        Categories       => Set<Category>();
    public DbSet<Brand>           Brands           => Set<Brand>();
    public DbSet<PMProduct>       CatalogProducts  => Set<PMProduct>();
    public DbSet<ProductVariant>  ProductVariants  => Set<ProductVariant>();
    public DbSet<InventoryRecord> InventoryRecords => Set<InventoryRecord>();

    // General Ledger
    public DbSet<FiscalYear>   FiscalYears   => Set<FiscalYear>();
    public DbSet<FiscalPeriod> FiscalPeriods => Set<FiscalPeriod>();
    public DbSet<AccountType>  AccountTypes  => Set<AccountType>();
    public DbSet<Account>      Accounts      => Set<Account>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine>  JournalLines  => Set<JournalLine>();

    // Accounts Receivable
    public DbSet<Customer>       Customers       => Set<Customer>();
    public DbSet<SalesOrder>     SalesOrders     => Set<SalesOrder>();
    public DbSet<SalesOrderLine> SalesOrderLines => Set<SalesOrderLine>();
    public DbSet<ARInvoice>      ARInvoices      => Set<ARInvoice>();
    public DbSet<ARPayment>      ARPayments      => Set<ARPayment>();

    // Accounts Payable
    public DbSet<Vendor>                    Vendors                   => Set<Vendor>();
    public DbSet<PurchaseOrder>             PurchaseOrders            => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine>         PurchaseOrderLines        => Set<PurchaseOrderLine>();
    public DbSet<PurchaseOrderReceipt>      PurchaseOrderReceipts     => Set<PurchaseOrderReceipt>();
    public DbSet<PurchaseOrderReceiptLine>  PurchaseOrderReceiptLines => Set<PurchaseOrderReceiptLine>();
    public DbSet<APInvoice>                 APInvoices                => Set<APInvoice>();
    public DbSet<APPayment>                 APPayments                => Set<APPayment>();

    // Retail
    public DbSet<RetailStore>        RetailStores        => Set<RetailStore>();
    public DbSet<POSTransaction>     POSTransactions     => Set<POSTransaction>();
    public DbSet<POSTransactionLine> POSTransactionLines => Set<POSTransactionLine>();
    public DbSet<POSPayment>         POSPayments         => Set<POSPayment>();
    public DbSet<Promotion>          Promotions          => Set<Promotion>();
    public DbSet<Coupon>             Coupons             => Set<Coupon>();
    public DbSet<CouponRedemption>   CouponRedemptions   => Set<CouponRedemption>();

    // Marketing
    public DbSet<Campaign>                Campaigns               => Set<Campaign>();
    public DbSet<LoyaltyProgram>          LoyaltyPrograms         => Set<LoyaltyProgram>();
    public DbSet<CustomerLoyaltyAccount>  CustomerLoyaltyAccounts => Set<CustomerLoyaltyAccount>();

    // Trade / Price Agreements
    public DbSet<PriceAgreement> PriceAgreements => Set<PriceAgreement>();

    // Data Management
    public DbSet<ImportJob>      ImportJobs      => Set<ImportJob>();
    public DbSet<ImportJobRow>   ImportJobRows   => Set<ImportJobRow>();
    public DbSet<ExportJobRow>   ExportJobRows   => Set<ExportJobRow>();
    public DbSet<BatchJobConfig> BatchJobConfigs => Set<BatchJobConfig>();

    // System Admin
    public DbSet<AppUser>        AppUsers        => Set<AppUser>();
    public DbSet<Role>           Roles           => Set<Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole>       UserRoles       => Set<UserRole>();
    public DbSet<AuditLogEntry>  AuditLogs       => Set<AuditLogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Exclude the obsolete AR Product tombstone — it has no table
        //modelBuilder.Ignore<DessertERP.Domain.Modules.AccountsReceivable.Product>();

        // ── Organization-scoped global filters ────────────────────────────────
        // IMPORTANT: Reference _orgService inside each lambda — NOT a captured local Guid.
        // EF Core evaluates these per-query using the current DbContext instance,
        // so _orgService.OrganizationId is resolved fresh on every request.
        // Capturing a local Guid value would bake it in at model-build time (wrong).

        // Organizations — soft delete only (no org scope — cross-org list is needed)
        modelBuilder.Entity<Organization>()
            .HasQueryFilter(e => !e.IsDeleted);

        // Product Management
        modelBuilder.Entity<Category>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<Brand>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<PMProduct>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ProductVariant>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<InventoryRecord>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // GL
        modelBuilder.Entity<FiscalYear>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<Account>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<JournalEntry>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // AR
        modelBuilder.Entity<Customer>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<SalesOrder>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ARInvoice>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ARPayment>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // AP
        modelBuilder.Entity<Vendor>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<PurchaseOrder>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<APInvoice>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<APPayment>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // Data Management — org-scoped, soft-delete
        modelBuilder.Entity<ImportJob>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        // ImportJobRow: soft-delete only — org filter is enforced via the parent ImportJob
        modelBuilder.Entity<ImportJobRow>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BatchJobConfig>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // Retail — org-scoped, soft-delete
        modelBuilder.Entity<RetailStore>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<POSTransaction>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<POSTransactionLine>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<POSPayment>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Promotion>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<Coupon>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<CouponRedemption>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // Marketing — org-scoped, no soft-delete on domain entities (use IsActive)
        modelBuilder.Entity<Campaign>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<LoyaltyProgram>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<CustomerLoyaltyAccount>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);

        // Price Agreements — org-scoped, no soft-delete
        modelBuilder.Entity<PriceAgreement>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);

        // System Admin — org-scoped, soft-delete
        modelBuilder.Entity<AppUser>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<Role>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified))
        {
            if (entry.Entity is Domain.Common.BaseEntity entity)
                entity.SetUpdated();
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
