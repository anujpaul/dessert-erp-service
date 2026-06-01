using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.AccountsReceivable;
using DessertERP.Domain.Modules.DataManagement;
using DessertERP.Domain.Modules.GeneralLedger;
using DessertERP.Domain.Modules.Organization;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Common.Interfaces;

public interface IAppDbContext
{
    // Organizations
    DbSet<Organization> Organizations { get; }

    // Product Management
    DbSet<Category>        Categories       { get; }
    DbSet<Brand>           Brands           { get; }
    DbSet<Domain.Modules.ProductManagement.Product> CatalogProducts { get; }
    DbSet<ProductVariant>  ProductVariants  { get; }
    DbSet<InventoryRecord> InventoryRecords { get; }

    // General Ledger
    DbSet<FiscalYear>    FiscalYears    { get; }
    DbSet<FiscalPeriod>  FiscalPeriods  { get; }
    DbSet<AccountType>   AccountTypes   { get; }
    DbSet<Account>       Accounts       { get; }
    DbSet<JournalEntry>  JournalEntries { get; }
    DbSet<JournalLine>   JournalLines   { get; }

    // Accounts Receivable
    DbSet<Customer>       Customers       { get; }
    DbSet<SalesOrder>     SalesOrders     { get; }
    DbSet<SalesOrderLine> SalesOrderLines { get; }
    DbSet<ARInvoice>      ARInvoices      { get; }
    DbSet<ARPayment>      ARPayments      { get; }

    // Accounts Payable
    DbSet<Vendor>           Vendors           { get; }
    DbSet<PurchaseOrder>    PurchaseOrders    { get; }
    DbSet<PurchaseOrderLine> PurchaseOrderLines { get; }
    DbSet<APInvoice>        APInvoices        { get; }
    DbSet<APPayment>        APPayments        { get; }

    // System Admin
    DbSet<AppUser>       AppUsers   { get; }
    DbSet<Role>          Roles      { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<UserRole>      UserRoles  { get; }
    DbSet<AuditLogEntry> AuditLogs  { get; }

    // Data Management
    DbSet<ImportJob>      ImportJobs      { get; }
    DbSet<ImportJobRow>   ImportJobRows   { get; }
    DbSet<ExportJobRow>   ExportJobRows   { get; }
    DbSet<BatchJobConfig> BatchJobConfigs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
