using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.CashBank;
using DessertERP.Domain.Modules.FixedAssets;
using DessertERP.Domain.Modules.DataManagement;
using DessertERP.Domain.Modules.GeneralLedger;
using DessertERP.Domain.Modules.Organization;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Domain.Modules.Marketing;
using DessertERP.Domain.Modules.Retail;
using DessertERP.Domain.Modules.SystemAdmin;
using DessertERP.Domain.Modules.Workflow;
using DessertERP.Domain.Modules.Expenses;
using DessertERP.Domain.Modules.WarehouseManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Common.Interfaces;

public interface IAppDbContext
{
    // Organizations
    DbSet<Organization> Organizations { get; }

    // Workflow Engine
    DbSet<WorkflowTemplate>     WorkflowTemplates     { get; }
    DbSet<WorkflowTemplateStep> WorkflowTemplateSteps { get; }
    DbSet<WorkflowInstance>     WorkflowInstances     { get; }
    DbSet<WorkflowApprovalStep> WorkflowApprovalSteps { get; }

    // Expense Management
    DbSet<ExpenseCategory> ExpenseCategories { get; }
    DbSet<ExpenseReport>   ExpenseReports    { get; }
    DbSet<ExpenseLine>     ExpenseLines      { get; }

    // Product Management
    DbSet<Category>        Categories       { get; }
    DbSet<Brand>           Brands           { get; }
    DbSet<Domain.Modules.ProductManagement.Product> CatalogProducts { get; }
    DbSet<ProductVariant>  ProductVariants  { get; }
    DbSet<InventoryRecord>      InventoryRecords      { get; }
    DbSet<InventoryTransaction> InventoryTransactions { get; }
    DbSet<Warehouse> Warehouses { get; }
    DbSet<WarehouseLocation> WarehouseLocations { get; }
    DbSet<WarehouseInventoryBalance> WarehouseInventoryBalances { get; }

    // General Ledger
    DbSet<FiscalYear>    FiscalYears    { get; }
    DbSet<FiscalPeriod>  FiscalPeriods  { get; }
    DbSet<AccountType>   AccountTypes   { get; }
    DbSet<Account>       Accounts       { get; }
    DbSet<JournalEntry>  JournalEntries { get; }
    DbSet<JournalLine>   JournalLines   { get; }
    DbSet<Currency>      Currencies     { get; }

    // Accounts Receivable
    DbSet<Customer>        Customers         { get; }
    DbSet<CustomerAddress> CustomerAddresses { get; }
    DbSet<CustomerContact> CustomerContacts  { get; }
    DbSet<SalesOrder>      SalesOrders       { get; }
    DbSet<SalesOrderLine>  SalesOrderLines   { get; }
    DbSet<ARInvoice>       ARInvoices        { get; }
    DbSet<ARPayment>       ARPayments        { get; }

    // S2C additions
    DbSet<SalesQuotation>     SalesQuotations     { get; }
    DbSet<SalesQuotationLine> SalesQuotationLines { get; }
    DbSet<CustomerCreditNote> CustomerCreditNotes { get; }
    DbSet<DunningRecord>      DunningRecords      { get; }

    // Accounts Payable
    DbSet<Vendor>                   Vendors                   { get; }
    DbSet<VendorAddress>            VendorAddresses           { get; }
    DbSet<VendorContact>            VendorContacts            { get; }
    DbSet<PurchaseOrder>            PurchaseOrders            { get; }
    DbSet<PurchaseOrderLine>        PurchaseOrderLines        { get; }
    DbSet<PurchaseOrderReceipt>     PurchaseOrderReceipts     { get; }
    DbSet<PurchaseOrderReceiptLine> PurchaseOrderReceiptLines { get; }
    DbSet<APInvoice>                APInvoices                { get; }
    DbSet<APPayment>                APPayments                { get; }

    // P2P additions
    DbSet<PurchaseRequisition>     PurchaseRequisitions     { get; }
    DbSet<PurchaseRequisitionLine> PurchaseRequisitionLines { get; }
    DbSet<VendorCreditNote>        VendorCreditNotes        { get; }
    DbSet<PaymentProposal>         PaymentProposals         { get; }
    DbSet<PaymentProposalLine>     PaymentProposalLines     { get; }

    // System Admin
    DbSet<AppUser>        AppUsers        { get; }
    DbSet<Role>           Roles           { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<UserRole>       UserRoles       { get; }
    DbSet<AuditLogEntry>  AuditLogs       { get; }

    // Data Management
    DbSet<ImportJob>      ImportJobs      { get; }
    DbSet<ImportJobRow>   ImportJobRows   { get; }
    DbSet<ExportJobRow>   ExportJobRows   { get; }
    DbSet<BatchJobConfig> BatchJobConfigs { get; }

    // Retail
    DbSet<RetailStore>        RetailStores        { get; }
    DbSet<POSTransaction>     POSTransactions     { get; }
    DbSet<POSTransactionLine> POSTransactionLines { get; }
    DbSet<POSPayment>         POSPayments         { get; }
    DbSet<RetailStatement>    RetailStatements    { get; }
    DbSet<RetailTenderSettlement> RetailTenderSettlements { get; }
    DbSet<RetailTransactionStaging> RetailTransactionStaging { get; }
    DbSet<RetailTransactionStagingLine> RetailTransactionStagingLines { get; }
    DbSet<RetailTransactionStagingTender> RetailTransactionStagingTenders { get; }
    DbSet<Promotion>          Promotions          { get; }
    DbSet<Coupon>             Coupons             { get; }
    DbSet<CouponRedemption>   CouponRedemptions   { get; }

    // Marketing
    DbSet<Campaign>               Campaigns               { get; }
    DbSet<LoyaltyProgram>         LoyaltyPrograms         { get; }
    DbSet<CustomerLoyaltyAccount> CustomerLoyaltyAccounts { get; }

    // Trade / Price Agreements
    DbSet<PriceAgreement> PriceAgreements { get; }

    // Cash & Bank Management
    DbSet<BankAccount>       BankAccounts       { get; }
    DbSet<BankTransaction>   BankTransactions   { get; }
    DbSet<BankReconciliation> BankReconciliations { get; }
    DbSet<CashJournal>       CashJournals       { get; }
    DbSet<CashJournalLine>   CashJournalLines   { get; }

    // Fixed Assets
    DbSet<FixedAsset>        FixedAssets        { get; }
    DbSet<AssetDepreciation> AssetDepreciations { get; }
    DbSet<AssetDisposal>     AssetDisposals     { get; }
    DbSet<AssetTransfer>     AssetTransfers     { get; }
    DbSet<AssetMaintenance>  AssetMaintenances  { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
