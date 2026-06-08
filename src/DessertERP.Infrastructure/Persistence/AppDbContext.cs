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
using DessertERP.Domain.Modules.Workflow;
using DessertERP.Domain.Modules.Expenses;
using DessertERP.Domain.Modules.CashBank;
using DessertERP.Domain.Modules.FixedAssets;
using DessertERP.Domain.Modules.WarehouseManagement;
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

    // Workflow Engine
    public DbSet<WorkflowTemplate>     WorkflowTemplates     => Set<WorkflowTemplate>();
    public DbSet<WorkflowTemplateStep> WorkflowTemplateSteps => Set<WorkflowTemplateStep>();
    public DbSet<WorkflowInstance>     WorkflowInstances     => Set<WorkflowInstance>();
    public DbSet<WorkflowApprovalStep> WorkflowApprovalSteps => Set<WorkflowApprovalStep>();

    // Expense Management
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<ExpenseReport>   ExpenseReports    => Set<ExpenseReport>();
    public DbSet<ExpenseLine>     ExpenseLines      => Set<ExpenseLine>();

    // Product Management
    public DbSet<Category>        Categories       => Set<Category>();
    public DbSet<Brand>           Brands           => Set<Brand>();
    public DbSet<PMProduct>       CatalogProducts  => Set<PMProduct>();
    public DbSet<ProductVariant>  ProductVariants  => Set<ProductVariant>();
    public DbSet<InventoryRecord>      InventoryRecords      => Set<InventoryRecord>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    // General Ledger
    public DbSet<FiscalYear>   FiscalYears   => Set<FiscalYear>();
    public DbSet<FiscalPeriod> FiscalPeriods => Set<FiscalPeriod>();
    public DbSet<AccountType>  AccountTypes  => Set<AccountType>();
    public DbSet<Account>      Accounts      => Set<Account>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine>  JournalLines  => Set<JournalLine>();
    public DbSet<Currency>     Currencies    => Set<Currency>();

    // Accounts Receivable
    public DbSet<Customer>           Customers           => Set<Customer>();
    public DbSet<CustomerAddress>    CustomerAddresses   => Set<CustomerAddress>();
    public DbSet<CustomerContact>    CustomerContacts    => Set<CustomerContact>();
    public DbSet<SalesOrder>         SalesOrders         => Set<SalesOrder>();
    public DbSet<SalesOrderLine>     SalesOrderLines     => Set<SalesOrderLine>();
    public DbSet<ARInvoice>          ARInvoices          => Set<ARInvoice>();
    public DbSet<ARPayment>          ARPayments          => Set<ARPayment>();
    // S2C additions
    public DbSet<SalesQuotation>     SalesQuotations     => Set<SalesQuotation>();
    public DbSet<SalesQuotationLine> SalesQuotationLines => Set<SalesQuotationLine>();
    public DbSet<CustomerCreditNote> CustomerCreditNotes => Set<CustomerCreditNote>();
    public DbSet<DunningRecord>      DunningRecords      => Set<DunningRecord>();

    // Accounts Payable
    public DbSet<Vendor>                    Vendors                   => Set<Vendor>();
    public DbSet<VendorAddress>             VendorAddresses           => Set<VendorAddress>();
    public DbSet<VendorContact>             VendorContacts            => Set<VendorContact>();
    public DbSet<PurchaseRequisition>       PurchaseRequisitions      => Set<PurchaseRequisition>();
    public DbSet<PurchaseRequisitionLine>   PurchaseRequisitionLines  => Set<PurchaseRequisitionLine>();
    public DbSet<PurchaseOrder>             PurchaseOrders            => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine>         PurchaseOrderLines        => Set<PurchaseOrderLine>();
    public DbSet<PurchaseOrderReceipt>      PurchaseOrderReceipts     => Set<PurchaseOrderReceipt>();
    public DbSet<PurchaseOrderReceiptLine>  PurchaseOrderReceiptLines => Set<PurchaseOrderReceiptLine>();
    public DbSet<APInvoice>                 APInvoices                => Set<APInvoice>();
    public DbSet<APPayment>                 APPayments                => Set<APPayment>();
    public DbSet<VendorCreditNote>          VendorCreditNotes         => Set<VendorCreditNote>();
    public DbSet<PaymentProposal>           PaymentProposals          => Set<PaymentProposal>();
    public DbSet<PaymentProposalLine>       PaymentProposalLines      => Set<PaymentProposalLine>();

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

    // Cash & Bank Management
    public DbSet<BankAccount>        BankAccounts        => Set<BankAccount>();
    public DbSet<BankTransaction>    BankTransactions    => Set<BankTransaction>();
    public DbSet<BankReconciliation> BankReconciliations => Set<BankReconciliation>();
    public DbSet<CashJournal>        CashJournals        => Set<CashJournal>();
    public DbSet<CashJournalLine>    CashJournalLines    => Set<CashJournalLine>();

    // Fixed Assets
    public DbSet<FixedAsset>        FixedAssets        => Set<FixedAsset>();
    public DbSet<AssetDepreciation> AssetDepreciations => Set<AssetDepreciation>();
    public DbSet<AssetDisposal>     AssetDisposals     => Set<AssetDisposal>();
    public DbSet<AssetTransfer>     AssetTransfers     => Set<AssetTransfer>();
    public DbSet<AssetMaintenance>  AssetMaintenances  => Set<AssetMaintenance>();

    // Warehouse Management
    public DbSet<Warehouse>         Warehouses         => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    public DbSet<InboundOrder>      InboundOrders      => Set<InboundOrder>();
    public DbSet<InboundOrderLine>  InboundOrderLines  => Set<InboundOrderLine>();
    public DbSet<OutboundOrder>     OutboundOrders     => Set<OutboundOrder>();
    public DbSet<OutboundOrderLine> OutboundOrderLines => Set<OutboundOrderLine>();
    public DbSet<TransferOrder>     TransferOrders     => Set<TransferOrder>();
    public DbSet<TransferOrderLine> TransferOrderLines => Set<TransferOrderLine>();

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
        modelBuilder.Entity<InventoryTransaction>()
            .HasQueryFilter(e => (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // GL
        modelBuilder.Entity<FiscalYear>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<Account>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<JournalEntry>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<Currency>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // AR
        modelBuilder.Entity<Customer>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<CustomerAddress>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<CustomerContact>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<SalesOrder>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ARInvoice>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ARPayment>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<SalesQuotation>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<SalesQuotationLine>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CustomerCreditNote>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<DunningRecord>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));

        // AP
        modelBuilder.Entity<Vendor>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<VendorAddress>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<VendorContact>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<PurchaseRequisition>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<PurchaseOrder>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<APInvoice>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<APPayment>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<VendorCreditNote>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<PaymentProposal>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);

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

        // Workflow Engine — org-scoped, soft-delete
        modelBuilder.Entity<WorkflowTemplate>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<WorkflowTemplateStep>()
            .HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<WorkflowInstance>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<WorkflowApprovalStep>()
            .HasQueryFilter(e => !e.IsDeleted);

        // Expense Management — org-scoped, soft-delete
        modelBuilder.Entity<ExpenseCategory>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ExpenseReport>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<ExpenseLine>()
            .HasQueryFilter(e => !e.IsDeleted);

        // Warehouse Management — org-scoped (no soft-delete on warehouse entities)
        modelBuilder.Entity<Warehouse>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<InboundOrder>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<OutboundOrder>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);
        modelBuilder.Entity<TransferOrder>()
            .HasQueryFilter(e => _orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId);

        // Cash & Bank Management — org-scoped, soft-delete
        modelBuilder.Entity<BankAccount>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<BankTransaction>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<BankReconciliation>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<CashJournal>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<CashJournalLine>()
            .HasQueryFilter(e => !e.IsDeleted);

        // Fixed Assets — org-scoped (no soft-delete, disposals are status-based)
        modelBuilder.Entity<FixedAsset>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<AssetDisposal>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<AssetTransfer>()
            .HasQueryFilter(e => !e.IsDeleted && (_orgService == null || _orgService.OrganizationId == Guid.Empty || e.OrganizationId == _orgService.OrganizationId));
        modelBuilder.Entity<AssetMaintenance>()
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
