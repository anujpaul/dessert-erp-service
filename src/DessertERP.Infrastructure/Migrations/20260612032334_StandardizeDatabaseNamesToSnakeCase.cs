using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StandardizeDatabaseNamesToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_account_types_AccountTypeId",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_accounts_accounts_ParentAccountId",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ap_invoices_ap_invoices_LinkedPrepaymentInvoiceId",
                table: "ap_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ap_invoices_purchase_orders_PurchaseOrderId",
                table: "ap_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ap_invoices_vendors_VendorId",
                table: "ap_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ap_payments_ap_invoices_APInvoiceId",
                table: "ap_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ap_payments_vendors_VendorId",
                table: "ap_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ar_invoices_customers_CustomerId",
                table: "ar_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ar_invoices_sales_orders_SalesOrderId",
                table: "ar_invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ar_payments_ar_invoices_ARInvoiceId",
                table: "ar_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ar_payments_customers_CustomerId",
                table: "ar_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_depreciations_fixed_assets_AssetId",
                table: "asset_depreciations");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_disposals_fixed_assets_AssetId",
                table: "asset_disposals");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_maintenances_fixed_assets_AssetId",
                table: "asset_maintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_asset_transfers_fixed_assets_AssetId",
                table: "asset_transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_bank_reconciliations_bank_accounts_BankAccountId",
                table: "bank_reconciliations");

            migrationBuilder.DropForeignKey(
                name: "FK_bank_transactions_bank_accounts_BankAccountId",
                table: "bank_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_cash_journal_lines_cash_journals_JournalId",
                table: "cash_journal_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_cash_journals_bank_accounts_BankAccountId",
                table: "cash_journals");

            migrationBuilder.DropForeignKey(
                name: "FK_catalog_products_brands_BrandId",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "FK_catalog_products_categories_CategoryId",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "FK_catalog_products_vendors_PreferredVendorId",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_ParentCategoryId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_coupons_promotions_PromotionId",
                table: "coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_addresses_customers_CustomerId",
                table: "customer_addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_contacts_customers_CustomerId",
                table: "customer_contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_credit_notes_ar_invoices_ARInvoiceId",
                table: "customer_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_credit_notes_customers_CustomerId",
                table: "customer_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerLoyaltyAccounts_LoyaltyPrograms_LoyaltyProgramId",
                table: "CustomerLoyaltyAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_dunning_records_ar_invoices_ARInvoiceId",
                table: "dunning_records");

            migrationBuilder.DropForeignKey(
                name: "FK_dunning_records_customers_CustomerId",
                table: "dunning_records");

            migrationBuilder.DropForeignKey(
                name: "FK_expense_lines_expense_reports_ExpenseReportId",
                table: "expense_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_fiscal_periods_fiscal_years_FiscalYearId",
                table: "fiscal_periods");

            migrationBuilder.DropForeignKey(
                name: "FK_import_job_rows_import_jobs_ImportJobId",
                table: "import_job_rows");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundOrderLines_InboundOrders_InboundOrderId",
                table: "InboundOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundOrderLines_WarehouseLocations_LocationId",
                table: "InboundOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundOrders_Warehouses_WarehouseId",
                table: "InboundOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_records_product_variants_ProductVariantId",
                table: "inventory_records");

            migrationBuilder.DropForeignKey(
                name: "FK_inventory_transactions_product_variants_ProductVariantId",
                table: "inventory_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_journal_entries_fiscal_periods_FiscalPeriodId",
                table: "journal_entries");

            migrationBuilder.DropForeignKey(
                name: "FK_journal_lines_accounts_AccountId",
                table: "journal_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_journal_lines_journal_entries_JournalEntryId",
                table: "journal_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrderLines_OutboundOrders_OutboundOrderId",
                table: "OutboundOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrderLines_WarehouseLocations_FromLocationId",
                table: "OutboundOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboundOrders_Warehouses_WarehouseId",
                table: "OutboundOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_proposal_lines_ap_invoices_APInvoiceId",
                table: "payment_proposal_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_proposal_lines_ap_payments_APPaymentId",
                table: "payment_proposal_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_payment_proposal_lines_payment_proposals_ProposalId",
                table: "payment_proposal_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_pos_payments_pos_transactions_POSTransactionId",
                table: "pos_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_pos_transaction_lines_pos_transactions_POSTransactionId",
                table: "pos_transaction_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceAgreements_catalog_products_ProductId",
                table: "PriceAgreements");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceAgreements_product_variants_VariantId",
                table: "PriceAgreements");

            migrationBuilder.DropForeignKey(
                name: "FK_product_variants_catalog_products_ProductId",
                table: "product_variants");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_lines_product_variants_ProductVariantId",
                table: "purchase_order_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_lines_purchase_orders_PurchaseOrderId",
                table: "purchase_order_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_receipt_lines_purchase_order_lines_PurchaseO~",
                table: "purchase_order_receipt_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_receipt_lines_purchase_order_receipts_Receip~",
                table: "purchase_order_receipt_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_receipts_WarehouseLocations_WarehouseLocation",
                table: "purchase_order_receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_receipts_Warehouses_WarehouseId",
                table: "purchase_order_receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_order_receipts_purchase_orders_PurchaseOrderId",
                table: "purchase_order_receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_orders_Warehouses_WarehouseId",
                table: "purchase_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_orders_vendors_VendorId",
                table: "purchase_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_purchase_requisition_lines_purchase_requisitions_Requisitio~",
                table: "purchase_requisition_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_retail_transaction_staging_lines_retail_transaction_staging~",
                table: "retail_transaction_staging_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_retail_transaction_staging_tenders_retail_transaction_stagi~",
                table: "retail_transaction_staging_tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permissions_roles_RoleId",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_order_lines_product_variants_ProductVariantId",
                table: "sales_order_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_order_lines_sales_orders_SalesOrderId",
                table: "sales_order_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_orders_customers_CustomerId",
                table: "sales_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_quotation_lines_sales_quotations_QuotationId",
                table: "sales_quotation_lines");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_quotations_customers_CustomerId",
                table: "sales_quotations");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrderLines_TransferOrders_TransferOrderId",
                table: "TransferOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrderLines_WarehouseLocations_FromLocationId",
                table: "TransferOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrderLines_WarehouseLocations_ToLocationId",
                table: "TransferOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrders_Warehouses_FromWarehouseId",
                table: "TransferOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrders_Warehouses_ToWarehouseId",
                table: "TransferOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_app_users_UserId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_roles_RoleId",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_vendor_addresses_vendors_VendorId",
                table: "vendor_addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_vendor_contacts_vendors_VendorId",
                table: "vendor_contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_vendor_credit_notes_ap_invoices_APInvoiceId",
                table: "vendor_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "FK_vendor_credit_notes_vendors_VendorId",
                table: "vendor_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseInventoryBalances_WarehouseLocations_WarehouseLocat",
                table: "WarehouseInventoryBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseInventoryBalances_Warehouses_WarehouseId",
                table: "WarehouseInventoryBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseInventoryBalances_product_variants_ProductVariantId",
                table: "WarehouseInventoryBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseLocations_Warehouses_WarehouseId",
                table: "WarehouseLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_OperationalSites_SiteId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_WarehouseTypes_WarehouseTypeId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_workflow_approval_steps_workflow_instances_WorkflowInstance~",
                table: "workflow_approval_steps");

            migrationBuilder.DropForeignKey(
                name: "FK_workflow_instances_workflow_templates_TemplateId",
                table: "workflow_instances");

            migrationBuilder.DropForeignKey(
                name: "FK_workflow_template_steps_workflow_templates_WorkflowTemplate~",
                table: "workflow_template_steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workflow_templates",
                table: "workflow_templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workflow_template_steps",
                table: "workflow_template_steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workflow_instances",
                table: "workflow_instances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workflow_approval_steps",
                table: "workflow_approval_steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vendors",
                table: "vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vendor_credit_notes",
                table: "vendor_credit_notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vendor_contacts",
                table: "vendor_contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vendor_addresses",
                table: "vendor_addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_quotations",
                table: "sales_quotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_quotation_lines",
                table: "sales_quotation_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_orders",
                table: "sales_orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_order_lines",
                table: "sales_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permissions",
                table: "role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_retail_transaction_staging_tenders",
                table: "retail_transaction_staging_tenders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_retail_transaction_staging_lines",
                table: "retail_transaction_staging_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_retail_transaction_staging",
                table: "retail_transaction_staging");

            migrationBuilder.DropPrimaryKey(
                name: "PK_retail_tender_settlements",
                table: "retail_tender_settlements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_retail_stores",
                table: "retail_stores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_retail_statements",
                table: "retail_statements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase_requisitions",
                table: "purchase_requisitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase_requisition_lines",
                table: "purchase_requisition_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase_orders",
                table: "purchase_orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase_order_receipts",
                table: "purchase_order_receipts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase_order_receipt_lines",
                table: "purchase_order_receipt_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_purchase_order_lines",
                table: "purchase_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_promotions",
                table: "promotions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_variants",
                table: "product_variants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pos_transactions",
                table: "pos_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pos_transaction_lines",
                table: "pos_transaction_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pos_payments",
                table: "pos_payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payment_proposals",
                table: "payment_proposals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payment_proposal_lines",
                table: "payment_proposal_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations",
                table: "organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_journal_lines",
                table: "journal_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_journal_entries",
                table: "journal_entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventory_transactions",
                table: "inventory_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventory_records",
                table: "inventory_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_import_jobs",
                table: "import_jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_import_job_rows",
                table: "import_job_rows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fixed_assets",
                table: "fixed_assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fiscal_years",
                table: "fiscal_years");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fiscal_periods",
                table: "fiscal_periods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_export_job_rows",
                table: "export_job_rows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expense_reports",
                table: "expense_reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expense_lines",
                table: "expense_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expense_categories",
                table: "expense_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dunning_records",
                table: "dunning_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer_credit_notes",
                table: "customer_credit_notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer_contacts",
                table: "customer_contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer_addresses",
                table: "customer_addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_currencies",
                table: "currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_coupons",
                table: "coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_coupon_redemptions",
                table: "coupon_redemptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_catalog_products",
                table: "catalog_products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cash_journals",
                table: "cash_journals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cash_journal_lines",
                table: "cash_journal_lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_brands",
                table: "brands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_batch_job_configs",
                table: "batch_job_configs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_transactions",
                table: "bank_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_reconciliations",
                table: "bank_reconciliations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_accounts",
                table: "bank_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_audit_logs",
                table: "audit_logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asset_transfers",
                table: "asset_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asset_maintenances",
                table: "asset_maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asset_disposals",
                table: "asset_disposals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asset_depreciations",
                table: "asset_depreciations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ar_payments",
                table: "ar_payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ar_invoices",
                table: "ar_invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_users",
                table: "app_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ap_payments",
                table: "ap_payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ap_invoices",
                table: "ap_invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_account_types",
                table: "account_types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseTypes",
                table: "WarehouseTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseLocations",
                table: "WarehouseLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseInventoryBalances",
                table: "WarehouseInventoryBalances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransferOrders",
                table: "TransferOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransferOrderLines",
                table: "TransferOrderLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceAgreements",
                table: "PriceAgreements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboundOrders",
                table: "OutboundOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboundOrderLines",
                table: "OutboundOrderLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperationalSites",
                table: "OperationalSites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoyaltyPrograms",
                table: "LoyaltyPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboundOrders",
                table: "InboundOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InboundOrderLines",
                table: "InboundOrderLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerLoyaltyAccounts",
                table: "CustomerLoyaltyAccounts");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "warehouses");

            migrationBuilder.RenameTable(
                name: "Campaigns",
                newName: "campaigns");

            migrationBuilder.RenameTable(
                name: "WarehouseTypes",
                newName: "warehouse_types");

            migrationBuilder.RenameTable(
                name: "WarehouseLocations",
                newName: "warehouse_locations");

            migrationBuilder.RenameTable(
                name: "WarehouseInventoryBalances",
                newName: "warehouse_inventory_balances");

            migrationBuilder.RenameTable(
                name: "TransferOrders",
                newName: "transfer_orders");

            migrationBuilder.RenameTable(
                name: "TransferOrderLines",
                newName: "transfer_order_lines");

            migrationBuilder.RenameTable(
                name: "PriceAgreements",
                newName: "price_agreements");

            migrationBuilder.RenameTable(
                name: "OutboundOrders",
                newName: "outbound_orders");

            migrationBuilder.RenameTable(
                name: "OutboundOrderLines",
                newName: "outbound_order_lines");

            migrationBuilder.RenameTable(
                name: "OperationalSites",
                newName: "operational_sites");

            migrationBuilder.RenameTable(
                name: "LoyaltyPrograms",
                newName: "loyalty_programs");

            migrationBuilder.RenameTable(
                name: "InboundOrders",
                newName: "inbound_orders");

            migrationBuilder.RenameTable(
                name: "InboundOrderLines",
                newName: "inbound_order_lines");

            migrationBuilder.RenameTable(
                name: "CustomerLoyaltyAccounts",
                newName: "customer_loyalty_accounts");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "workflow_templates",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "workflow_templates",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "workflow_templates",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "workflow_templates",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "workflow_templates",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "workflow_templates",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "workflow_templates",
                newName: "document_type");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "workflow_templates",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AmountThreshold",
                table: "workflow_templates",
                newName: "amount_threshold");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "workflow_template_steps",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "workflow_template_steps",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowTemplateId",
                table: "workflow_template_steps",
                newName: "workflow_template_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "workflow_template_steps",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StepOrder",
                table: "workflow_template_steps",
                newName: "step_order");

            migrationBuilder.RenameColumn(
                name: "StepName",
                table: "workflow_template_steps",
                newName: "step_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "workflow_template_steps",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "workflow_template_steps",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ApproverUserId",
                table: "workflow_template_steps",
                newName: "approver_user_id");

            migrationBuilder.RenameColumn(
                name: "ApproverRole",
                table: "workflow_template_steps",
                newName: "approver_role");

            migrationBuilder.RenameIndex(
                name: "IX_workflow_template_steps_WorkflowTemplateId",
                table: "workflow_template_steps",
                newName: "ix_workflow_template_steps_workflow_template_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "workflow_instances",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "workflow_instances",
                newName: "comments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "workflow_instances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "workflow_instances",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalSteps",
                table: "workflow_instances",
                newName: "total_steps");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "workflow_instances",
                newName: "template_id");

            migrationBuilder.RenameColumn(
                name: "SubmittedBy",
                table: "workflow_instances",
                newName: "submitted_by");

            migrationBuilder.RenameColumn(
                name: "RejectedReason",
                table: "workflow_instances",
                newName: "rejected_reason");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "workflow_instances",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "workflow_instances",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "workflow_instances",
                newName: "document_type");

            migrationBuilder.RenameColumn(
                name: "DocumentRef",
                table: "workflow_instances",
                newName: "document_ref");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "workflow_instances",
                newName: "document_id");

            migrationBuilder.RenameColumn(
                name: "DocumentAmount",
                table: "workflow_instances",
                newName: "document_amount");

            migrationBuilder.RenameColumn(
                name: "CurrentStepIndex",
                table: "workflow_instances",
                newName: "current_step_index");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "workflow_instances",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "workflow_instances",
                newName: "completed_at");

            migrationBuilder.RenameIndex(
                name: "IX_workflow_instances_TemplateId",
                table: "workflow_instances",
                newName: "ix_workflow_instances_template_id");

            migrationBuilder.RenameIndex(
                name: "IX_workflow_instances_OrganizationId_Status",
                table: "workflow_instances",
                newName: "ix_workflow_instances_organization_id_status");

            migrationBuilder.RenameIndex(
                name: "IX_workflow_instances_OrganizationId_DocumentType_DocumentId",
                table: "workflow_instances",
                newName: "ix_workflow_instances_organization_id_document_type_document_id");

            migrationBuilder.RenameColumn(
                name: "Decision",
                table: "workflow_approval_steps",
                newName: "decision");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "workflow_approval_steps",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "workflow_approval_steps",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "workflow_approval_steps",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StepOrder",
                table: "workflow_approval_steps",
                newName: "step_order");

            migrationBuilder.RenameColumn(
                name: "StepName",
                table: "workflow_approval_steps",
                newName: "step_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "workflow_approval_steps",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "workflow_approval_steps",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ApproverUserId",
                table: "workflow_approval_steps",
                newName: "approver_user_id");

            migrationBuilder.RenameColumn(
                name: "ApproverRole",
                table: "workflow_approval_steps",
                newName: "approver_role");

            migrationBuilder.RenameColumn(
                name: "ActedByComments",
                table: "workflow_approval_steps",
                newName: "acted_by_comments");

            migrationBuilder.RenameColumn(
                name: "ActedBy",
                table: "workflow_approval_steps",
                newName: "acted_by");

            migrationBuilder.RenameColumn(
                name: "ActedAt",
                table: "workflow_approval_steps",
                newName: "acted_at");

            migrationBuilder.RenameIndex(
                name: "IX_workflow_approval_steps_WorkflowInstanceId",
                table: "workflow_approval_steps",
                newName: "ix_workflow_approval_steps_workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "warehouses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "warehouses",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "warehouses",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "warehouses",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "warehouses",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "warehouses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WarehouseTypeId",
                table: "warehouses",
                newName: "warehouse_type_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "warehouses",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "warehouses",
                newName: "site_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "warehouses",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "warehouses",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "warehouses",
                newName: "is_default");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "warehouses",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "warehouses",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_WarehouseTypeId",
                table: "warehouses",
                newName: "ix_warehouses_warehouse_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_SiteId",
                table: "warehouses",
                newName: "ix_warehouses_site_id");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouses_OrganizationId_Code",
                table: "warehouses",
                newName: "ix_warehouses_organization_id_code");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "vendors",
                newName: "website");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "vendors",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "vendors",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "vendors",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "vendors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "vendors",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "vendors",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "vendors",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vendors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VendorNumber",
                table: "vendors",
                newName: "vendor_number");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "vendors",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TaxId",
                table: "vendors",
                newName: "tax_id");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress",
                table: "vendors",
                newName: "shipping_address");

            migrationBuilder.RenameColumn(
                name: "PaymentTermsDays",
                table: "vendors",
                newName: "payment_terms_days");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "vendors",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsExported",
                table: "vendors",
                newName: "is_exported");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "vendors",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ExportedAt",
                table: "vendors",
                newName: "exported_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "vendors",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BillingAddress",
                table: "vendors",
                newName: "billing_address");

            migrationBuilder.RenameColumn(
                name: "BankRoutingNumber",
                table: "vendors",
                newName: "bank_routing_number");

            migrationBuilder.RenameColumn(
                name: "BankAccountNumber",
                table: "vendors",
                newName: "bank_account_number");

            migrationBuilder.RenameColumn(
                name: "BankAccountName",
                table: "vendors",
                newName: "bank_account_name");

            migrationBuilder.RenameIndex(
                name: "IX_vendors_OrganizationId_VendorNumber",
                table: "vendors",
                newName: "ix_vendors_organization_id_vendor_number");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "vendor_credit_notes",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "vendor_credit_notes",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "vendor_credit_notes",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "vendor_credit_notes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vendor_credit_notes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "vendor_credit_notes",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "VendorCNRef",
                table: "vendor_credit_notes",
                newName: "vendor_cn_ref");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "vendor_credit_notes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "vendor_credit_notes",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "vendor_credit_notes",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "vendor_credit_notes",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                table: "vendor_credit_notes",
                newName: "purchase_order_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "vendor_credit_notes",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "vendor_credit_notes",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreditNoteNumber",
                table: "vendor_credit_notes",
                newName: "credit_note_number");

            migrationBuilder.RenameColumn(
                name: "CreditDate",
                table: "vendor_credit_notes",
                newName: "credit_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "vendor_credit_notes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AppliedAmount",
                table: "vendor_credit_notes",
                newName: "applied_amount");

            migrationBuilder.RenameColumn(
                name: "APInvoiceId",
                table: "vendor_credit_notes",
                newName: "ap_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_vendor_credit_notes_VendorId",
                table: "vendor_credit_notes",
                newName: "ix_vendor_credit_notes_vendor_id");

            migrationBuilder.RenameIndex(
                name: "IX_vendor_credit_notes_OrganizationId_CreditNoteNumber",
                table: "vendor_credit_notes",
                newName: "ix_vendor_credit_notes_organization_id_credit_note_number");

            migrationBuilder.RenameIndex(
                name: "IX_vendor_credit_notes_APInvoiceId",
                table: "vendor_credit_notes",
                newName: "ix_vendor_credit_notes_ap_invoice_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "vendor_contacts",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "vendor_contacts",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "vendor_contacts",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "vendor_contacts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "vendor_contacts",
                newName: "mobile");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "vendor_contacts",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vendor_contacts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "vendor_contacts",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "vendor_contacts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "vendor_contacts",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "vendor_contacts",
                newName: "is_primary");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "vendor_contacts",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "vendor_contacts",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_vendor_contacts_VendorId",
                table: "vendor_contacts",
                newName: "ix_vendor_contacts_vendor_id");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "vendor_addresses",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Line2",
                table: "vendor_addresses",
                newName: "line2");

            migrationBuilder.RenameColumn(
                name: "Line1",
                table: "vendor_addresses",
                newName: "line1");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "vendor_addresses",
                newName: "label");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "vendor_addresses",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "vendor_addresses",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vendor_addresses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "vendor_addresses",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "vendor_addresses",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "vendor_addresses",
                newName: "postal_code");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "vendor_addresses",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "vendor_addresses",
                newName: "is_primary");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "vendor_addresses",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "vendor_addresses",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AddressType",
                table: "vendor_addresses",
                newName: "address_type");

            migrationBuilder.RenameIndex(
                name: "IX_vendor_addresses_VendorId",
                table: "vendor_addresses",
                newName: "ix_vendor_addresses_vendor_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_roles",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "user_roles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "user_roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "user_roles",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "user_roles",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_UserId_RoleId",
                table: "user_roles",
                newName: "ix_user_roles_user_id_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_RoleId",
                table: "user_roles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "sales_quotations",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "sales_quotations",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "sales_quotations",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "sales_quotations",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sales_quotations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "sales_quotations",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "ValidUntil",
                table: "sales_quotations",
                newName: "valid_until");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "sales_quotations",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "sales_quotations",
                newName: "tax_total");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "sales_quotations",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "sales_quotations",
                newName: "rejection_reason");

            migrationBuilder.RenameColumn(
                name: "QuotationNumber",
                table: "sales_quotations",
                newName: "quotation_number");

            migrationBuilder.RenameColumn(
                name: "QuotationDate",
                table: "sales_quotations",
                newName: "quotation_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "sales_quotations",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "sales_quotations",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "sales_quotations",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "DiscountTotal",
                table: "sales_quotations",
                newName: "discount_total");

            migrationBuilder.RenameColumn(
                name: "CustomerRef",
                table: "sales_quotations",
                newName: "customer_ref");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "sales_quotations",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "sales_quotations",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConvertedToSOId",
                table: "sales_quotations",
                newName: "converted_to_so_id");

            migrationBuilder.RenameColumn(
                name: "ConvertedAt",
                table: "sales_quotations",
                newName: "converted_at");

            migrationBuilder.RenameIndex(
                name: "IX_sales_quotations_OrganizationId_QuotationNumber",
                table: "sales_quotations",
                newName: "ix_sales_quotations_organization_id_quotation_number");

            migrationBuilder.RenameIndex(
                name: "IX_sales_quotations_CustomerId",
                table: "sales_quotations",
                newName: "ix_sales_quotations_customer_id");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "sales_quotation_lines",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "sales_quotation_lines",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sales_quotation_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VariantDescription",
                table: "sales_quotation_lines",
                newName: "variant_description");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "sales_quotation_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "sales_quotation_lines",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "sales_quotation_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "sales_quotation_lines",
                newName: "tax_rate");

            migrationBuilder.RenameColumn(
                name: "QuotationId",
                table: "sales_quotation_lines",
                newName: "quotation_id");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "sales_quotation_lines",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "sales_quotation_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "sales_quotation_lines",
                newName: "line_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "sales_quotation_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DiscountPct",
                table: "sales_quotation_lines",
                newName: "discount_pct");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "sales_quotation_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_sales_quotation_lines_QuotationId",
                table: "sales_quotation_lines",
                newName: "ix_sales_quotation_lines_quotation_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "sales_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "sales_orders",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "sales_orders",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sales_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "sales_orders",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "sales_orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "sales_orders",
                newName: "tax_total");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "sales_orders",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "RequestedShipDate",
                table: "sales_orders",
                newName: "requested_ship_date");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "sales_orders",
                newName: "rejection_reason");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "sales_orders",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "sales_orders",
                newName: "order_number");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "sales_orders",
                newName: "order_date");

            migrationBuilder.RenameColumn(
                name: "IsExported",
                table: "sales_orders",
                newName: "is_exported");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "sales_orders",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "sales_orders",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "ExportedAt",
                table: "sales_orders",
                newName: "exported_at");

            migrationBuilder.RenameColumn(
                name: "DiscountTotal",
                table: "sales_orders",
                newName: "discount_total");

            migrationBuilder.RenameColumn(
                name: "DeliveryReference",
                table: "sales_orders",
                newName: "delivery_reference");

            migrationBuilder.RenameColumn(
                name: "DeliveredAt",
                table: "sales_orders",
                newName: "delivered_at");

            migrationBuilder.RenameColumn(
                name: "CustomerRef",
                table: "sales_orders",
                newName: "customer_ref");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "sales_orders",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "sales_orders",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ActualShipDate",
                table: "sales_orders",
                newName: "actual_ship_date");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "sales_orders",
                newName: "ar_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_sales_orders_OrganizationId_OrderNumber",
                table: "sales_orders",
                newName: "ix_sales_orders_organization_id_order_number");

            migrationBuilder.RenameIndex(
                name: "IX_sales_orders_CustomerId",
                table: "sales_orders",
                newName: "ix_sales_orders_customer_id");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "sales_order_lines",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "sales_order_lines",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sales_order_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VariantDescription",
                table: "sales_order_lines",
                newName: "variant_description");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "sales_order_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "sales_order_lines",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "sales_order_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "sales_order_lines",
                newName: "tax_rate");

            migrationBuilder.RenameColumn(
                name: "SalesOrderId",
                table: "sales_order_lines",
                newName: "sales_order_id");

            migrationBuilder.RenameColumn(
                name: "QuantityShipped",
                table: "sales_order_lines",
                newName: "quantity_shipped");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "sales_order_lines",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "sales_order_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "sales_order_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DiscountPct",
                table: "sales_order_lines",
                newName: "discount_pct");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "sales_order_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_sales_order_lines_SalesOrderId",
                table: "sales_order_lines",
                newName: "ix_sales_order_lines_sales_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_sales_order_lines_ProductVariantId",
                table: "sales_order_lines",
                newName: "ix_sales_order_lines_product_variant_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "roles",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "roles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "roles",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsSystemRole",
                table: "roles",
                newName: "is_system_role");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "roles",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "roles",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_roles_OrganizationId_Name",
                table: "roles",
                newName: "ix_roles_organization_id_name");

            migrationBuilder.RenameColumn(
                name: "Module",
                table: "role_permissions",
                newName: "module");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "role_permissions",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "role_permissions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "role_permissions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "role_permissions",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "role_permissions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "role_permissions",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_role_permissions_RoleId_Module_Action",
                table: "role_permissions",
                newName: "ix_role_permissions_role_id_module_action");

            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "retail_transaction_staging_tenders",
                newName: "sequence");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "retail_transaction_staging_tenders",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "retail_transaction_staging_tenders",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "retail_transaction_staging_tenders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "retail_transaction_staging_tenders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RetailTransactionStagingId",
                table: "retail_transaction_staging_tenders",
                newName: "retail_transaction_staging_id");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "retail_transaction_staging_tenders",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "retail_transaction_staging_tenders",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "retail_transaction_staging_tenders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_retail_transaction_staging_tenders_RetailTransactionStaging~",
                table: "retail_transaction_staging_tenders",
                newName: "ix_retail_transaction_staging_tenders_retail_transaction_stagi");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "retail_transaction_staging_lines",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "retail_transaction_staging_lines",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "retail_transaction_staging_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ValidationMessage",
                table: "retail_transaction_staging_lines",
                newName: "validation_message");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "retail_transaction_staging_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "retail_transaction_staging_lines",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "retail_transaction_staging_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "retail_transaction_staging_lines",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "RetailTransactionStagingId",
                table: "retail_transaction_staging_lines",
                newName: "retail_transaction_staging_id");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "retail_transaction_staging_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "PosItemId",
                table: "retail_transaction_staging_lines",
                newName: "pos_item_id");

            migrationBuilder.RenameColumn(
                name: "MatchedProductVariantId",
                table: "retail_transaction_staging_lines",
                newName: "matched_product_variant_id");

            migrationBuilder.RenameColumn(
                name: "LineTotal",
                table: "retail_transaction_staging_lines",
                newName: "line_total");

            migrationBuilder.RenameColumn(
                name: "LineSubTotal",
                table: "retail_transaction_staging_lines",
                newName: "line_sub_total");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "retail_transaction_staging_lines",
                newName: "line_number");

            migrationBuilder.RenameColumn(
                name: "IsReturn",
                table: "retail_transaction_staging_lines",
                newName: "is_return");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "retail_transaction_staging_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "retail_transaction_staging_lines",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "retail_transaction_staging_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_retail_transaction_staging_lines_RetailTransactionStagingId~",
                table: "retail_transaction_staging_lines",
                newName: "ix_retail_transaction_staging_lines_retail_transaction_staging");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "retail_transaction_staging",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "retail_transaction_staging",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "retail_transaction_staging",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ValidationMessage",
                table: "retail_transaction_staging",
                newName: "validation_message");

            migrationBuilder.RenameColumn(
                name: "ValidatedAt",
                table: "retail_transaction_staging",
                newName: "validated_at");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "retail_transaction_staging",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TransactionNumber",
                table: "retail_transaction_staging",
                newName: "transaction_number");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "retail_transaction_staging",
                newName: "transaction_date");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "retail_transaction_staging",
                newName: "tax_total");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "retail_transaction_staging",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "StoreCode",
                table: "retail_transaction_staging",
                newName: "store_code");

            migrationBuilder.RenameColumn(
                name: "SourceHash",
                table: "retail_transaction_staging",
                newName: "source_hash");

            migrationBuilder.RenameColumn(
                name: "SourceFile",
                table: "retail_transaction_staging",
                newName: "source_file");

            migrationBuilder.RenameColumn(
                name: "RetailStatementId",
                table: "retail_transaction_staging",
                newName: "retail_statement_id");

            migrationBuilder.RenameColumn(
                name: "RawXml",
                table: "retail_transaction_staging",
                newName: "raw_xml");

            migrationBuilder.RenameColumn(
                name: "PromotedTransactionId",
                table: "retail_transaction_staging",
                newName: "promoted_transaction_id");

            migrationBuilder.RenameColumn(
                name: "PromotedAt",
                table: "retail_transaction_staging",
                newName: "promoted_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "retail_transaction_staging",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OperatorId",
                table: "retail_transaction_staging",
                newName: "operator_id");

            migrationBuilder.RenameColumn(
                name: "IsReturn",
                table: "retail_transaction_staging",
                newName: "is_return");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "retail_transaction_staging",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "retail_transaction_staging",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "DiscountTotal",
                table: "retail_transaction_staging",
                newName: "discount_total");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "retail_transaction_staging",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BusinessDate",
                table: "retail_transaction_staging",
                newName: "business_date");

            migrationBuilder.RenameIndex(
                name: "IX_retail_transaction_staging_OrganizationId_TransactionNumber",
                table: "retail_transaction_staging",
                newName: "ix_retail_transaction_staging_organization_id_transaction_numb");

            migrationBuilder.RenameIndex(
                name: "IX_retail_transaction_staging_OrganizationId_Status_CreatedAt",
                table: "retail_transaction_staging",
                newName: "ix_retail_transaction_staging_organization_id_status_created_at");

            migrationBuilder.RenameIndex(
                name: "IX_retail_transaction_staging_OrganizationId_SourceHash",
                table: "retail_transaction_staging",
                newName: "ix_retail_transaction_staging_organization_id_source_hash");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "retail_tender_settlements",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "retail_tender_settlements",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "retail_tender_settlements",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "retail_tender_settlements",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "retail_tender_settlements",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SettledAt",
                table: "retail_tender_settlements",
                newName: "settled_at");

            migrationBuilder.RenameColumn(
                name: "RetailStatementId",
                table: "retail_tender_settlements",
                newName: "retail_statement_id");

            migrationBuilder.RenameColumn(
                name: "ProcessorReference",
                table: "retail_tender_settlements",
                newName: "processor_reference");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "retail_tender_settlements",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "retail_tender_settlements",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "retail_tender_settlements",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "retail_tender_settlements",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BankTransactionId",
                table: "retail_tender_settlements",
                newName: "bank_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_retail_tender_settlements_RetailStatementId",
                table: "retail_tender_settlements",
                newName: "ix_retail_tender_settlements_retail_statement_id");

            migrationBuilder.RenameIndex(
                name: "IX_retail_tender_settlements_OrganizationId_Status",
                table: "retail_tender_settlements",
                newName: "ix_retail_tender_settlements_organization_id_status");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "retail_stores",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "retail_stores",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "retail_stores",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "retail_stores",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "retail_stores",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StoreCode",
                table: "retail_stores",
                newName: "store_code");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "retail_stores",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "ManagerName",
                table: "retail_stores",
                newName: "manager_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "retail_stores",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "retail_stores",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "retail_stores",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_retail_stores_OrganizationId_StoreCode",
                table: "retail_stores",
                newName: "ix_retail_stores_organization_id_store_code");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "retail_statements",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "retail_statements",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "retail_statements",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "retail_statements",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TransactionCount",
                table: "retail_statements",
                newName: "transaction_count");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "retail_statements",
                newName: "tax_total");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "retail_statements",
                newName: "store_id");

            migrationBuilder.RenameColumn(
                name: "StatementNumber",
                table: "retail_statements",
                newName: "statement_number");

            migrationBuilder.RenameColumn(
                name: "PostingError",
                table: "retail_statements",
                newName: "posting_error");

            migrationBuilder.RenameColumn(
                name: "PostedAt",
                table: "retail_statements",
                newName: "posted_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "retail_statements",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "NetSales",
                table: "retail_statements",
                newName: "net_sales");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "retail_statements",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "retail_statements",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "retail_statements",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "DiscountTotal",
                table: "retail_statements",
                newName: "discount_total");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "retail_statements",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CostTotal",
                table: "retail_statements",
                newName: "cost_total");

            migrationBuilder.RenameColumn(
                name: "BusinessDate",
                table: "retail_statements",
                newName: "business_date");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "retail_statements",
                newName: "ar_invoice_id");

            migrationBuilder.RenameColumn(
                name: "ARCreditNoteId",
                table: "retail_statements",
                newName: "ar_credit_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_retail_statements_OrganizationId_StoreId_BusinessDate_Curre~",
                table: "retail_statements",
                newName: "ix_retail_statements_organization_id_store_id_business_date_cu");

            migrationBuilder.RenameIndex(
                name: "IX_retail_statements_OrganizationId_StatementNumber",
                table: "retail_statements",
                newName: "ix_retail_statements_organization_id_statement_number");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "purchase_requisitions",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "purchase_requisitions",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "purchase_requisitions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "purchase_requisitions",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "purchase_requisitions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RequisitionNumber",
                table: "purchase_requisitions",
                newName: "requisition_number");

            migrationBuilder.RenameColumn(
                name: "RequestedBy",
                table: "purchase_requisitions",
                newName: "requested_by");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "purchase_requisitions",
                newName: "rejection_reason");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "purchase_requisitions",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "NeededByDate",
                table: "purchase_requisitions",
                newName: "needed_by_date");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "purchase_requisitions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DepartmentCode",
                table: "purchase_requisitions",
                newName: "department_code");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "purchase_requisitions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CostCenterCode",
                table: "purchase_requisitions",
                newName: "cost_center_code");

            migrationBuilder.RenameColumn(
                name: "ConvertedToPOId",
                table: "purchase_requisitions",
                newName: "converted_to_po_id");

            migrationBuilder.RenameColumn(
                name: "ConvertedAt",
                table: "purchase_requisitions",
                newName: "converted_at");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_requisitions_OrganizationId_RequisitionNumber",
                table: "purchase_requisitions",
                newName: "ix_purchase_requisitions_organization_id_requisition_number");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "purchase_requisition_lines",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "purchase_requisition_lines",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "purchase_requisition_lines",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "purchase_requisition_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "purchase_requisition_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "purchase_requisition_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "SuggestedVendorId",
                table: "purchase_requisition_lines",
                newName: "suggested_vendor_id");

            migrationBuilder.RenameColumn(
                name: "RequisitionId",
                table: "purchase_requisition_lines",
                newName: "requisition_id");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "purchase_requisition_lines",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "purchase_requisition_lines",
                newName: "line_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "purchase_requisition_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GlAccountCode",
                table: "purchase_requisition_lines",
                newName: "gl_account_code");

            migrationBuilder.RenameColumn(
                name: "EstimatedUnitCost",
                table: "purchase_requisition_lines",
                newName: "estimated_unit_cost");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "purchase_requisition_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_requisition_lines_RequisitionId",
                table: "purchase_requisition_lines",
                newName: "ix_purchase_requisition_lines_requisition_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "purchase_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "purchase_orders",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "purchase_orders",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "purchase_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "purchase_orders",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "purchase_orders",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "purchase_orders",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "purchase_orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "purchase_orders",
                newName: "tax_total");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "purchase_orders",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "purchase_orders",
                newName: "rejection_reason");

            migrationBuilder.RenameColumn(
                name: "PONumber",
                table: "purchase_orders",
                newName: "po_number");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "purchase_orders",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "purchase_orders",
                newName: "order_date");

            migrationBuilder.RenameColumn(
                name: "IsExported",
                table: "purchase_orders",
                newName: "is_exported");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "purchase_orders",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "InvoicedAmount",
                table: "purchase_orders",
                newName: "invoiced_amount");

            migrationBuilder.RenameColumn(
                name: "InvoiceStatus",
                table: "purchase_orders",
                newName: "invoice_status");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "purchase_orders",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "ExportedAt",
                table: "purchase_orders",
                newName: "exported_at");

            migrationBuilder.RenameColumn(
                name: "ExpectedDate",
                table: "purchase_orders",
                newName: "expected_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "purchase_orders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_orders_WarehouseId",
                table: "purchase_orders",
                newName: "ix_purchase_orders_warehouse_id");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_orders_VendorId",
                table: "purchase_orders",
                newName: "ix_purchase_orders_vendor_id");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_orders_OrganizationId_PONumber",
                table: "purchase_orders",
                newName: "ix_purchase_orders_organization_id_po_number");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "purchase_order_receipts",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "purchase_order_receipts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WarehouseLocationId",
                table: "purchase_order_receipts",
                newName: "warehouse_location_id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "purchase_order_receipts",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "purchase_order_receipts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "purchase_order_receipts",
                newName: "received_date");

            migrationBuilder.RenameColumn(
                name: "ReceiptNumber",
                table: "purchase_order_receipts",
                newName: "receipt_number");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                table: "purchase_order_receipts",
                newName: "purchase_order_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "purchase_order_receipts",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "purchase_order_receipts",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "purchase_order_receipts",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_receipts_WarehouseLocationId",
                table: "purchase_order_receipts",
                newName: "ix_purchase_order_receipts_warehouse_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_receipts_WarehouseId",
                table: "purchase_order_receipts",
                newName: "ix_purchase_order_receipts_warehouse_id");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_receipts_ReceiptNumber",
                table: "purchase_order_receipts",
                newName: "ix_purchase_order_receipts_receipt_number");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_receipts_PurchaseOrderId",
                table: "purchase_order_receipts",
                newName: "ix_purchase_order_receipts_purchase_order_id");

            migrationBuilder.RenameColumn(
                name: "Qty",
                table: "purchase_order_receipt_lines",
                newName: "qty");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "purchase_order_receipt_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "purchase_order_receipt_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ReceiptId",
                table: "purchase_order_receipt_lines",
                newName: "receipt_id");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderLineId",
                table: "purchase_order_receipt_lines",
                newName: "purchase_order_line_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "purchase_order_receipt_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "purchase_order_receipt_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_receipt_lines_ReceiptId",
                table: "purchase_order_receipt_lines",
                newName: "ix_purchase_order_receipt_lines_receipt_id");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_receipt_lines_PurchaseOrderLineId",
                table: "purchase_order_receipt_lines",
                newName: "ix_purchase_order_receipt_lines_purchase_order_line_id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "purchase_order_lines",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "purchase_order_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "purchase_order_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "purchase_order_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "purchase_order_lines",
                newName: "unit_cost");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "purchase_order_lines",
                newName: "tax_rate");

            migrationBuilder.RenameColumn(
                name: "ReceivedQty",
                table: "purchase_order_lines",
                newName: "received_qty");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                table: "purchase_order_lines",
                newName: "purchase_order_id");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "purchase_order_lines",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "purchase_order_lines",
                newName: "product_code");

            migrationBuilder.RenameColumn(
                name: "OrderedQty",
                table: "purchase_order_lines",
                newName: "ordered_qty");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "purchase_order_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "purchase_order_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_lines_PurchaseOrderId",
                table: "purchase_order_lines",
                newName: "ix_purchase_order_lines_purchase_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_purchase_order_lines_ProductVariantId",
                table: "purchase_order_lines",
                newName: "ix_purchase_order_lines_product_variant_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "promotions",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "promotions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "promotions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "promotions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsedCount",
                table: "promotions",
                newName: "used_count");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "promotions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "promotions",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "promotions",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "MinimumOrderAmount",
                table: "promotions",
                newName: "minimum_order_amount");

            migrationBuilder.RenameColumn(
                name: "MaxUsesTotal",
                table: "promotions",
                newName: "max_uses_total");

            migrationBuilder.RenameColumn(
                name: "MaxUsesPerCustomer",
                table: "promotions",
                newName: "max_uses_per_customer");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "promotions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GetQuantity",
                table: "promotions",
                newName: "get_quantity");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "promotions",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "promotions",
                newName: "discount_value");

            migrationBuilder.RenameColumn(
                name: "DiscountType",
                table: "promotions",
                newName: "discount_type");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "promotions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BuyQuantity",
                table: "promotions",
                newName: "buy_quantity");

            migrationBuilder.RenameColumn(
                name: "ApplyToAllProducts",
                table: "promotions",
                newName: "apply_to_all_products");

            migrationBuilder.RenameColumn(
                name: "ApplicableSkus",
                table: "promotions",
                newName: "applicable_skus");

            migrationBuilder.RenameIndex(
                name: "IX_promotions_OrganizationId",
                table: "promotions",
                newName: "ix_promotions_organization_id");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "product_variants",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "product_variants",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "product_variants",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "product_variants",
                newName: "size");

            migrationBuilder.RenameColumn(
                name: "Material",
                table: "product_variants",
                newName: "material");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "product_variants",
                newName: "color");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "product_variants",
                newName: "barcode");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "product_variants",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "product_variants",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "product_variants",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "PriceOverride",
                table: "product_variants",
                newName: "price_override");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "product_variants",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "product_variants",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "product_variants",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CostOverride",
                table: "product_variants",
                newName: "cost_override");

            migrationBuilder.RenameColumn(
                name: "AdditionalAttributes",
                table: "product_variants",
                newName: "additional_attributes");

            migrationBuilder.RenameIndex(
                name: "IX_product_variants_Barcode",
                table: "product_variants",
                newName: "ix_product_variants_barcode");

            migrationBuilder.RenameIndex(
                name: "IX_product_variants_ProductId",
                table: "product_variants",
                newName: "ix_product_variants_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_product_variants_OrganizationId_Sku",
                table: "product_variants",
                newName: "ix_product_variants_organization_id_sku");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "pos_transactions",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "pos_transactions",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Channel",
                table: "pos_transactions",
                newName: "channel");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pos_transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "pos_transactions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "pos_transactions",
                newName: "transaction_type");

            migrationBuilder.RenameColumn(
                name: "TransactionNumber",
                table: "pos_transactions",
                newName: "transaction_number");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "pos_transactions",
                newName: "transaction_date");

            migrationBuilder.RenameColumn(
                name: "TenderedAmount",
                table: "pos_transactions",
                newName: "tendered_amount");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "pos_transactions",
                newName: "tax_total");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "pos_transactions",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "pos_transactions",
                newName: "store_id");

            migrationBuilder.RenameColumn(
                name: "SourceFile",
                table: "pos_transactions",
                newName: "source_file");

            migrationBuilder.RenameColumn(
                name: "RetailStatementId",
                table: "pos_transactions",
                newName: "retail_statement_id");

            migrationBuilder.RenameColumn(
                name: "ProcessingError",
                table: "pos_transactions",
                newName: "processing_error");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "pos_transactions",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "pos_transactions",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "pos_transactions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "pos_transactions",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "FulfillmentStatus",
                table: "pos_transactions",
                newName: "fulfillment_status");

            migrationBuilder.RenameColumn(
                name: "ExternalRef",
                table: "pos_transactions",
                newName: "external_ref");

            migrationBuilder.RenameColumn(
                name: "ExternalOrderRef",
                table: "pos_transactions",
                newName: "external_order_ref");

            migrationBuilder.RenameColumn(
                name: "DiscountTotal",
                table: "pos_transactions",
                newName: "discount_total");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress",
                table: "pos_transactions",
                newName: "delivery_address");

            migrationBuilder.RenameColumn(
                name: "CustomerPhone",
                table: "pos_transactions",
                newName: "customer_phone");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "pos_transactions",
                newName: "customer_name");

            migrationBuilder.RenameColumn(
                name: "CustomerEmail",
                table: "pos_transactions",
                newName: "customer_email");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "pos_transactions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CouponDiscount",
                table: "pos_transactions",
                newName: "coupon_discount");

            migrationBuilder.RenameColumn(
                name: "CouponCode",
                table: "pos_transactions",
                newName: "coupon_code");

            migrationBuilder.RenameColumn(
                name: "ChannelNotes",
                table: "pos_transactions",
                newName: "channel_notes");

            migrationBuilder.RenameColumn(
                name: "ChangeAmount",
                table: "pos_transactions",
                newName: "change_amount");

            migrationBuilder.RenameColumn(
                name: "CashierName",
                table: "pos_transactions",
                newName: "cashier_name");

            migrationBuilder.RenameColumn(
                name: "CashierId",
                table: "pos_transactions",
                newName: "cashier_id");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "pos_transactions",
                newName: "ar_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_pos_transactions_Status",
                table: "pos_transactions",
                newName: "ix_pos_transactions_status");

            migrationBuilder.RenameIndex(
                name: "IX_pos_transactions_RetailStatementId",
                table: "pos_transactions",
                newName: "ix_pos_transactions_retail_statement_id");

            migrationBuilder.RenameIndex(
                name: "IX_pos_transactions_OrganizationId_TransactionNumber",
                table: "pos_transactions",
                newName: "ix_pos_transactions_organization_id_transaction_number");

            migrationBuilder.RenameIndex(
                name: "IX_pos_transactions_OrganizationId_ExternalRef",
                table: "pos_transactions",
                newName: "ix_pos_transactions_organization_id_external_ref");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "pos_transaction_lines",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "pos_transaction_lines",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pos_transaction_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "pos_transaction_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "pos_transaction_lines",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "pos_transaction_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "pos_transaction_lines",
                newName: "tax_rate");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "pos_transaction_lines",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "pos_transaction_lines",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "pos_transaction_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "POSTransactionId",
                table: "pos_transaction_lines",
                newName: "pos_transaction_id");

            migrationBuilder.RenameColumn(
                name: "LineTotal",
                table: "pos_transaction_lines",
                newName: "line_total");

            migrationBuilder.RenameColumn(
                name: "LineSubTotal",
                table: "pos_transaction_lines",
                newName: "line_sub_total");

            migrationBuilder.RenameColumn(
                name: "IsReturn",
                table: "pos_transaction_lines",
                newName: "is_return");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "pos_transaction_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DiscountPct",
                table: "pos_transaction_lines",
                newName: "discount_pct");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "pos_transaction_lines",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "pos_transaction_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_pos_transaction_lines_POSTransactionId",
                table: "pos_transaction_lines",
                newName: "ix_pos_transaction_lines_pos_transaction_id");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "pos_payments",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "pos_payments",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pos_payments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "pos_payments",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "pos_payments",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "POSTransactionId",
                table: "pos_payments",
                newName: "pos_transaction_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "pos_payments",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "pos_payments",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_pos_payments_POSTransactionId",
                table: "pos_payments",
                newName: "ix_pos_payments_pos_transaction_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "payment_proposals",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "payment_proposals",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "payment_proposals",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "payment_proposals",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ProposalNumber",
                table: "payment_proposals",
                newName: "proposal_number");

            migrationBuilder.RenameColumn(
                name: "ProposalDate",
                table: "payment_proposals",
                newName: "proposal_date");

            migrationBuilder.RenameColumn(
                name: "ProcessedBy",
                table: "payment_proposals",
                newName: "processed_by");

            migrationBuilder.RenameColumn(
                name: "ProcessedAt",
                table: "payment_proposals",
                newName: "processed_at");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "payment_proposals",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "payment_proposals",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "payment_proposals",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "payment_proposals",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "payment_proposals",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BankAccount",
                table: "payment_proposals",
                newName: "bank_account");

            migrationBuilder.RenameIndex(
                name: "IX_payment_proposals_OrganizationId_ProposalNumber",
                table: "payment_proposals",
                newName: "ix_payment_proposals_organization_id_proposal_number");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "payment_proposal_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "payment_proposal_lines",
                newName: "vendor_name");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "payment_proposal_lines",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "payment_proposal_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ProposedAmount",
                table: "payment_proposal_lines",
                newName: "proposed_amount");

            migrationBuilder.RenameColumn(
                name: "ProposalId",
                table: "payment_proposal_lines",
                newName: "proposal_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "payment_proposal_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "payment_proposal_lines",
                newName: "invoice_number");

            migrationBuilder.RenameColumn(
                name: "InvoiceDueDate",
                table: "payment_proposal_lines",
                newName: "invoice_due_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "payment_proposal_lines",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "APPaymentId",
                table: "payment_proposal_lines",
                newName: "ap_payment_id");

            migrationBuilder.RenameColumn(
                name: "APInvoiceId",
                table: "payment_proposal_lines",
                newName: "ap_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_proposal_lines_ProposalId",
                table: "payment_proposal_lines",
                newName: "ix_payment_proposal_lines_proposal_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_proposal_lines_APPaymentId",
                table: "payment_proposal_lines",
                newName: "ix_payment_proposal_lines_ap_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_proposal_lines_APInvoiceId",
                table: "payment_proposal_lines",
                newName: "ix_payment_proposal_lines_ap_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_organizations_code",
                table: "organizations",
                newName: "ix_organizations_code");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "journal_lines",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "journal_lines",
                newName: "debit");

            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "journal_lines",
                newName: "credit");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "journal_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "journal_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "LineOrder",
                table: "journal_lines",
                newName: "line_order");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "journal_lines",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "journal_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "journal_lines",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "journal_lines",
                newName: "account_id");

            migrationBuilder.RenameIndex(
                name: "IX_journal_lines_JournalEntryId",
                table: "journal_lines",
                newName: "ix_journal_lines_journal_entry_id");

            migrationBuilder.RenameIndex(
                name: "IX_journal_lines_AccountId",
                table: "journal_lines",
                newName: "ix_journal_lines_account_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "journal_entries",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "journal_entries",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "journal_entries",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "journal_entries",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "journal_entries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "journal_entries",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalDebit",
                table: "journal_entries",
                newName: "total_debit");

            migrationBuilder.RenameColumn(
                name: "TotalCredit",
                table: "journal_entries",
                newName: "total_credit");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "journal_entries",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "JournalType",
                table: "journal_entries",
                newName: "journal_type");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "journal_entries",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FiscalPeriodId",
                table: "journal_entries",
                newName: "fiscal_period_id");

            migrationBuilder.RenameColumn(
                name: "EntryNumber",
                table: "journal_entries",
                newName: "entry_number");

            migrationBuilder.RenameColumn(
                name: "EntryDate",
                table: "journal_entries",
                newName: "entry_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "journal_entries",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_journal_entries_OrganizationId_EntryNumber",
                table: "journal_entries",
                newName: "ix_journal_entries_organization_id_entry_number");

            migrationBuilder.RenameIndex(
                name: "IX_journal_entries_FiscalPeriodId",
                table: "journal_entries",
                newName: "ix_journal_entries_fiscal_period_id");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "inventory_transactions",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "inventory_transactions",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "inventory_transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "inventory_transactions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "inventory_transactions",
                newName: "unit_cost");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "inventory_transactions",
                newName: "transaction_type");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "inventory_transactions",
                newName: "transaction_date");

            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "inventory_transactions",
                newName: "reference_number");

            migrationBuilder.RenameColumn(
                name: "ReferenceDocumentId",
                table: "inventory_transactions",
                newName: "reference_document_id");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "inventory_transactions",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "inventory_transactions",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "inventory_transactions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "inventory_transactions",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "inventory_transactions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BalanceAfter",
                table: "inventory_transactions",
                newName: "balance_after");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_transactions_ProductVariantId",
                table: "inventory_transactions",
                newName: "ix_inventory_transactions_product_variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_transactions_OrganizationId_ProductVariantId_Tran~",
                table: "inventory_transactions",
                newName: "ix_inventory_transactions_organization_id_product_variant_id_t");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "inventory_records",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "inventory_records",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "inventory_records",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ReorderPoint",
                table: "inventory_records",
                newName: "reorder_point");

            migrationBuilder.RenameColumn(
                name: "QuantityReserved",
                table: "inventory_records",
                newName: "quantity_reserved");

            migrationBuilder.RenameColumn(
                name: "QuantityOnOrder",
                table: "inventory_records",
                newName: "quantity_on_order");

            migrationBuilder.RenameColumn(
                name: "QuantityOnHand",
                table: "inventory_records",
                newName: "quantity_on_hand");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "inventory_records",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "inventory_records",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "MinimumStock",
                table: "inventory_records",
                newName: "minimum_stock");

            migrationBuilder.RenameColumn(
                name: "MaximumStock",
                table: "inventory_records",
                newName: "maximum_stock");

            migrationBuilder.RenameColumn(
                name: "LastReceivedDate",
                table: "inventory_records",
                newName: "last_received_date");

            migrationBuilder.RenameColumn(
                name: "LastCountDate",
                table: "inventory_records",
                newName: "last_count_date");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "inventory_records",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "inventory_records",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AverageCost",
                table: "inventory_records",
                newName: "average_cost");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_records_ProductVariantId",
                table: "inventory_records",
                newName: "ix_inventory_records_product_variant_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "import_jobs",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "import_jobs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "import_jobs",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TriggeredBy",
                table: "import_jobs",
                newName: "triggered_by");

            migrationBuilder.RenameColumn(
                name: "TotalRows",
                table: "import_jobs",
                newName: "total_rows");

            migrationBuilder.RenameColumn(
                name: "SuccessRows",
                table: "import_jobs",
                newName: "success_rows");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "import_jobs",
                newName: "started_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "import_jobs",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "import_jobs",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "import_jobs",
                newName: "file_path");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "import_jobs",
                newName: "file_name");

            migrationBuilder.RenameColumn(
                name: "FileFormat",
                table: "import_jobs",
                newName: "file_format");

            migrationBuilder.RenameColumn(
                name: "FailedRows",
                table: "import_jobs",
                newName: "failed_rows");

            migrationBuilder.RenameColumn(
                name: "ErrorSummary",
                table: "import_jobs",
                newName: "error_summary");

            migrationBuilder.RenameColumn(
                name: "EntityType",
                table: "import_jobs",
                newName: "entity_type");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "import_jobs",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "import_jobs",
                newName: "completed_at");

            migrationBuilder.RenameIndex(
                name: "IX_import_jobs_Status",
                table: "import_jobs",
                newName: "ix_import_jobs_status");

            migrationBuilder.RenameIndex(
                name: "IX_import_jobs_OrganizationId",
                table: "import_jobs",
                newName: "ix_import_jobs_organization_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "import_job_rows",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "import_job_rows",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "import_job_rows",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RowNumber",
                table: "import_job_rows",
                newName: "row_number");

            migrationBuilder.RenameColumn(
                name: "RawPayload",
                table: "import_job_rows",
                newName: "raw_payload");

            migrationBuilder.RenameColumn(
                name: "PromotedEntityId",
                table: "import_job_rows",
                newName: "promoted_entity_id");

            migrationBuilder.RenameColumn(
                name: "PromotedAt",
                table: "import_job_rows",
                newName: "promoted_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "import_job_rows",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ImportJobId",
                table: "import_job_rows",
                newName: "import_job_id");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "import_job_rows",
                newName: "error_message");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "import_job_rows",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_import_job_rows_ImportJobId_Status",
                table: "import_job_rows",
                newName: "ix_import_job_rows_import_job_id_status");

            migrationBuilder.RenameIndex(
                name: "IX_import_job_rows_ImportJobId",
                table: "import_job_rows",
                newName: "ix_import_job_rows_import_job_id");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "fixed_assets",
                newName: "supplier");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "fixed_assets",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "fixed_assets",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "fixed_assets",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "fixed_assets",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "fixed_assets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsefulLifeYears",
                table: "fixed_assets",
                newName: "useful_life_years");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "fixed_assets",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitsProducedToDate",
                table: "fixed_assets",
                newName: "units_produced_to_date");

            migrationBuilder.RenameColumn(
                name: "TotalEstimatedUnits",
                table: "fixed_assets",
                newName: "total_estimated_units");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "fixed_assets",
                newName: "serial_number");

            migrationBuilder.RenameColumn(
                name: "SalvageValue",
                table: "fixed_assets",
                newName: "salvage_value");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderRef",
                table: "fixed_assets",
                newName: "purchase_order_ref");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "fixed_assets",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LastDepreciationDate",
                table: "fixed_assets",
                newName: "last_depreciation_date");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "fixed_assets",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GLDepreciationExpenseAccountId",
                table: "fixed_assets",
                newName: "gl_depreciation_expense_account_id");

            migrationBuilder.RenameColumn(
                name: "GLAssetAccountId",
                table: "fixed_assets",
                newName: "gl_asset_account_id");

            migrationBuilder.RenameColumn(
                name: "GLAccumulatedDepreciationAccountId",
                table: "fixed_assets",
                newName: "gl_accumulated_depreciation_account_id");

            migrationBuilder.RenameColumn(
                name: "DepreciationStartDate",
                table: "fixed_assets",
                newName: "depreciation_start_date");

            migrationBuilder.RenameColumn(
                name: "DepreciationMethod",
                table: "fixed_assets",
                newName: "depreciation_method");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "fixed_assets",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AssetName",
                table: "fixed_assets",
                newName: "asset_name");

            migrationBuilder.RenameColumn(
                name: "AssetCode",
                table: "fixed_assets",
                newName: "asset_code");

            migrationBuilder.RenameColumn(
                name: "AcquisitionDate",
                table: "fixed_assets",
                newName: "acquisition_date");

            migrationBuilder.RenameColumn(
                name: "AcquisitionCost",
                table: "fixed_assets",
                newName: "acquisition_cost");

            migrationBuilder.RenameColumn(
                name: "AccumulatedDepreciation",
                table: "fixed_assets",
                newName: "accumulated_depreciation");

            migrationBuilder.RenameIndex(
                name: "IX_fixed_assets_OrganizationId_AssetCode",
                table: "fixed_assets",
                newName: "ix_fixed_assets_organization_id_asset_code");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "fiscal_years",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "fiscal_years",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "fiscal_years",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "fiscal_years",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "fiscal_years",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "fiscal_years",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "PeriodCount",
                table: "fiscal_years",
                newName: "period_count");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "fiscal_years",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "fiscal_years",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "fiscal_years",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "fiscal_years",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CalendarType",
                table: "fiscal_years",
                newName: "calendar_type");

            migrationBuilder.RenameIndex(
                name: "IX_fiscal_years_OrganizationId_Name",
                table: "fiscal_years",
                newName: "ix_fiscal_years_organization_id_name");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "fiscal_periods",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "fiscal_periods",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "fiscal_periods",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "fiscal_periods",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "fiscal_periods",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "PeriodNumber",
                table: "fiscal_periods",
                newName: "period_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "fiscal_periods",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FiscalYearId",
                table: "fiscal_periods",
                newName: "fiscal_year_id");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "fiscal_periods",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "fiscal_periods",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_fiscal_periods_FiscalYearId",
                table: "fiscal_periods",
                newName: "ix_fiscal_periods_fiscal_year_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "export_job_rows",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "export_job_rows",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "export_job_rows",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "export_job_rows",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "export_job_rows",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ExportedAt",
                table: "export_job_rows",
                newName: "exported_at");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "export_job_rows",
                newName: "error_message");

            migrationBuilder.RenameColumn(
                name: "EntityType",
                table: "export_job_rows",
                newName: "entity_type");

            migrationBuilder.RenameColumn(
                name: "EntityRef",
                table: "export_job_rows",
                newName: "entity_ref");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "export_job_rows",
                newName: "entity_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "export_job_rows",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BlobName",
                table: "export_job_rows",
                newName: "blob_name");

            migrationBuilder.RenameColumn(
                name: "BatchJobConfigId",
                table: "export_job_rows",
                newName: "batch_job_config_id");

            migrationBuilder.RenameIndex(
                name: "IX_export_job_rows_OrganizationId",
                table: "export_job_rows",
                newName: "ix_export_job_rows_organization_id");

            migrationBuilder.RenameIndex(
                name: "IX_export_job_rows_EntityType_EntityId",
                table: "export_job_rows",
                newName: "ix_export_job_rows_entity_type_entity_id");

            migrationBuilder.RenameIndex(
                name: "IX_export_job_rows_BatchJobConfigId_ExportedAt",
                table: "export_job_rows",
                newName: "ix_export_job_rows_batch_job_config_id_exported_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "expense_reports",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "expense_reports",
                newName: "purpose");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "expense_reports",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "expense_reports",
                newName: "department");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "expense_reports",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expense_reports",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "expense_reports",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "expense_reports",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "expense_reports",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "SubmittedBy",
                table: "expense_reports",
                newName: "submitted_by");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "expense_reports",
                newName: "submitted_at");

            migrationBuilder.RenameColumn(
                name: "ReportNumber",
                table: "expense_reports",
                newName: "report_number");

            migrationBuilder.RenameColumn(
                name: "RejectedReason",
                table: "expense_reports",
                newName: "rejected_reason");

            migrationBuilder.RenameColumn(
                name: "PeriodStart",
                table: "expense_reports",
                newName: "period_start");

            migrationBuilder.RenameColumn(
                name: "PeriodEnd",
                table: "expense_reports",
                newName: "period_end");

            migrationBuilder.RenameColumn(
                name: "PaidAmount",
                table: "expense_reports",
                newName: "paid_amount");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "expense_reports",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "expense_reports",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "EmployeeName",
                table: "expense_reports",
                newName: "employee_name");

            migrationBuilder.RenameColumn(
                name: "EmployeeEmail",
                table: "expense_reports",
                newName: "employee_email");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "expense_reports",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ApprovedBy",
                table: "expense_reports",
                newName: "approved_by");

            migrationBuilder.RenameColumn(
                name: "ApprovedAt",
                table: "expense_reports",
                newName: "approved_at");

            migrationBuilder.RenameColumn(
                name: "ApprovedAmount",
                table: "expense_reports",
                newName: "approved_amount");

            migrationBuilder.RenameColumn(
                name: "ApprovalStatus",
                table: "expense_reports",
                newName: "approval_status");

            migrationBuilder.RenameIndex(
                name: "IX_expense_reports_OrganizationId_ReportNumber",
                table: "expense_reports",
                newName: "ix_expense_reports_organization_id_report_number");

            migrationBuilder.RenameColumn(
                name: "Merchant",
                table: "expense_lines",
                newName: "merchant");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "expense_lines",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "expense_lines",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expense_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "expense_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ReceiptUrl",
                table: "expense_lines",
                newName: "receipt_url");

            migrationBuilder.RenameColumn(
                name: "IsReimbursable",
                table: "expense_lines",
                newName: "is_reimbursable");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "expense_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ExpenseReportId",
                table: "expense_lines",
                newName: "expense_report_id");

            migrationBuilder.RenameColumn(
                name: "ExpenseDate",
                table: "expense_lines",
                newName: "expense_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "expense_lines",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "expense_lines",
                newName: "category_name");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "expense_lines",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_expense_lines_ExpenseReportId",
                table: "expense_lines",
                newName: "ix_expense_lines_expense_report_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "expense_categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "expense_categories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expense_categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "expense_categories",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "expense_categories",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LimitPerClaim",
                table: "expense_categories",
                newName: "limit_per_claim");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "expense_categories",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "expense_categories",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "GLAccountId",
                table: "expense_categories",
                newName: "gl_account_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "expense_categories",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "dunning_records",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "dunning_records",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "dunning_records",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "dunning_records",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "dunning_records",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SentDate",
                table: "dunning_records",
                newName: "sent_date");

            migrationBuilder.RenameColumn(
                name: "ResolvedAt",
                table: "dunning_records",
                newName: "resolved_at");

            migrationBuilder.RenameColumn(
                name: "ResolutionNotes",
                table: "dunning_records",
                newName: "resolution_notes");

            migrationBuilder.RenameColumn(
                name: "OutstandingAmount",
                table: "dunning_records",
                newName: "outstanding_amount");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "dunning_records",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "dunning_records",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FollowUpDate",
                table: "dunning_records",
                newName: "follow_up_date");

            migrationBuilder.RenameColumn(
                name: "DunningNumber",
                table: "dunning_records",
                newName: "dunning_number");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "dunning_records",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "dunning_records",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "dunning_records",
                newName: "assigned_to");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "dunning_records",
                newName: "ar_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_dunning_records_OrganizationId_DunningNumber",
                table: "dunning_records",
                newName: "ix_dunning_records_organization_id_dunning_number");

            migrationBuilder.RenameIndex(
                name: "IX_dunning_records_CustomerId",
                table: "dunning_records",
                newName: "ix_dunning_records_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_dunning_records_ARInvoiceId",
                table: "dunning_records",
                newName: "ix_dunning_records_ar_invoice_id");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "customers",
                newName: "website");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "customers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "customers",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "customers",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "customers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "customers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "customers",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "customers",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "customers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress",
                table: "customers",
                newName: "shipping_address");

            migrationBuilder.RenameColumn(
                name: "PaymentTermsDays",
                table: "customers",
                newName: "payment_terms_days");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "customers",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "customers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CustomerNumber",
                table: "customers",
                newName: "customer_number");

            migrationBuilder.RenameColumn(
                name: "CreditLimit",
                table: "customers",
                newName: "credit_limit");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "customers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BillingAddress",
                table: "customers",
                newName: "billing_address");

            migrationBuilder.RenameIndex(
                name: "IX_customers_OrganizationId_CustomerNumber",
                table: "customers",
                newName: "ix_customers_organization_id_customer_number");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "customer_credit_notes",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "customer_credit_notes",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "customer_credit_notes",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "customer_credit_notes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customer_credit_notes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "customer_credit_notes",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "customer_credit_notes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "customer_credit_notes",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "customer_credit_notes",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "customer_credit_notes",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "SalesOrderId",
                table: "customer_credit_notes",
                newName: "sales_order_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "customer_credit_notes",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "customer_credit_notes",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CustomerRef",
                table: "customer_credit_notes",
                newName: "customer_ref");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "customer_credit_notes",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreditNoteNumber",
                table: "customer_credit_notes",
                newName: "credit_note_number");

            migrationBuilder.RenameColumn(
                name: "CreditDate",
                table: "customer_credit_notes",
                newName: "credit_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "customer_credit_notes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AppliedAmount",
                table: "customer_credit_notes",
                newName: "applied_amount");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "customer_credit_notes",
                newName: "ar_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_customer_credit_notes_OrganizationId_CreditNoteNumber",
                table: "customer_credit_notes",
                newName: "ix_customer_credit_notes_organization_id_credit_note_number");

            migrationBuilder.RenameIndex(
                name: "IX_customer_credit_notes_CustomerId",
                table: "customer_credit_notes",
                newName: "ix_customer_credit_notes_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_customer_credit_notes_ARInvoiceId",
                table: "customer_credit_notes",
                newName: "ix_customer_credit_notes_ar_invoice_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "customer_contacts",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "customer_contacts",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "customer_contacts",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "customer_contacts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "customer_contacts",
                newName: "mobile");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "customer_contacts",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customer_contacts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "customer_contacts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "customer_contacts",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "customer_contacts",
                newName: "is_primary");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "customer_contacts",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "customer_contacts",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "customer_contacts",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_customer_contacts_CustomerId",
                table: "customer_contacts",
                newName: "ix_customer_contacts_customer_id");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "customer_addresses",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Line2",
                table: "customer_addresses",
                newName: "line2");

            migrationBuilder.RenameColumn(
                name: "Line1",
                table: "customer_addresses",
                newName: "line1");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "customer_addresses",
                newName: "label");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "customer_addresses",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "customer_addresses",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customer_addresses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "customer_addresses",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "customer_addresses",
                newName: "postal_code");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "customer_addresses",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "customer_addresses",
                newName: "is_primary");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "customer_addresses",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "customer_addresses",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "customer_addresses",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AddressType",
                table: "customer_addresses",
                newName: "address_type");

            migrationBuilder.RenameIndex(
                name: "IX_customer_addresses_CustomerId",
                table: "customer_addresses",
                newName: "ix_customer_addresses_customer_id");

            migrationBuilder.RenameColumn(
                name: "Symbol",
                table: "currencies",
                newName: "symbol");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "currencies",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "currencies",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "currencies",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "currencies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "currencies",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RateUpdatedAt",
                table: "currencies",
                newName: "rate_updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "currencies",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "NumericCode",
                table: "currencies",
                newName: "numeric_code");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "currencies",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsBase",
                table: "currencies",
                newName: "is_base");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "currencies",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "ExchangeRate",
                table: "currencies",
                newName: "exchange_rate");

            migrationBuilder.RenameColumn(
                name: "DecimalPlaces",
                table: "currencies",
                newName: "decimal_places");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "currencies",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_currencies_OrganizationId_Code",
                table: "currencies",
                newName: "ix_currencies_organization_id_code");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "coupons",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "coupons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsedCount",
                table: "coupons",
                newName: "used_count");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "coupons",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PromotionId",
                table: "coupons",
                newName: "promotion_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "coupons",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "MaxUses",
                table: "coupons",
                newName: "max_uses");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "coupons",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "coupons",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "coupons",
                newName: "expires_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "coupons",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_coupons_PromotionId",
                table: "coupons",
                newName: "ix_coupons_promotion_id");

            migrationBuilder.RenameIndex(
                name: "IX_coupons_OrganizationId_Code",
                table: "coupons",
                newName: "ix_coupons_organization_id_code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "coupon_redemptions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "coupon_redemptions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RedeemedAt",
                table: "coupon_redemptions",
                newName: "redeemed_at");

            migrationBuilder.RenameColumn(
                name: "POSTransactionId",
                table: "coupon_redemptions",
                newName: "pos_transaction_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "coupon_redemptions",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "coupon_redemptions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DiscountApplied",
                table: "coupon_redemptions",
                newName: "discount_applied");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "coupon_redemptions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CouponId",
                table: "coupon_redemptions",
                newName: "coupon_id");

            migrationBuilder.RenameIndex(
                name: "IX_coupon_redemptions_POSTransactionId",
                table: "coupon_redemptions",
                newName: "ix_coupon_redemptions_pos_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_coupon_redemptions_CouponId",
                table: "coupon_redemptions",
                newName: "ix_coupon_redemptions_coupon_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "categories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "categories",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "categories",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "categories",
                newName: "tax_rate");

            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "categories",
                newName: "tax_code");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryId",
                table: "categories",
                newName: "parent_category_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "categories",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "categories",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "categories",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "categories",
                newName: "display_order");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "categories",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_categories_ParentCategoryId",
                table: "categories",
                newName: "ix_categories_parent_category_id");

            migrationBuilder.RenameIndex(
                name: "IX_categories_OrganizationId_Code",
                table: "categories",
                newName: "ix_categories_organization_id_code");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "catalog_products",
                newName: "tags");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "catalog_products",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "catalog_products",
                newName: "sku");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "catalog_products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "catalog_products",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "catalog_products",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "catalog_products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "catalog_products",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "catalog_products",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "TaxRateOverride",
                table: "catalog_products",
                newName: "tax_rate_override");

            migrationBuilder.RenameColumn(
                name: "ProductType",
                table: "catalog_products",
                newName: "product_type");

            migrationBuilder.RenameColumn(
                name: "PreferredVendorId",
                table: "catalog_products",
                newName: "preferred_vendor_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "catalog_products",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LongDescription",
                table: "catalog_products",
                newName: "long_description");

            migrationBuilder.RenameColumn(
                name: "IsExported",
                table: "catalog_products",
                newName: "is_exported");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "catalog_products",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "catalog_products",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "GenderTarget",
                table: "catalog_products",
                newName: "gender_target");

            migrationBuilder.RenameColumn(
                name: "ExportedAt",
                table: "catalog_products",
                newName: "exported_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "catalog_products",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "catalog_products",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "catalog_products",
                newName: "brand_id");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "catalog_products",
                newName: "base_price");

            migrationBuilder.RenameColumn(
                name: "BaseCost",
                table: "catalog_products",
                newName: "base_cost");

            migrationBuilder.RenameIndex(
                name: "IX_catalog_products_PreferredVendorId",
                table: "catalog_products",
                newName: "ix_catalog_products_preferred_vendor_id");

            migrationBuilder.RenameIndex(
                name: "IX_catalog_products_OrganizationId_Sku",
                table: "catalog_products",
                newName: "ix_catalog_products_organization_id_sku");

            migrationBuilder.RenameIndex(
                name: "IX_catalog_products_CategoryId",
                table: "catalog_products",
                newName: "ix_catalog_products_category_id");

            migrationBuilder.RenameIndex(
                name: "IX_catalog_products_BrandId",
                table: "catalog_products",
                newName: "ix_catalog_products_brand_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "cash_journals",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "cash_journals",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "cash_journals",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cash_journals",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "cash_journals",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalDebits",
                table: "cash_journals",
                newName: "total_debits");

            migrationBuilder.RenameColumn(
                name: "TotalCredits",
                table: "cash_journals",
                newName: "total_credits");

            migrationBuilder.RenameColumn(
                name: "PostedBy",
                table: "cash_journals",
                newName: "posted_by");

            migrationBuilder.RenameColumn(
                name: "PostedAt",
                table: "cash_journals",
                newName: "posted_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "cash_journals",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "JournalNumber",
                table: "cash_journals",
                newName: "journal_number");

            migrationBuilder.RenameColumn(
                name: "JournalDate",
                table: "cash_journals",
                newName: "journal_date");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "cash_journals",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "cash_journals",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "cash_journals",
                newName: "bank_account_id");

            migrationBuilder.RenameIndex(
                name: "IX_cash_journals_Status",
                table: "cash_journals",
                newName: "ix_cash_journals_status");

            migrationBuilder.RenameIndex(
                name: "IX_cash_journals_OrganizationId_JournalNumber",
                table: "cash_journals",
                newName: "ix_cash_journals_organization_id_journal_number");

            migrationBuilder.RenameIndex(
                name: "IX_cash_journals_BankAccountId",
                table: "cash_journals",
                newName: "ix_cash_journals_bank_account_id");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "cash_journal_lines",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "cash_journal_lines",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Debit",
                table: "cash_journal_lines",
                newName: "debit");

            migrationBuilder.RenameColumn(
                name: "Credit",
                table: "cash_journal_lines",
                newName: "credit");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cash_journal_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "cash_journal_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "JournalId",
                table: "cash_journal_lines",
                newName: "journal_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "cash_journal_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GLAccountId",
                table: "cash_journal_lines",
                newName: "gl_account_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "cash_journal_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_cash_journal_lines_JournalId",
                table: "cash_journal_lines",
                newName: "ix_cash_journal_lines_journal_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "campaigns",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "campaigns",
                newName: "tags");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "campaigns",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "campaigns",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "campaigns",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "campaigns",
                newName: "budget");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "campaigns",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "campaigns",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TargetAudience",
                table: "campaigns",
                newName: "target_audience");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "campaigns",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "ReachCount",
                table: "campaigns",
                newName: "reach_count");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "campaigns",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LinkedPromotionId",
                table: "campaigns",
                newName: "linked_promotion_id");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "campaigns",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "campaigns",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConversionCount",
                table: "campaigns",
                newName: "conversion_count");

            migrationBuilder.RenameColumn(
                name: "ActualSpend",
                table: "campaigns",
                newName: "actual_spend");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_Status",
                table: "campaigns",
                newName: "ix_campaigns_status");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_OrganizationId",
                table: "campaigns",
                newName: "ix_campaigns_organization_id");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "brands",
                newName: "website");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "brands",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "brands",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "brands",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "brands",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "brands",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "brands",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "brands",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "brands",
                newName: "logo_url");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "brands",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "brands",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "brands",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_brands_OrganizationId_Code",
                table: "brands",
                newName: "ix_brands_organization_id_code");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "batch_job_configs",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "batch_job_configs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "batch_job_configs",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "batch_job_configs",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LocalProcessedPath",
                table: "batch_job_configs",
                newName: "local_processed_path");

            migrationBuilder.RenameColumn(
                name: "LocalInboxPath",
                table: "batch_job_configs",
                newName: "local_inbox_path");

            migrationBuilder.RenameColumn(
                name: "LocalExportPath",
                table: "batch_job_configs",
                newName: "local_export_path");

            migrationBuilder.RenameColumn(
                name: "LocalErrorPath",
                table: "batch_job_configs",
                newName: "local_error_path");

            migrationBuilder.RenameColumn(
                name: "LastRunStatus",
                table: "batch_job_configs",
                newName: "last_run_status");

            migrationBuilder.RenameColumn(
                name: "LastRunRowsPromoted",
                table: "batch_job_configs",
                newName: "last_run_rows_promoted");

            migrationBuilder.RenameColumn(
                name: "LastRunMessage",
                table: "batch_job_configs",
                newName: "last_run_message");

            migrationBuilder.RenameColumn(
                name: "LastRunFilesProcessed",
                table: "batch_job_configs",
                newName: "last_run_files_processed");

            migrationBuilder.RenameColumn(
                name: "LastRunAt",
                table: "batch_job_configs",
                newName: "last_run_at");

            migrationBuilder.RenameColumn(
                name: "JobType",
                table: "batch_job_configs",
                newName: "job_type");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "batch_job_configs",
                newName: "is_enabled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "batch_job_configs",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FileFormat",
                table: "batch_job_configs",
                newName: "file_format");

            migrationBuilder.RenameColumn(
                name: "ExportFileNamePattern",
                table: "batch_job_configs",
                newName: "export_file_name_pattern");

            migrationBuilder.RenameColumn(
                name: "CronExpression",
                table: "batch_job_configs",
                newName: "cron_expression");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "batch_job_configs",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AutoConfirmSalesOrders",
                table: "batch_job_configs",
                newName: "auto_confirm_sales_orders");

            migrationBuilder.RenameIndex(
                name: "IX_batch_job_configs_OrganizationId_IsEnabled",
                table: "batch_job_configs",
                newName: "ix_batch_job_configs_organization_id_is_enabled");

            migrationBuilder.RenameIndex(
                name: "IX_batch_job_configs_OrganizationId",
                table: "batch_job_configs",
                newName: "ix_batch_job_configs_organization_id");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "bank_transactions",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "bank_transactions",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "bank_transactions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "bank_transactions",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bank_transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "bank_transactions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TransferToAccountId",
                table: "bank_transactions",
                newName: "transfer_to_account_id");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "bank_transactions",
                newName: "transaction_type");

            migrationBuilder.RenameColumn(
                name: "TransactionStatus",
                table: "bank_transactions",
                newName: "transaction_status");

            migrationBuilder.RenameColumn(
                name: "TransactionNumber",
                table: "bank_transactions",
                newName: "transaction_number");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "bank_transactions",
                newName: "transaction_date");

            migrationBuilder.RenameColumn(
                name: "ReconciliationId",
                table: "bank_transactions",
                newName: "reconciliation_id");

            migrationBuilder.RenameColumn(
                name: "ReconciledAt",
                table: "bank_transactions",
                newName: "reconciled_at");

            migrationBuilder.RenameColumn(
                name: "PostedBy",
                table: "bank_transactions",
                newName: "posted_by");

            migrationBuilder.RenameColumn(
                name: "PostedAt",
                table: "bank_transactions",
                newName: "posted_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "bank_transactions",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "bank_transactions",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "bank_transactions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CounterpartyName",
                table: "bank_transactions",
                newName: "counterparty_name");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "bank_transactions",
                newName: "bank_account_id");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "bank_transactions",
                newName: "ar_invoice_id");

            migrationBuilder.RenameColumn(
                name: "APInvoiceId",
                table: "bank_transactions",
                newName: "ap_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_bank_transactions_TransactionStatus",
                table: "bank_transactions",
                newName: "ix_bank_transactions_transaction_status");

            migrationBuilder.RenameIndex(
                name: "IX_bank_transactions_OrganizationId_TransactionNumber",
                table: "bank_transactions",
                newName: "ix_bank_transactions_organization_id_transaction_number");

            migrationBuilder.RenameIndex(
                name: "IX_bank_transactions_BankAccountId_TransactionDate",
                table: "bank_transactions",
                newName: "ix_bank_transactions_bank_account_id_transaction_date");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "bank_reconciliations",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "bank_reconciliations",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bank_reconciliations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "bank_reconciliations",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SystemOpeningBalance",
                table: "bank_reconciliations",
                newName: "system_opening_balance");

            migrationBuilder.RenameColumn(
                name: "StatementStartDate",
                table: "bank_reconciliations",
                newName: "statement_start_date");

            migrationBuilder.RenameColumn(
                name: "StatementOpeningBalance",
                table: "bank_reconciliations",
                newName: "statement_opening_balance");

            migrationBuilder.RenameColumn(
                name: "StatementEndDate",
                table: "bank_reconciliations",
                newName: "statement_end_date");

            migrationBuilder.RenameColumn(
                name: "StatementClosingBalance",
                table: "bank_reconciliations",
                newName: "statement_closing_balance");

            migrationBuilder.RenameColumn(
                name: "ReconciliationNumber",
                table: "bank_reconciliations",
                newName: "reconciliation_number");

            migrationBuilder.RenameColumn(
                name: "ReconciledAmount",
                table: "bank_reconciliations",
                newName: "reconciled_amount");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "bank_reconciliations",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "bank_reconciliations",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "bank_reconciliations",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CompletedBy",
                table: "bank_reconciliations",
                newName: "completed_by");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "bank_reconciliations",
                newName: "completed_at");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "bank_reconciliations",
                newName: "bank_account_id");

            migrationBuilder.RenameIndex(
                name: "IX_bank_reconciliations_OrganizationId_ReconciliationNumber",
                table: "bank_reconciliations",
                newName: "ix_bank_reconciliations_organization_id_reconciliation_number");

            migrationBuilder.RenameIndex(
                name: "IX_bank_reconciliations_BankAccountId",
                table: "bank_reconciliations",
                newName: "ix_bank_reconciliations_bank_account_id");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "bank_accounts",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "IBAN",
                table: "bank_accounts",
                newName: "iban");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "bank_accounts",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bank_accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "bank_accounts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SwiftCode",
                table: "bank_accounts",
                newName: "swift_code");

            migrationBuilder.RenameColumn(
                name: "RoutingNumber",
                table: "bank_accounts",
                newName: "routing_number");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "bank_accounts",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LastReconciledBalance",
                table: "bank_accounts",
                newName: "last_reconciled_balance");

            migrationBuilder.RenameColumn(
                name: "LastReconciledAt",
                table: "bank_accounts",
                newName: "last_reconciled_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "bank_accounts",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GLAccountId",
                table: "bank_accounts",
                newName: "gl_account_id");

            migrationBuilder.RenameColumn(
                name: "CurrentBalance",
                table: "bank_accounts",
                newName: "current_balance");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "bank_accounts",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BankName",
                table: "bank_accounts",
                newName: "bank_name");

            migrationBuilder.RenameColumn(
                name: "BankBranch",
                table: "bank_accounts",
                newName: "bank_branch");

            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "bank_accounts",
                newName: "account_type");

            migrationBuilder.RenameColumn(
                name: "AccountStatus",
                table: "bank_accounts",
                newName: "account_status");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "bank_accounts",
                newName: "account_number");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "bank_accounts",
                newName: "account_name");

            migrationBuilder.RenameColumn(
                name: "AccountCode",
                table: "bank_accounts",
                newName: "account_code");

            migrationBuilder.RenameIndex(
                name: "IX_bank_accounts_OrganizationId_AccountCode",
                table: "bank_accounts",
                newName: "ix_bank_accounts_organization_id_account_code");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "audit_logs",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Module",
                table: "audit_logs",
                newName: "module");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "audit_logs",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "audit_logs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "audit_logs",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "audit_logs",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "audit_logs",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OldValues",
                table: "audit_logs",
                newName: "old_values");

            migrationBuilder.RenameColumn(
                name: "OccurredAt",
                table: "audit_logs",
                newName: "occurred_at");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "audit_logs",
                newName: "new_values");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "audit_logs",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "audit_logs",
                newName: "ip_address");

            migrationBuilder.RenameColumn(
                name: "EntityType",
                table: "audit_logs",
                newName: "entity_type");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "audit_logs",
                newName: "entity_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "audit_logs",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_audit_logs_UserId",
                table: "audit_logs",
                newName: "ix_audit_logs_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_audit_logs_OrganizationId_OccurredAt",
                table: "audit_logs",
                newName: "ix_audit_logs_organization_id_occurred_at");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "asset_transfers",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asset_transfers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "asset_transfers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TransferredBy",
                table: "asset_transfers",
                newName: "transferred_by");

            migrationBuilder.RenameColumn(
                name: "TransferDate",
                table: "asset_transfers",
                newName: "transfer_date");

            migrationBuilder.RenameColumn(
                name: "ToLocation",
                table: "asset_transfers",
                newName: "to_location");

            migrationBuilder.RenameColumn(
                name: "ToDepartment",
                table: "asset_transfers",
                newName: "to_department");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "asset_transfers",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "asset_transfers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FromLocation",
                table: "asset_transfers",
                newName: "from_location");

            migrationBuilder.RenameColumn(
                name: "FromDepartment",
                table: "asset_transfers",
                newName: "from_department");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "asset_transfers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "asset_transfers",
                newName: "asset_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_transfers_AssetId",
                table: "asset_transfers",
                newName: "ix_asset_transfers_asset_id");

            migrationBuilder.RenameColumn(
                name: "Vendor",
                table: "asset_maintenances",
                newName: "vendor");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "asset_maintenances",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "asset_maintenances",
                newName: "cost");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asset_maintenances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "asset_maintenances",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PerformedBy",
                table: "asset_maintenances",
                newName: "performed_by");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "asset_maintenances",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "NextMaintenanceDue",
                table: "asset_maintenances",
                newName: "next_maintenance_due");

            migrationBuilder.RenameColumn(
                name: "MaintenanceType",
                table: "asset_maintenances",
                newName: "maintenance_type");

            migrationBuilder.RenameColumn(
                name: "MaintenanceDate",
                table: "asset_maintenances",
                newName: "maintenance_date");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "asset_maintenances",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "asset_maintenances",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CapitalizeCost",
                table: "asset_maintenances",
                newName: "capitalize_cost");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "asset_maintenances",
                newName: "asset_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_maintenances_AssetId",
                table: "asset_maintenances",
                newName: "ix_asset_maintenances_asset_id");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "asset_disposals",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asset_disposals",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "asset_disposals",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "asset_disposals",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "NetBookValueAtDisposal",
                table: "asset_disposals",
                newName: "net_book_value_at_disposal");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "asset_disposals",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "asset_disposals",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GLGainLossAccountId",
                table: "asset_disposals",
                newName: "gl_gain_loss_account_id");

            migrationBuilder.RenameColumn(
                name: "DisposedBy",
                table: "asset_disposals",
                newName: "disposed_by");

            migrationBuilder.RenameColumn(
                name: "DisposalType",
                table: "asset_disposals",
                newName: "disposal_type");

            migrationBuilder.RenameColumn(
                name: "DisposalProceeds",
                table: "asset_disposals",
                newName: "disposal_proceeds");

            migrationBuilder.RenameColumn(
                name: "DisposalDate",
                table: "asset_disposals",
                newName: "disposal_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "asset_disposals",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BuyerName",
                table: "asset_disposals",
                newName: "buyer_name");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "asset_disposals",
                newName: "asset_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_disposals_AssetId",
                table: "asset_disposals",
                newName: "ix_asset_disposals_asset_id");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "asset_depreciations",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "asset_depreciations",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "asset_depreciations",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "asset_depreciations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "asset_depreciations",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RunningNBV",
                table: "asset_depreciations",
                newName: "running_nbv");

            migrationBuilder.RenameColumn(
                name: "PostedBy",
                table: "asset_depreciations",
                newName: "posted_by");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "asset_depreciations",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "asset_depreciations",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "asset_depreciations",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "asset_depreciations",
                newName: "asset_id");

            migrationBuilder.RenameIndex(
                name: "IX_asset_depreciations_AssetId",
                table: "asset_depreciations",
                newName: "ix_asset_depreciations_asset_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ar_payments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "ar_payments",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "ar_payments",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ar_payments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ar_payments",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PaymentNumber",
                table: "ar_payments",
                newName: "payment_number");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "ar_payments",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "ar_payments",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "ar_payments",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "ar_payments",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ar_payments",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ar_payments",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ar_payments",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ARInvoiceId",
                table: "ar_payments",
                newName: "ar_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_ar_payments_CustomerId",
                table: "ar_payments",
                newName: "ix_ar_payments_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_ar_payments_ARInvoiceId",
                table: "ar_payments",
                newName: "ix_ar_payments_ar_invoice_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ar_invoices",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ar_invoices",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ar_invoices",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "ar_invoices",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ar_invoices",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "ar_invoices",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "ar_invoices",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "ar_invoices",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "SalesOrderId",
                table: "ar_invoices",
                newName: "sales_order_id");

            migrationBuilder.RenameColumn(
                name: "PaidAmount",
                table: "ar_invoices",
                newName: "paid_amount");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "ar_invoices",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "ar_invoices",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ar_invoices",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "ar_invoices",
                newName: "invoice_number");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "ar_invoices",
                newName: "invoice_date");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "ar_invoices",
                newName: "due_date");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "ar_invoices",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ar_invoices",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ar_invoices",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_ar_invoices_SalesOrderId",
                table: "ar_invoices",
                newName: "ix_ar_invoices_sales_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_ar_invoices_OrganizationId_InvoiceNumber",
                table: "ar_invoices",
                newName: "ix_ar_invoices_organization_id_invoice_number");

            migrationBuilder.RenameIndex(
                name: "IX_ar_invoices_CustomerId",
                table: "ar_invoices",
                newName: "ix_ar_invoices_customer_id");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "app_users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "app_users",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "app_users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "app_users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "app_users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiry",
                table: "app_users",
                newName: "refresh_token_expiry");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "app_users",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "app_users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "app_users",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LockedUntil",
                table: "app_users",
                newName: "locked_until");

            migrationBuilder.RenameColumn(
                name: "LastLoginAt",
                table: "app_users",
                newName: "last_login_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "app_users",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "app_users",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "FailedLoginAttempts",
                table: "app_users",
                newName: "failed_login_attempts");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "app_users",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_app_users_OrganizationId_Username",
                table: "app_users",
                newName: "ix_app_users_organization_id_username");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ap_payments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "ap_payments",
                newName: "reference");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "ap_payments",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ap_payments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "ap_payments",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ap_payments",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PaymentNumber",
                table: "ap_payments",
                newName: "payment_number");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "ap_payments",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "ap_payments",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "ap_payments",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "ap_payments",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ap_payments",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ap_payments",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "APInvoiceId",
                table: "ap_payments",
                newName: "ap_invoice_id");

            migrationBuilder.RenameIndex(
                name: "IX_ap_payments_VendorId",
                table: "ap_payments",
                newName: "ix_ap_payments_vendor_id");

            migrationBuilder.RenameIndex(
                name: "IX_ap_payments_APInvoiceId",
                table: "ap_payments",
                newName: "ix_ap_payments_ap_invoice_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ap_invoices",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ap_invoices",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ap_invoices",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "ap_invoices",
                newName: "workflow_instance_id");

            migrationBuilder.RenameColumn(
                name: "VendorInvoiceRef",
                table: "ap_invoices",
                newName: "vendor_invoice_ref");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "ap_invoices",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ap_invoices",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "ap_invoices",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "ap_invoices",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "ap_invoices",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                table: "ap_invoices",
                newName: "purchase_order_id");

            migrationBuilder.RenameColumn(
                name: "PrepaymentApplied",
                table: "ap_invoices",
                newName: "prepayment_applied");

            migrationBuilder.RenameColumn(
                name: "PaidAmount",
                table: "ap_invoices",
                newName: "paid_amount");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "ap_invoices",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "MatchStatus",
                table: "ap_invoices",
                newName: "match_status");

            migrationBuilder.RenameColumn(
                name: "MatchNotes",
                table: "ap_invoices",
                newName: "match_notes");

            migrationBuilder.RenameColumn(
                name: "LinkedPrepaymentInvoiceId",
                table: "ap_invoices",
                newName: "linked_prepayment_invoice_id");

            migrationBuilder.RenameColumn(
                name: "JournalEntryId",
                table: "ap_invoices",
                newName: "journal_entry_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ap_invoices",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "InvoiceType",
                table: "ap_invoices",
                newName: "invoice_type");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "ap_invoices",
                newName: "invoice_number");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "ap_invoices",
                newName: "invoice_date");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "ap_invoices",
                newName: "due_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ap_invoices",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BypassReason",
                table: "ap_invoices",
                newName: "bypass_reason");

            migrationBuilder.RenameIndex(
                name: "IX_ap_invoices_VendorId",
                table: "ap_invoices",
                newName: "ix_ap_invoices_vendor_id");

            migrationBuilder.RenameIndex(
                name: "IX_ap_invoices_PurchaseOrderId",
                table: "ap_invoices",
                newName: "ix_ap_invoices_purchase_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_ap_invoices_OrganizationId_InvoiceNumber",
                table: "ap_invoices",
                newName: "ix_ap_invoices_organization_id_invoice_number");

            migrationBuilder.RenameIndex(
                name: "IX_ap_invoices_LinkedPrepaymentInvoiceId",
                table: "ap_invoices",
                newName: "ix_ap_invoices_linked_prepayment_invoice_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "accounts",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "accounts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "accounts",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "accounts",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "accounts",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "accounts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                table: "accounts",
                newName: "parent_account_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "accounts",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsHeaderAccount",
                table: "accounts",
                newName: "is_header_account");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "accounts",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "accounts",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AllowManualEntry",
                table: "accounts",
                newName: "allow_manual_entry");

            migrationBuilder.RenameColumn(
                name: "AccountTypeId",
                table: "accounts",
                newName: "account_type_id");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "accounts",
                newName: "account_number");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_ParentAccountId",
                table: "accounts",
                newName: "ix_accounts_parent_account_id");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_OrganizationId_AccountNumber",
                table: "accounts",
                newName: "ix_accounts_organization_id_account_number");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_AccountTypeId",
                table: "accounts",
                newName: "ix_accounts_account_type_id");

            migrationBuilder.RenameColumn(
                name: "Nature",
                table: "account_types",
                newName: "nature");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "account_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "account_types",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "account_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "account_types",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "account_types",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "account_types",
                newName: "display_order");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "account_types",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "warehouse_types",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "warehouse_types",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "warehouse_types",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "warehouse_types",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "warehouse_types",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "warehouse_types",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "warehouse_types",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "warehouse_types",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTypes_OrganizationId_Name",
                table: "warehouse_types",
                newName: "ix_warehouse_types_organization_id_name");

            migrationBuilder.RenameColumn(
                name: "Zone",
                table: "warehouse_locations",
                newName: "zone");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "warehouse_locations",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "warehouse_locations",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Bin",
                table: "warehouse_locations",
                newName: "bin");

            migrationBuilder.RenameColumn(
                name: "Bay",
                table: "warehouse_locations",
                newName: "bay");

            migrationBuilder.RenameColumn(
                name: "Aisle",
                table: "warehouse_locations",
                newName: "aisle");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "warehouse_locations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "warehouse_locations",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "warehouse_locations",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsReceivable",
                table: "warehouse_locations",
                newName: "is_receivable");

            migrationBuilder.RenameColumn(
                name: "IsPickable",
                table: "warehouse_locations",
                newName: "is_pickable");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "warehouse_locations",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "warehouse_locations",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "warehouse_locations",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseLocations_WarehouseId_Code",
                table: "warehouse_locations",
                newName: "ix_warehouse_locations_warehouse_id_code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "warehouse_inventory_balances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WarehouseLocationId",
                table: "warehouse_inventory_balances",
                newName: "warehouse_location_id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "warehouse_inventory_balances",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "warehouse_inventory_balances",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "QuantityReserved",
                table: "warehouse_inventory_balances",
                newName: "quantity_reserved");

            migrationBuilder.RenameColumn(
                name: "QuantityOnHand",
                table: "warehouse_inventory_balances",
                newName: "quantity_on_hand");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "warehouse_inventory_balances",
                newName: "product_variant_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "warehouse_inventory_balances",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "warehouse_inventory_balances",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "warehouse_inventory_balances",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseInventoryBalances_WarehouseLocationId",
                table: "warehouse_inventory_balances",
                newName: "ix_warehouse_inventory_balances_warehouse_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseInventoryBalances_WarehouseId",
                table: "warehouse_inventory_balances",
                newName: "ix_warehouse_inventory_balances_warehouse_id");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseInventoryBalances_ProductVariantId",
                table: "warehouse_inventory_balances",
                newName: "ix_warehouse_inventory_balances_product_variant_id");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseInventoryBalances_OrganizationId_ProductVariantId_W",
                table: "warehouse_inventory_balances",
                newName: "ix_warehouse_inventory_balances_organization_id_product_varian");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "transfer_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "transfer_orders",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transfer_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "transfer_orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ToWarehouseId",
                table: "transfer_orders",
                newName: "to_warehouse_id");

            migrationBuilder.RenameColumn(
                name: "ShippedDate",
                table: "transfer_orders",
                newName: "shipped_date");

            migrationBuilder.RenameColumn(
                name: "RequestedDate",
                table: "transfer_orders",
                newName: "requested_date");

            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "transfer_orders",
                newName: "received_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "transfer_orders",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "transfer_orders",
                newName: "order_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "transfer_orders",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FromWarehouseId",
                table: "transfer_orders",
                newName: "from_warehouse_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "transfer_orders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrders_ToWarehouseId",
                table: "transfer_orders",
                newName: "ix_transfer_orders_to_warehouse_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrders_OrganizationId_OrderNumber",
                table: "transfer_orders",
                newName: "ix_transfer_orders_organization_id_order_number");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrders_FromWarehouseId",
                table: "transfer_orders",
                newName: "ix_transfer_orders_from_warehouse_id");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "transfer_order_lines",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transfer_order_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "transfer_order_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "transfer_order_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "TransferOrderId",
                table: "transfer_order_lines",
                newName: "transfer_order_id");

            migrationBuilder.RenameColumn(
                name: "ToLocationId",
                table: "transfer_order_lines",
                newName: "to_location_id");

            migrationBuilder.RenameColumn(
                name: "ShippedQuantity",
                table: "transfer_order_lines",
                newName: "shipped_quantity");

            migrationBuilder.RenameColumn(
                name: "RequestedQuantity",
                table: "transfer_order_lines",
                newName: "requested_quantity");

            migrationBuilder.RenameColumn(
                name: "ReceivedQuantity",
                table: "transfer_order_lines",
                newName: "received_quantity");

            migrationBuilder.RenameColumn(
                name: "ProductSku",
                table: "transfer_order_lines",
                newName: "product_sku");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "transfer_order_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "transfer_order_lines",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "LotNumber",
                table: "transfer_order_lines",
                newName: "lot_number");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "transfer_order_lines",
                newName: "line_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "transfer_order_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FromLocationId",
                table: "transfer_order_lines",
                newName: "from_location_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "transfer_order_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrderLines_TransferOrderId",
                table: "transfer_order_lines",
                newName: "ix_transfer_order_lines_transfer_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrderLines_ToLocationId",
                table: "transfer_order_lines",
                newName: "ix_transfer_order_lines_to_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrderLines_FromLocationId",
                table: "transfer_order_lines",
                newName: "ix_transfer_order_lines_from_location_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "price_agreements",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "price_agreements",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "price_agreements",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "price_agreements",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "price_agreements",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "price_agreements",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "price_agreements",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "price_agreements",
                newName: "variant_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "price_agreements",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "price_agreements",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "price_agreements",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "PriceType",
                table: "price_agreements",
                newName: "price_type");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "price_agreements",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "price_agreements",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "price_agreements",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "price_agreements",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "price_agreements",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_PriceAgreements_VariantId_PriceType_IsActive",
                table: "price_agreements",
                newName: "ix_price_agreements_variant_id_price_type_is_active");

            migrationBuilder.RenameIndex(
                name: "IX_PriceAgreements_ProductId_PriceType_IsActive",
                table: "price_agreements",
                newName: "ix_price_agreements_product_id_price_type_is_active");

            migrationBuilder.RenameIndex(
                name: "IX_PriceAgreements_OrganizationId_IsActive_StartDate_EndDate",
                table: "price_agreements",
                newName: "ix_price_agreements_organization_id_is_active_start_date_end_d");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "outbound_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "outbound_orders",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Carrier",
                table: "outbound_orders",
                newName: "carrier");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "outbound_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "outbound_orders",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "outbound_orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TrackingNumber",
                table: "outbound_orders",
                newName: "tracking_number");

            migrationBuilder.RenameColumn(
                name: "ShippedDate",
                table: "outbound_orders",
                newName: "shipped_date");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress",
                table: "outbound_orders",
                newName: "ship_to_address");

            migrationBuilder.RenameColumn(
                name: "SalesOrderId",
                table: "outbound_orders",
                newName: "sales_order_id");

            migrationBuilder.RenameColumn(
                name: "RequestedDate",
                table: "outbound_orders",
                newName: "requested_date");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "outbound_orders",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "outbound_orders",
                newName: "order_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "outbound_orders",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "outbound_orders",
                newName: "customer_name");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "outbound_orders",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "outbound_orders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_OutboundOrders_WarehouseId",
                table: "outbound_orders",
                newName: "ix_outbound_orders_warehouse_id");

            migrationBuilder.RenameIndex(
                name: "IX_OutboundOrders_OrganizationId_OrderNumber",
                table: "outbound_orders",
                newName: "ix_outbound_orders_organization_id_order_number");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "outbound_order_lines",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "outbound_order_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "outbound_order_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "outbound_order_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "ShippedQuantity",
                table: "outbound_order_lines",
                newName: "shipped_quantity");

            migrationBuilder.RenameColumn(
                name: "RequestedQuantity",
                table: "outbound_order_lines",
                newName: "requested_quantity");

            migrationBuilder.RenameColumn(
                name: "ProductSku",
                table: "outbound_order_lines",
                newName: "product_sku");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "outbound_order_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "outbound_order_lines",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "PickedQuantity",
                table: "outbound_order_lines",
                newName: "picked_quantity");

            migrationBuilder.RenameColumn(
                name: "OutboundOrderId",
                table: "outbound_order_lines",
                newName: "outbound_order_id");

            migrationBuilder.RenameColumn(
                name: "LotNumber",
                table: "outbound_order_lines",
                newName: "lot_number");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "outbound_order_lines",
                newName: "line_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "outbound_order_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FromLocationId",
                table: "outbound_order_lines",
                newName: "from_location_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "outbound_order_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_OutboundOrderLines_OutboundOrderId",
                table: "outbound_order_lines",
                newName: "ix_outbound_order_lines_outbound_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_OutboundOrderLines_FromLocationId",
                table: "outbound_order_lines",
                newName: "ix_outbound_order_lines_from_location_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "operational_sites",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "operational_sites",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "operational_sites",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "operational_sites",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "operational_sites",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "operational_sites",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "operational_sites",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "operational_sites",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsWarehouse",
                table: "operational_sites",
                newName: "is_warehouse");

            migrationBuilder.RenameColumn(
                name: "IsReturnCenter",
                table: "operational_sites",
                newName: "is_return_center");

            migrationBuilder.RenameColumn(
                name: "IsRetailStore",
                table: "operational_sites",
                newName: "is_retail_store");

            migrationBuilder.RenameColumn(
                name: "IsFulfillmentCenter",
                table: "operational_sites",
                newName: "is_fulfillment_center");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "operational_sites",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "operational_sites",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "operational_sites",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_OperationalSites_OrganizationId_Code",
                table: "operational_sites",
                newName: "ix_operational_sites_organization_id_code");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "loyalty_programs",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "loyalty_programs",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loyalty_programs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "loyalty_programs",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SilverThreshold",
                table: "loyalty_programs",
                newName: "silver_threshold");

            migrationBuilder.RenameColumn(
                name: "RedemptionThreshold",
                table: "loyalty_programs",
                newName: "redemption_threshold");

            migrationBuilder.RenameColumn(
                name: "PointsPerDollar",
                table: "loyalty_programs",
                newName: "points_per_dollar");

            migrationBuilder.RenameColumn(
                name: "PlatinumThreshold",
                table: "loyalty_programs",
                newName: "platinum_threshold");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "loyalty_programs",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "loyalty_programs",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "GoldThreshold",
                table: "loyalty_programs",
                newName: "gold_threshold");

            migrationBuilder.RenameColumn(
                name: "DollarPerPoint",
                table: "loyalty_programs",
                newName: "dollar_per_point");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "loyalty_programs",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_LoyaltyPrograms_OrganizationId",
                table: "loyalty_programs",
                newName: "ix_loyalty_programs_organization_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "inbound_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "inbound_orders",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "inbound_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "inbound_orders",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "inbound_orders",
                newName: "vendor_name");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "inbound_orders",
                newName: "vendor_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "inbound_orders",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "inbound_orders",
                newName: "received_date");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                table: "inbound_orders",
                newName: "purchase_order_id");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "inbound_orders",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "inbound_orders",
                newName: "order_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "inbound_orders",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "ExpectedDate",
                table: "inbound_orders",
                newName: "expected_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "inbound_orders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_InboundOrders_WarehouseId",
                table: "inbound_orders",
                newName: "ix_inbound_orders_warehouse_id");

            migrationBuilder.RenameIndex(
                name: "IX_InboundOrders_OrganizationId_OrderNumber",
                table: "inbound_orders",
                newName: "ix_inbound_orders_organization_id_order_number");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "inbound_order_lines",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "inbound_order_lines",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "inbound_order_lines",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "inbound_order_lines",
                newName: "unit_of_measure");

            migrationBuilder.RenameColumn(
                name: "ReceivedQuantity",
                table: "inbound_order_lines",
                newName: "received_quantity");

            migrationBuilder.RenameColumn(
                name: "ProductSku",
                table: "inbound_order_lines",
                newName: "product_sku");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "inbound_order_lines",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "inbound_order_lines",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "OrderedQuantity",
                table: "inbound_order_lines",
                newName: "ordered_quantity");

            migrationBuilder.RenameColumn(
                name: "LotNumber",
                table: "inbound_order_lines",
                newName: "lot_number");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "inbound_order_lines",
                newName: "location_id");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "inbound_order_lines",
                newName: "line_number");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "inbound_order_lines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "InboundOrderId",
                table: "inbound_order_lines",
                newName: "inbound_order_id");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "inbound_order_lines",
                newName: "expiry_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "inbound_order_lines",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_InboundOrderLines_LocationId",
                table: "inbound_order_lines",
                newName: "ix_inbound_order_lines_location_id");

            migrationBuilder.RenameIndex(
                name: "IX_InboundOrderLines_InboundOrderId",
                table: "inbound_order_lines",
                newName: "ix_inbound_order_lines_inbound_order_id");

            migrationBuilder.RenameColumn(
                name: "Tier",
                table: "customer_loyalty_accounts",
                newName: "tier");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customer_loyalty_accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "customer_loyalty_accounts",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TotalPoints",
                table: "customer_loyalty_accounts",
                newName: "total_points");

            migrationBuilder.RenameColumn(
                name: "RedeemedPoints",
                table: "customer_loyalty_accounts",
                newName: "redeemed_points");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "customer_loyalty_accounts",
                newName: "organization_id");

            migrationBuilder.RenameColumn(
                name: "LoyaltyProgramId",
                table: "customer_loyalty_accounts",
                newName: "loyalty_program_id");

            migrationBuilder.RenameColumn(
                name: "LastActivityAt",
                table: "customer_loyalty_accounts",
                newName: "last_activity_at");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "customer_loyalty_accounts",
                newName: "customer_name");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "customer_loyalty_accounts",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CustomerEmail",
                table: "customer_loyalty_accounts",
                newName: "customer_email");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "customer_loyalty_accounts",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerLoyaltyAccounts_OrganizationId_CustomerId",
                table: "customer_loyalty_accounts",
                newName: "ix_customer_loyalty_accounts_organization_id_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerLoyaltyAccounts_LoyaltyProgramId",
                table: "customer_loyalty_accounts",
                newName: "ix_customer_loyalty_accounts_loyalty_program_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_workflow_templates",
                table: "workflow_templates",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_workflow_template_steps",
                table: "workflow_template_steps",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_workflow_instances",
                table: "workflow_instances",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_workflow_approval_steps",
                table: "workflow_approval_steps",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_warehouses",
                table: "warehouses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_vendors",
                table: "vendors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_vendor_credit_notes",
                table: "vendor_credit_notes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_vendor_contacts",
                table: "vendor_contacts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_vendor_addresses",
                table: "vendor_addresses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sales_quotations",
                table: "sales_quotations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sales_quotation_lines",
                table: "sales_quotation_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sales_orders",
                table: "sales_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sales_order_lines",
                table: "sales_order_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                table: "roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permissions",
                table: "role_permissions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_retail_transaction_staging_tenders",
                table: "retail_transaction_staging_tenders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_retail_transaction_staging_lines",
                table: "retail_transaction_staging_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_retail_transaction_staging",
                table: "retail_transaction_staging",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_retail_tender_settlements",
                table: "retail_tender_settlements",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_retail_stores",
                table: "retail_stores",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_retail_statements",
                table: "retail_statements",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_purchase_requisitions",
                table: "purchase_requisitions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_purchase_requisition_lines",
                table: "purchase_requisition_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_purchase_orders",
                table: "purchase_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_purchase_order_receipts",
                table: "purchase_order_receipts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_purchase_order_receipt_lines",
                table: "purchase_order_receipt_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_purchase_order_lines",
                table: "purchase_order_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_promotions",
                table: "promotions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_product_variants",
                table: "product_variants",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pos_transactions",
                table: "pos_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pos_transaction_lines",
                table: "pos_transaction_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pos_payments",
                table: "pos_payments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_payment_proposals",
                table: "payment_proposals",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_payment_proposal_lines",
                table: "payment_proposal_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_organizations",
                table: "organizations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_journal_lines",
                table: "journal_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_journal_entries",
                table: "journal_entries",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inventory_transactions",
                table: "inventory_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inventory_records",
                table: "inventory_records",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_import_jobs",
                table: "import_jobs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_import_job_rows",
                table: "import_job_rows",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_fixed_assets",
                table: "fixed_assets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_fiscal_years",
                table: "fiscal_years",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_fiscal_periods",
                table: "fiscal_periods",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_export_job_rows",
                table: "export_job_rows",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expense_reports",
                table: "expense_reports",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expense_lines",
                table: "expense_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_expense_categories",
                table: "expense_categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_dunning_records",
                table: "dunning_records",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customers",
                table: "customers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_credit_notes",
                table: "customer_credit_notes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_contacts",
                table: "customer_contacts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_addresses",
                table: "customer_addresses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_currencies",
                table: "currencies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_coupons",
                table: "coupons",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_coupon_redemptions",
                table: "coupon_redemptions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_catalog_products",
                table: "catalog_products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cash_journals",
                table: "cash_journals",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cash_journal_lines",
                table: "cash_journal_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_campaigns",
                table: "campaigns",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_brands",
                table: "brands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_batch_job_configs",
                table: "batch_job_configs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_transactions",
                table: "bank_transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_reconciliations",
                table: "bank_reconciliations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_accounts",
                table: "bank_accounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_audit_logs",
                table: "audit_logs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asset_transfers",
                table: "asset_transfers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asset_maintenances",
                table: "asset_maintenances",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asset_disposals",
                table: "asset_disposals",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_asset_depreciations",
                table: "asset_depreciations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ar_payments",
                table: "ar_payments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ar_invoices",
                table: "ar_invoices",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_users",
                table: "app_users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ap_payments",
                table: "ap_payments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ap_invoices",
                table: "ap_invoices",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounts",
                table: "accounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_account_types",
                table: "account_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_warehouse_types",
                table: "warehouse_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_warehouse_locations",
                table: "warehouse_locations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_warehouse_inventory_balances",
                table: "warehouse_inventory_balances",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transfer_orders",
                table: "transfer_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transfer_order_lines",
                table: "transfer_order_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_price_agreements",
                table: "price_agreements",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbound_orders",
                table: "outbound_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbound_order_lines",
                table: "outbound_order_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_operational_sites",
                table: "operational_sites",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_loyalty_programs",
                table: "loyalty_programs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbound_orders",
                table: "inbound_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbound_order_lines",
                table: "inbound_order_lines",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_loyalty_accounts",
                table: "customer_loyalty_accounts",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_account_types_account_type_id",
                table: "accounts",
                column: "account_type_id",
                principalTable: "account_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_accounts_parent_account_id",
                table: "accounts",
                column: "parent_account_id",
                principalTable: "accounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ap_invoices_ap_invoices_linked_prepayment_invoice_id",
                table: "ap_invoices",
                column: "linked_prepayment_invoice_id",
                principalTable: "ap_invoices",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ap_invoices_purchase_orders_purchase_order_id",
                table: "ap_invoices",
                column: "purchase_order_id",
                principalTable: "purchase_orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ap_invoices_vendors_vendor_id",
                table: "ap_invoices",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ap_payments_ap_invoices_ap_invoice_id",
                table: "ap_payments",
                column: "ap_invoice_id",
                principalTable: "ap_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ap_payments_vendors_vendor_id",
                table: "ap_payments",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ar_invoices_customers_customer_id",
                table: "ar_invoices",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ar_invoices_sales_orders_sales_order_id",
                table: "ar_invoices",
                column: "sales_order_id",
                principalTable: "sales_orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ar_payments_ar_invoices_ar_invoice_id",
                table: "ar_payments",
                column: "ar_invoice_id",
                principalTable: "ar_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ar_payments_customers_customer_id",
                table: "ar_payments",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asset_depreciations_fixed_assets_asset_id",
                table: "asset_depreciations",
                column: "asset_id",
                principalTable: "fixed_assets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asset_disposals_fixed_assets_asset_id",
                table: "asset_disposals",
                column: "asset_id",
                principalTable: "fixed_assets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asset_maintenances_fixed_assets_asset_id",
                table: "asset_maintenances",
                column: "asset_id",
                principalTable: "fixed_assets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_asset_transfers_fixed_assets_asset_id",
                table: "asset_transfers",
                column: "asset_id",
                principalTable: "fixed_assets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bank_reconciliations_bank_accounts_bank_account_id",
                table: "bank_reconciliations",
                column: "bank_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_bank_transactions_bank_accounts_bank_account_id",
                table: "bank_transactions",
                column: "bank_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_cash_journal_lines_cash_journals_journal_id",
                table: "cash_journal_lines",
                column: "journal_id",
                principalTable: "cash_journals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cash_journals_bank_accounts_bank_account_id",
                table: "cash_journals",
                column: "bank_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_products_brands_brand_id",
                table: "catalog_products",
                column: "brand_id",
                principalTable: "brands",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_products_categories_category_id",
                table: "catalog_products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_products_vendors_preferred_vendor_id",
                table: "catalog_products",
                column: "preferred_vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_categories_categories_parent_category_id",
                table: "categories",
                column: "parent_category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_coupons_promotions_promotion_id",
                table: "coupons",
                column: "promotion_id",
                principalTable: "promotions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_addresses_customers_customer_id",
                table: "customer_addresses",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_contacts_customers_customer_id",
                table: "customer_contacts",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_credit_notes_ar_invoices_ar_invoice_id",
                table: "customer_credit_notes",
                column: "ar_invoice_id",
                principalTable: "ar_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_credit_notes_customers_customer_id",
                table: "customer_credit_notes",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_loyalty_accounts_loyalty_programs_loyalty_program_",
                table: "customer_loyalty_accounts",
                column: "loyalty_program_id",
                principalTable: "loyalty_programs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_dunning_records_ar_invoices_ar_invoice_id",
                table: "dunning_records",
                column: "ar_invoice_id",
                principalTable: "ar_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_dunning_records_customers_customer_id",
                table: "dunning_records",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_expense_lines_expense_reports_expense_report_id",
                table: "expense_lines",
                column: "expense_report_id",
                principalTable: "expense_reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_fiscal_periods_fiscal_years_fiscal_year_id",
                table: "fiscal_periods",
                column: "fiscal_year_id",
                principalTable: "fiscal_years",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_import_job_rows_import_jobs_import_job_id",
                table: "import_job_rows",
                column: "import_job_id",
                principalTable: "import_jobs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_inbound_order_lines_inbound_orders_inbound_order_id",
                table: "inbound_order_lines",
                column: "inbound_order_id",
                principalTable: "inbound_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_inbound_order_lines_warehouse_locations_location_id",
                table: "inbound_order_lines",
                column: "location_id",
                principalTable: "warehouse_locations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_inbound_orders_warehouses_warehouse_id",
                table: "inbound_orders",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_inventory_records_product_variants_product_variant_id",
                table: "inventory_records",
                column: "product_variant_id",
                principalTable: "product_variants",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_inventory_transactions_product_variants_product_variant_id",
                table: "inventory_transactions",
                column: "product_variant_id",
                principalTable: "product_variants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_journal_entries_fiscal_periods_fiscal_period_id",
                table: "journal_entries",
                column: "fiscal_period_id",
                principalTable: "fiscal_periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_journal_lines_accounts_account_id",
                table: "journal_lines",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_journal_lines_journal_entries_journal_entry_id",
                table: "journal_lines",
                column: "journal_entry_id",
                principalTable: "journal_entries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_outbound_order_lines_outbound_orders_outbound_order_id",
                table: "outbound_order_lines",
                column: "outbound_order_id",
                principalTable: "outbound_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_outbound_order_lines_warehouse_locations_from_location_id",
                table: "outbound_order_lines",
                column: "from_location_id",
                principalTable: "warehouse_locations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_outbound_orders_warehouses_warehouse_id",
                table: "outbound_orders",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payment_proposal_lines_ap_invoices_ap_invoice_id",
                table: "payment_proposal_lines",
                column: "ap_invoice_id",
                principalTable: "ap_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_payment_proposal_lines_ap_payments_ap_payment_id",
                table: "payment_proposal_lines",
                column: "ap_payment_id",
                principalTable: "ap_payments",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_payment_proposal_lines_payment_proposals_proposal_id",
                table: "payment_proposal_lines",
                column: "proposal_id",
                principalTable: "payment_proposals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pos_payments_pos_transactions_pos_transaction_id",
                table: "pos_payments",
                column: "pos_transaction_id",
                principalTable: "pos_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pos_transaction_lines_pos_transactions_pos_transaction_id",
                table: "pos_transaction_lines",
                column: "pos_transaction_id",
                principalTable: "pos_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_price_agreements_catalog_products_product_id",
                table: "price_agreements",
                column: "product_id",
                principalTable: "catalog_products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_price_agreements_product_variants_variant_id",
                table: "price_agreements",
                column: "variant_id",
                principalTable: "product_variants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_product_variants_catalog_products_product_id",
                table: "product_variants",
                column: "product_id",
                principalTable: "catalog_products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_lines_product_variants_product_variant_id",
                table: "purchase_order_lines",
                column: "product_variant_id",
                principalTable: "product_variants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_lines_purchase_orders_purchase_order_id",
                table: "purchase_order_lines",
                column: "purchase_order_id",
                principalTable: "purchase_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_receipt_lines_purchase_order_lines_purchase_",
                table: "purchase_order_receipt_lines",
                column: "purchase_order_line_id",
                principalTable: "purchase_order_lines",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_receipt_lines_purchase_order_receipts_receip",
                table: "purchase_order_receipt_lines",
                column: "receipt_id",
                principalTable: "purchase_order_receipts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_receipts_purchase_orders_purchase_order_id",
                table: "purchase_order_receipts",
                column: "purchase_order_id",
                principalTable: "purchase_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_receipts_warehouse_locations_warehouse_locat",
                table: "purchase_order_receipts",
                column: "warehouse_location_id",
                principalTable: "warehouse_locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_order_receipts_warehouses_warehouse_id",
                table: "purchase_order_receipts",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_orders_vendors_vendor_id",
                table: "purchase_orders",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_orders_warehouses_warehouse_id",
                table: "purchase_orders",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_purchase_requisition_lines_purchase_requisitions_requisitio",
                table: "purchase_requisition_lines",
                column: "requisition_id",
                principalTable: "purchase_requisitions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_retail_transaction_staging_lines_retail_transaction_staging",
                table: "retail_transaction_staging_lines",
                column: "retail_transaction_staging_id",
                principalTable: "retail_transaction_staging",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_retail_transaction_staging_tenders_retail_transaction_stagi",
                table: "retail_transaction_staging_tenders",
                column: "retail_transaction_staging_id",
                principalTable: "retail_transaction_staging",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_roles_role_id",
                table: "role_permissions",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sales_order_lines_product_variants_product_variant_id",
                table: "sales_order_lines",
                column: "product_variant_id",
                principalTable: "product_variants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sales_order_lines_sales_orders_sales_order_id",
                table: "sales_order_lines",
                column: "sales_order_id",
                principalTable: "sales_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sales_orders_customers_customer_id",
                table: "sales_orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sales_quotation_lines_sales_quotations_quotation_id",
                table: "sales_quotation_lines",
                column: "quotation_id",
                principalTable: "sales_quotations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sales_quotations_customers_customer_id",
                table: "sales_quotations",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_transfer_order_lines_transfer_orders_transfer_order_id",
                table: "transfer_order_lines",
                column: "transfer_order_id",
                principalTable: "transfer_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_transfer_order_lines_warehouse_locations_from_location_id",
                table: "transfer_order_lines",
                column: "from_location_id",
                principalTable: "warehouse_locations",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_transfer_order_lines_warehouse_locations_to_location_id",
                table: "transfer_order_lines",
                column: "to_location_id",
                principalTable: "warehouse_locations",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_transfer_orders_warehouses_from_warehouse_id",
                table: "transfer_orders",
                column: "from_warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_transfer_orders_warehouses_to_warehouse_id",
                table: "transfer_orders",
                column: "to_warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_app_users_user_id",
                table: "user_roles",
                column: "user_id",
                principalTable: "app_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vendor_addresses_vendors_vendor_id",
                table: "vendor_addresses",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vendor_contacts_vendors_vendor_id",
                table: "vendor_contacts",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vendor_credit_notes_ap_invoices_ap_invoice_id",
                table: "vendor_credit_notes",
                column: "ap_invoice_id",
                principalTable: "ap_invoices",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_vendor_credit_notes_vendors_vendor_id",
                table: "vendor_credit_notes",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_warehouse_inventory_balances_product_variants_product_varia",
                table: "warehouse_inventory_balances",
                column: "product_variant_id",
                principalTable: "product_variants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_warehouse_inventory_balances_warehouse_locations_warehouse_",
                table: "warehouse_inventory_balances",
                column: "warehouse_location_id",
                principalTable: "warehouse_locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_warehouse_inventory_balances_warehouses_warehouse_id",
                table: "warehouse_inventory_balances",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_warehouse_locations_warehouses_warehouse_id",
                table: "warehouse_locations",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_warehouses_operational_sites_site_id",
                table: "warehouses",
                column: "site_id",
                principalTable: "operational_sites",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_warehouses_warehouse_types_warehouse_type_id",
                table: "warehouses",
                column: "warehouse_type_id",
                principalTable: "warehouse_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_workflow_approval_steps_workflow_instances_workflow_instanc",
                table: "workflow_approval_steps",
                column: "workflow_instance_id",
                principalTable: "workflow_instances",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_workflow_instances_workflow_templates_template_id",
                table: "workflow_instances",
                column: "template_id",
                principalTable: "workflow_templates",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_workflow_template_steps_workflow_templates_workflow_templat",
                table: "workflow_template_steps",
                column: "workflow_template_id",
                principalTable: "workflow_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_accounts_account_types_account_type_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_accounts_accounts_parent_account_id",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_ap_invoices_ap_invoices_linked_prepayment_invoice_id",
                table: "ap_invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_ap_invoices_purchase_orders_purchase_order_id",
                table: "ap_invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_ap_invoices_vendors_vendor_id",
                table: "ap_invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_ap_payments_ap_invoices_ap_invoice_id",
                table: "ap_payments");

            migrationBuilder.DropForeignKey(
                name: "fk_ap_payments_vendors_vendor_id",
                table: "ap_payments");

            migrationBuilder.DropForeignKey(
                name: "fk_ar_invoices_customers_customer_id",
                table: "ar_invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_ar_invoices_sales_orders_sales_order_id",
                table: "ar_invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_ar_payments_ar_invoices_ar_invoice_id",
                table: "ar_payments");

            migrationBuilder.DropForeignKey(
                name: "fk_ar_payments_customers_customer_id",
                table: "ar_payments");

            migrationBuilder.DropForeignKey(
                name: "fk_asset_depreciations_fixed_assets_asset_id",
                table: "asset_depreciations");

            migrationBuilder.DropForeignKey(
                name: "fk_asset_disposals_fixed_assets_asset_id",
                table: "asset_disposals");

            migrationBuilder.DropForeignKey(
                name: "fk_asset_maintenances_fixed_assets_asset_id",
                table: "asset_maintenances");

            migrationBuilder.DropForeignKey(
                name: "fk_asset_transfers_fixed_assets_asset_id",
                table: "asset_transfers");

            migrationBuilder.DropForeignKey(
                name: "fk_bank_reconciliations_bank_accounts_bank_account_id",
                table: "bank_reconciliations");

            migrationBuilder.DropForeignKey(
                name: "fk_bank_transactions_bank_accounts_bank_account_id",
                table: "bank_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_cash_journal_lines_cash_journals_journal_id",
                table: "cash_journal_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_cash_journals_bank_accounts_bank_account_id",
                table: "cash_journals");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_products_brands_brand_id",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_products_categories_category_id",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_products_vendors_preferred_vendor_id",
                table: "catalog_products");

            migrationBuilder.DropForeignKey(
                name: "fk_categories_categories_parent_category_id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "fk_coupons_promotions_promotion_id",
                table: "coupons");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_addresses_customers_customer_id",
                table: "customer_addresses");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_contacts_customers_customer_id",
                table: "customer_contacts");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_credit_notes_ar_invoices_ar_invoice_id",
                table: "customer_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_credit_notes_customers_customer_id",
                table: "customer_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_loyalty_accounts_loyalty_programs_loyalty_program_",
                table: "customer_loyalty_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_dunning_records_ar_invoices_ar_invoice_id",
                table: "dunning_records");

            migrationBuilder.DropForeignKey(
                name: "fk_dunning_records_customers_customer_id",
                table: "dunning_records");

            migrationBuilder.DropForeignKey(
                name: "fk_expense_lines_expense_reports_expense_report_id",
                table: "expense_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_fiscal_periods_fiscal_years_fiscal_year_id",
                table: "fiscal_periods");

            migrationBuilder.DropForeignKey(
                name: "fk_import_job_rows_import_jobs_import_job_id",
                table: "import_job_rows");

            migrationBuilder.DropForeignKey(
                name: "fk_inbound_order_lines_inbound_orders_inbound_order_id",
                table: "inbound_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_inbound_order_lines_warehouse_locations_location_id",
                table: "inbound_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_inbound_orders_warehouses_warehouse_id",
                table: "inbound_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_inventory_records_product_variants_product_variant_id",
                table: "inventory_records");

            migrationBuilder.DropForeignKey(
                name: "fk_inventory_transactions_product_variants_product_variant_id",
                table: "inventory_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_journal_entries_fiscal_periods_fiscal_period_id",
                table: "journal_entries");

            migrationBuilder.DropForeignKey(
                name: "fk_journal_lines_accounts_account_id",
                table: "journal_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_journal_lines_journal_entries_journal_entry_id",
                table: "journal_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_outbound_order_lines_outbound_orders_outbound_order_id",
                table: "outbound_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_outbound_order_lines_warehouse_locations_from_location_id",
                table: "outbound_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_outbound_orders_warehouses_warehouse_id",
                table: "outbound_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_payment_proposal_lines_ap_invoices_ap_invoice_id",
                table: "payment_proposal_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_payment_proposal_lines_ap_payments_ap_payment_id",
                table: "payment_proposal_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_payment_proposal_lines_payment_proposals_proposal_id",
                table: "payment_proposal_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_pos_payments_pos_transactions_pos_transaction_id",
                table: "pos_payments");

            migrationBuilder.DropForeignKey(
                name: "fk_pos_transaction_lines_pos_transactions_pos_transaction_id",
                table: "pos_transaction_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_price_agreements_catalog_products_product_id",
                table: "price_agreements");

            migrationBuilder.DropForeignKey(
                name: "fk_price_agreements_product_variants_variant_id",
                table: "price_agreements");

            migrationBuilder.DropForeignKey(
                name: "fk_product_variants_catalog_products_product_id",
                table: "product_variants");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_lines_product_variants_product_variant_id",
                table: "purchase_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_lines_purchase_orders_purchase_order_id",
                table: "purchase_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_receipt_lines_purchase_order_lines_purchase_",
                table: "purchase_order_receipt_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_receipt_lines_purchase_order_receipts_receip",
                table: "purchase_order_receipt_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_receipts_purchase_orders_purchase_order_id",
                table: "purchase_order_receipts");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_receipts_warehouse_locations_warehouse_locat",
                table: "purchase_order_receipts");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_order_receipts_warehouses_warehouse_id",
                table: "purchase_order_receipts");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_orders_vendors_vendor_id",
                table: "purchase_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_orders_warehouses_warehouse_id",
                table: "purchase_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_purchase_requisition_lines_purchase_requisitions_requisitio",
                table: "purchase_requisition_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_retail_transaction_staging_lines_retail_transaction_staging",
                table: "retail_transaction_staging_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_retail_transaction_staging_tenders_retail_transaction_stagi",
                table: "retail_transaction_staging_tenders");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_roles_role_id",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_sales_order_lines_product_variants_product_variant_id",
                table: "sales_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_sales_order_lines_sales_orders_sales_order_id",
                table: "sales_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_sales_orders_customers_customer_id",
                table: "sales_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_sales_quotation_lines_sales_quotations_quotation_id",
                table: "sales_quotation_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_sales_quotations_customers_customer_id",
                table: "sales_quotations");

            migrationBuilder.DropForeignKey(
                name: "fk_transfer_order_lines_transfer_orders_transfer_order_id",
                table: "transfer_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_transfer_order_lines_warehouse_locations_from_location_id",
                table: "transfer_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_transfer_order_lines_warehouse_locations_to_location_id",
                table: "transfer_order_lines");

            migrationBuilder.DropForeignKey(
                name: "fk_transfer_orders_warehouses_from_warehouse_id",
                table: "transfer_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_transfer_orders_warehouses_to_warehouse_id",
                table: "transfer_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_app_users_user_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_vendor_addresses_vendors_vendor_id",
                table: "vendor_addresses");

            migrationBuilder.DropForeignKey(
                name: "fk_vendor_contacts_vendors_vendor_id",
                table: "vendor_contacts");

            migrationBuilder.DropForeignKey(
                name: "fk_vendor_credit_notes_ap_invoices_ap_invoice_id",
                table: "vendor_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "fk_vendor_credit_notes_vendors_vendor_id",
                table: "vendor_credit_notes");

            migrationBuilder.DropForeignKey(
                name: "fk_warehouse_inventory_balances_product_variants_product_varia",
                table: "warehouse_inventory_balances");

            migrationBuilder.DropForeignKey(
                name: "fk_warehouse_inventory_balances_warehouse_locations_warehouse_",
                table: "warehouse_inventory_balances");

            migrationBuilder.DropForeignKey(
                name: "fk_warehouse_inventory_balances_warehouses_warehouse_id",
                table: "warehouse_inventory_balances");

            migrationBuilder.DropForeignKey(
                name: "fk_warehouse_locations_warehouses_warehouse_id",
                table: "warehouse_locations");

            migrationBuilder.DropForeignKey(
                name: "fk_warehouses_operational_sites_site_id",
                table: "warehouses");

            migrationBuilder.DropForeignKey(
                name: "fk_warehouses_warehouse_types_warehouse_type_id",
                table: "warehouses");

            migrationBuilder.DropForeignKey(
                name: "fk_workflow_approval_steps_workflow_instances_workflow_instanc",
                table: "workflow_approval_steps");

            migrationBuilder.DropForeignKey(
                name: "fk_workflow_instances_workflow_templates_template_id",
                table: "workflow_instances");

            migrationBuilder.DropForeignKey(
                name: "fk_workflow_template_steps_workflow_templates_workflow_templat",
                table: "workflow_template_steps");

            migrationBuilder.DropPrimaryKey(
                name: "pk_workflow_templates",
                table: "workflow_templates");

            migrationBuilder.DropPrimaryKey(
                name: "pk_workflow_template_steps",
                table: "workflow_template_steps");

            migrationBuilder.DropPrimaryKey(
                name: "pk_workflow_instances",
                table: "workflow_instances");

            migrationBuilder.DropPrimaryKey(
                name: "pk_workflow_approval_steps",
                table: "workflow_approval_steps");

            migrationBuilder.DropPrimaryKey(
                name: "pk_warehouses",
                table: "warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_vendors",
                table: "vendors");

            migrationBuilder.DropPrimaryKey(
                name: "pk_vendor_credit_notes",
                table: "vendor_credit_notes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_vendor_contacts",
                table: "vendor_contacts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_vendor_addresses",
                table: "vendor_addresses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sales_quotations",
                table: "sales_quotations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sales_quotation_lines",
                table: "sales_quotation_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sales_orders",
                table: "sales_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sales_order_lines",
                table: "sales_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permissions",
                table: "role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_retail_transaction_staging_tenders",
                table: "retail_transaction_staging_tenders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_retail_transaction_staging_lines",
                table: "retail_transaction_staging_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_retail_transaction_staging",
                table: "retail_transaction_staging");

            migrationBuilder.DropPrimaryKey(
                name: "pk_retail_tender_settlements",
                table: "retail_tender_settlements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_retail_stores",
                table: "retail_stores");

            migrationBuilder.DropPrimaryKey(
                name: "pk_retail_statements",
                table: "retail_statements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_purchase_requisitions",
                table: "purchase_requisitions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_purchase_requisition_lines",
                table: "purchase_requisition_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_purchase_orders",
                table: "purchase_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_purchase_order_receipts",
                table: "purchase_order_receipts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_purchase_order_receipt_lines",
                table: "purchase_order_receipt_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_purchase_order_lines",
                table: "purchase_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_promotions",
                table: "promotions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_product_variants",
                table: "product_variants");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pos_transactions",
                table: "pos_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pos_transaction_lines",
                table: "pos_transaction_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pos_payments",
                table: "pos_payments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_payment_proposals",
                table: "payment_proposals");

            migrationBuilder.DropPrimaryKey(
                name: "pk_payment_proposal_lines",
                table: "payment_proposal_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_organizations",
                table: "organizations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_journal_lines",
                table: "journal_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_journal_entries",
                table: "journal_entries");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inventory_transactions",
                table: "inventory_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inventory_records",
                table: "inventory_records");

            migrationBuilder.DropPrimaryKey(
                name: "pk_import_jobs",
                table: "import_jobs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_import_job_rows",
                table: "import_job_rows");

            migrationBuilder.DropPrimaryKey(
                name: "pk_fixed_assets",
                table: "fixed_assets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_fiscal_years",
                table: "fiscal_years");

            migrationBuilder.DropPrimaryKey(
                name: "pk_fiscal_periods",
                table: "fiscal_periods");

            migrationBuilder.DropPrimaryKey(
                name: "pk_export_job_rows",
                table: "export_job_rows");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expense_reports",
                table: "expense_reports");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expense_lines",
                table: "expense_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_expense_categories",
                table: "expense_categories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_dunning_records",
                table: "dunning_records");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_credit_notes",
                table: "customer_credit_notes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_contacts",
                table: "customer_contacts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_addresses",
                table: "customer_addresses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_currencies",
                table: "currencies");

            migrationBuilder.DropPrimaryKey(
                name: "pk_coupons",
                table: "coupons");

            migrationBuilder.DropPrimaryKey(
                name: "pk_coupon_redemptions",
                table: "coupon_redemptions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_catalog_products",
                table: "catalog_products");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cash_journals",
                table: "cash_journals");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cash_journal_lines",
                table: "cash_journal_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_campaigns",
                table: "campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "pk_brands",
                table: "brands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_batch_job_configs",
                table: "batch_job_configs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_transactions",
                table: "bank_transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_reconciliations",
                table: "bank_reconciliations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_accounts",
                table: "bank_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_audit_logs",
                table: "audit_logs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asset_transfers",
                table: "asset_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asset_maintenances",
                table: "asset_maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asset_disposals",
                table: "asset_disposals");

            migrationBuilder.DropPrimaryKey(
                name: "pk_asset_depreciations",
                table: "asset_depreciations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ar_payments",
                table: "ar_payments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ar_invoices",
                table: "ar_invoices");

            migrationBuilder.DropPrimaryKey(
                name: "pk_app_users",
                table: "app_users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ap_payments",
                table: "ap_payments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ap_invoices",
                table: "ap_invoices");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_account_types",
                table: "account_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_warehouse_types",
                table: "warehouse_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_warehouse_locations",
                table: "warehouse_locations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_warehouse_inventory_balances",
                table: "warehouse_inventory_balances");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transfer_orders",
                table: "transfer_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transfer_order_lines",
                table: "transfer_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_price_agreements",
                table: "price_agreements");

            migrationBuilder.DropPrimaryKey(
                name: "pk_outbound_orders",
                table: "outbound_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_outbound_order_lines",
                table: "outbound_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_operational_sites",
                table: "operational_sites");

            migrationBuilder.DropPrimaryKey(
                name: "pk_loyalty_programs",
                table: "loyalty_programs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbound_orders",
                table: "inbound_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbound_order_lines",
                table: "inbound_order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_loyalty_accounts",
                table: "customer_loyalty_accounts");

            migrationBuilder.RenameTable(
                name: "warehouses",
                newName: "Warehouses");

            migrationBuilder.RenameTable(
                name: "campaigns",
                newName: "Campaigns");

            migrationBuilder.RenameTable(
                name: "warehouse_types",
                newName: "WarehouseTypes");

            migrationBuilder.RenameTable(
                name: "warehouse_locations",
                newName: "WarehouseLocations");

            migrationBuilder.RenameTable(
                name: "warehouse_inventory_balances",
                newName: "WarehouseInventoryBalances");

            migrationBuilder.RenameTable(
                name: "transfer_orders",
                newName: "TransferOrders");

            migrationBuilder.RenameTable(
                name: "transfer_order_lines",
                newName: "TransferOrderLines");

            migrationBuilder.RenameTable(
                name: "price_agreements",
                newName: "PriceAgreements");

            migrationBuilder.RenameTable(
                name: "outbound_orders",
                newName: "OutboundOrders");

            migrationBuilder.RenameTable(
                name: "outbound_order_lines",
                newName: "OutboundOrderLines");

            migrationBuilder.RenameTable(
                name: "operational_sites",
                newName: "OperationalSites");

            migrationBuilder.RenameTable(
                name: "loyalty_programs",
                newName: "LoyaltyPrograms");

            migrationBuilder.RenameTable(
                name: "inbound_orders",
                newName: "InboundOrders");

            migrationBuilder.RenameTable(
                name: "inbound_order_lines",
                newName: "InboundOrderLines");

            migrationBuilder.RenameTable(
                name: "customer_loyalty_accounts",
                newName: "CustomerLoyaltyAccounts");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "workflow_templates",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workflow_templates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "workflow_templates",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "workflow_templates",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "workflow_templates",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "workflow_templates",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "document_type",
                table: "workflow_templates",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "workflow_templates",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "amount_threshold",
                table: "workflow_templates",
                newName: "AmountThreshold");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "workflow_template_steps",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workflow_template_steps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_template_id",
                table: "workflow_template_steps",
                newName: "WorkflowTemplateId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "workflow_template_steps",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "step_order",
                table: "workflow_template_steps",
                newName: "StepOrder");

            migrationBuilder.RenameColumn(
                name: "step_name",
                table: "workflow_template_steps",
                newName: "StepName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "workflow_template_steps",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "workflow_template_steps",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "approver_user_id",
                table: "workflow_template_steps",
                newName: "ApproverUserId");

            migrationBuilder.RenameColumn(
                name: "approver_role",
                table: "workflow_template_steps",
                newName: "ApproverRole");

            migrationBuilder.RenameIndex(
                name: "ix_workflow_template_steps_workflow_template_id",
                table: "workflow_template_steps",
                newName: "IX_workflow_template_steps_WorkflowTemplateId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "workflow_instances",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "comments",
                table: "workflow_instances",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workflow_instances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "workflow_instances",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_steps",
                table: "workflow_instances",
                newName: "TotalSteps");

            migrationBuilder.RenameColumn(
                name: "template_id",
                table: "workflow_instances",
                newName: "TemplateId");

            migrationBuilder.RenameColumn(
                name: "submitted_by",
                table: "workflow_instances",
                newName: "SubmittedBy");

            migrationBuilder.RenameColumn(
                name: "rejected_reason",
                table: "workflow_instances",
                newName: "RejectedReason");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "workflow_instances",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "workflow_instances",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "document_type",
                table: "workflow_instances",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "document_ref",
                table: "workflow_instances",
                newName: "DocumentRef");

            migrationBuilder.RenameColumn(
                name: "document_id",
                table: "workflow_instances",
                newName: "DocumentId");

            migrationBuilder.RenameColumn(
                name: "document_amount",
                table: "workflow_instances",
                newName: "DocumentAmount");

            migrationBuilder.RenameColumn(
                name: "current_step_index",
                table: "workflow_instances",
                newName: "CurrentStepIndex");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "workflow_instances",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "completed_at",
                table: "workflow_instances",
                newName: "CompletedAt");

            migrationBuilder.RenameIndex(
                name: "ix_workflow_instances_template_id",
                table: "workflow_instances",
                newName: "IX_workflow_instances_TemplateId");

            migrationBuilder.RenameIndex(
                name: "ix_workflow_instances_organization_id_status",
                table: "workflow_instances",
                newName: "IX_workflow_instances_OrganizationId_Status");

            migrationBuilder.RenameIndex(
                name: "ix_workflow_instances_organization_id_document_type_document_id",
                table: "workflow_instances",
                newName: "IX_workflow_instances_OrganizationId_DocumentType_DocumentId");

            migrationBuilder.RenameColumn(
                name: "decision",
                table: "workflow_approval_steps",
                newName: "Decision");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workflow_approval_steps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "workflow_approval_steps",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "workflow_approval_steps",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "step_order",
                table: "workflow_approval_steps",
                newName: "StepOrder");

            migrationBuilder.RenameColumn(
                name: "step_name",
                table: "workflow_approval_steps",
                newName: "StepName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "workflow_approval_steps",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "workflow_approval_steps",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "approver_user_id",
                table: "workflow_approval_steps",
                newName: "ApproverUserId");

            migrationBuilder.RenameColumn(
                name: "approver_role",
                table: "workflow_approval_steps",
                newName: "ApproverRole");

            migrationBuilder.RenameColumn(
                name: "acted_by_comments",
                table: "workflow_approval_steps",
                newName: "ActedByComments");

            migrationBuilder.RenameColumn(
                name: "acted_by",
                table: "workflow_approval_steps",
                newName: "ActedBy");

            migrationBuilder.RenameColumn(
                name: "acted_at",
                table: "workflow_approval_steps",
                newName: "ActedAt");

            migrationBuilder.RenameIndex(
                name: "ix_workflow_approval_steps_workflow_instance_id",
                table: "workflow_approval_steps",
                newName: "IX_workflow_approval_steps_WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Warehouses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Warehouses",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Warehouses",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "Warehouses",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Warehouses",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Warehouses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "warehouse_type_id",
                table: "Warehouses",
                newName: "WarehouseTypeId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Warehouses",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "site_id",
                table: "Warehouses",
                newName: "SiteId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "Warehouses",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Warehouses",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_default",
                table: "Warehouses",
                newName: "IsDefault");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Warehouses",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Warehouses",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_warehouses_warehouse_type_id",
                table: "Warehouses",
                newName: "IX_Warehouses_WarehouseTypeId");

            migrationBuilder.RenameIndex(
                name: "ix_warehouses_site_id",
                table: "Warehouses",
                newName: "IX_Warehouses_SiteId");

            migrationBuilder.RenameIndex(
                name: "ix_warehouses_organization_id_code",
                table: "Warehouses",
                newName: "IX_Warehouses_OrganizationId_Code");

            migrationBuilder.RenameColumn(
                name: "website",
                table: "vendors",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "vendors",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "vendors",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "vendors",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "vendors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "vendors",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "vendors",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "vendors",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "vendors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendor_number",
                table: "vendors",
                newName: "VendorNumber");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vendors",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tax_id",
                table: "vendors",
                newName: "TaxId");

            migrationBuilder.RenameColumn(
                name: "shipping_address",
                table: "vendors",
                newName: "ShippingAddress");

            migrationBuilder.RenameColumn(
                name: "payment_terms_days",
                table: "vendors",
                newName: "PaymentTermsDays");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "vendors",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_exported",
                table: "vendors",
                newName: "IsExported");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "vendors",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "exported_at",
                table: "vendors",
                newName: "ExportedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "vendors",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "billing_address",
                table: "vendors",
                newName: "BillingAddress");

            migrationBuilder.RenameColumn(
                name: "bank_routing_number",
                table: "vendors",
                newName: "BankRoutingNumber");

            migrationBuilder.RenameColumn(
                name: "bank_account_number",
                table: "vendors",
                newName: "BankAccountNumber");

            migrationBuilder.RenameColumn(
                name: "bank_account_name",
                table: "vendors",
                newName: "BankAccountName");

            migrationBuilder.RenameIndex(
                name: "ix_vendors_organization_id_vendor_number",
                table: "vendors",
                newName: "IX_vendors_OrganizationId_VendorNumber");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "vendor_credit_notes",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "vendor_credit_notes",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "vendor_credit_notes",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "vendor_credit_notes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "vendor_credit_notes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "vendor_credit_notes",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "vendor_cn_ref",
                table: "vendor_credit_notes",
                newName: "VendorCNRef");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vendor_credit_notes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "vendor_credit_notes",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "vendor_credit_notes",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "vendor_credit_notes",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "purchase_order_id",
                table: "vendor_credit_notes",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "vendor_credit_notes",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "vendor_credit_notes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "credit_note_number",
                table: "vendor_credit_notes",
                newName: "CreditNoteNumber");

            migrationBuilder.RenameColumn(
                name: "credit_date",
                table: "vendor_credit_notes",
                newName: "CreditDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "vendor_credit_notes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "applied_amount",
                table: "vendor_credit_notes",
                newName: "AppliedAmount");

            migrationBuilder.RenameColumn(
                name: "ap_invoice_id",
                table: "vendor_credit_notes",
                newName: "APInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_vendor_credit_notes_vendor_id",
                table: "vendor_credit_notes",
                newName: "IX_vendor_credit_notes_VendorId");

            migrationBuilder.RenameIndex(
                name: "ix_vendor_credit_notes_organization_id_credit_note_number",
                table: "vendor_credit_notes",
                newName: "IX_vendor_credit_notes_OrganizationId_CreditNoteNumber");

            migrationBuilder.RenameIndex(
                name: "ix_vendor_credit_notes_ap_invoice_id",
                table: "vendor_credit_notes",
                newName: "IX_vendor_credit_notes_APInvoiceId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "vendor_contacts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "vendor_contacts",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "vendor_contacts",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "vendor_contacts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "mobile",
                table: "vendor_contacts",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "vendor_contacts",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "vendor_contacts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "vendor_contacts",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vendor_contacts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "vendor_contacts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_primary",
                table: "vendor_contacts",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "vendor_contacts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "vendor_contacts",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_vendor_contacts_vendor_id",
                table: "vendor_contacts",
                newName: "IX_vendor_contacts_VendorId");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "vendor_addresses",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "line2",
                table: "vendor_addresses",
                newName: "Line2");

            migrationBuilder.RenameColumn(
                name: "line1",
                table: "vendor_addresses",
                newName: "Line1");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "vendor_addresses",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "vendor_addresses",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "vendor_addresses",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "vendor_addresses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "vendor_addresses",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vendor_addresses",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "postal_code",
                table: "vendor_addresses",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "vendor_addresses",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_primary",
                table: "vendor_addresses",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "vendor_addresses",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "vendor_addresses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "address_type",
                table: "vendor_addresses",
                newName: "AddressType");

            migrationBuilder.RenameIndex(
                name: "ix_vendor_addresses_vendor_id",
                table: "vendor_addresses",
                newName: "IX_vendor_addresses_VendorId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_roles",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "user_roles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "user_roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "user_roles",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "user_roles",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_user_id_role_id",
                table: "user_roles",
                newName: "IX_user_roles_UserId_RoleId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                newName: "IX_user_roles_RoleId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "sales_quotations",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "sales_quotations",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "sales_quotations",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "sales_quotations",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sales_quotations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "sales_quotations",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "valid_until",
                table: "sales_quotations",
                newName: "ValidUntil");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "sales_quotations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tax_total",
                table: "sales_quotations",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "sales_quotations",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "rejection_reason",
                table: "sales_quotations",
                newName: "RejectionReason");

            migrationBuilder.RenameColumn(
                name: "quotation_number",
                table: "sales_quotations",
                newName: "QuotationNumber");

            migrationBuilder.RenameColumn(
                name: "quotation_date",
                table: "sales_quotations",
                newName: "QuotationDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "sales_quotations",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "sales_quotations",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "sales_quotations",
                newName: "GrandTotal");

            migrationBuilder.RenameColumn(
                name: "discount_total",
                table: "sales_quotations",
                newName: "DiscountTotal");

            migrationBuilder.RenameColumn(
                name: "customer_ref",
                table: "sales_quotations",
                newName: "CustomerRef");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "sales_quotations",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sales_quotations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "converted_to_so_id",
                table: "sales_quotations",
                newName: "ConvertedToSOId");

            migrationBuilder.RenameColumn(
                name: "converted_at",
                table: "sales_quotations",
                newName: "ConvertedAt");

            migrationBuilder.RenameIndex(
                name: "ix_sales_quotations_organization_id_quotation_number",
                table: "sales_quotations",
                newName: "IX_sales_quotations_OrganizationId_QuotationNumber");

            migrationBuilder.RenameIndex(
                name: "ix_sales_quotations_customer_id",
                table: "sales_quotations",
                newName: "IX_sales_quotations_CustomerId");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "sales_quotation_lines",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "sales_quotation_lines",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sales_quotation_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "variant_description",
                table: "sales_quotation_lines",
                newName: "VariantDescription");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "sales_quotation_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "sales_quotation_lines",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "sales_quotation_lines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "tax_rate",
                table: "sales_quotation_lines",
                newName: "TaxRate");

            migrationBuilder.RenameColumn(
                name: "quotation_id",
                table: "sales_quotation_lines",
                newName: "QuotationId");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "sales_quotation_lines",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "sales_quotation_lines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "line_number",
                table: "sales_quotation_lines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "sales_quotation_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "discount_pct",
                table: "sales_quotation_lines",
                newName: "DiscountPct");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sales_quotation_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_sales_quotation_lines_quotation_id",
                table: "sales_quotation_lines",
                newName: "IX_sales_quotation_lines_QuotationId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "sales_orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "sales_orders",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "sales_orders",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sales_orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "sales_orders",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "sales_orders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tax_total",
                table: "sales_orders",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "sales_orders",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "requested_ship_date",
                table: "sales_orders",
                newName: "RequestedShipDate");

            migrationBuilder.RenameColumn(
                name: "rejection_reason",
                table: "sales_orders",
                newName: "RejectionReason");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "sales_orders",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "sales_orders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "order_date",
                table: "sales_orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "is_exported",
                table: "sales_orders",
                newName: "IsExported");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "sales_orders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "sales_orders",
                newName: "GrandTotal");

            migrationBuilder.RenameColumn(
                name: "exported_at",
                table: "sales_orders",
                newName: "ExportedAt");

            migrationBuilder.RenameColumn(
                name: "discount_total",
                table: "sales_orders",
                newName: "DiscountTotal");

            migrationBuilder.RenameColumn(
                name: "delivery_reference",
                table: "sales_orders",
                newName: "DeliveryReference");

            migrationBuilder.RenameColumn(
                name: "delivered_at",
                table: "sales_orders",
                newName: "DeliveredAt");

            migrationBuilder.RenameColumn(
                name: "customer_ref",
                table: "sales_orders",
                newName: "CustomerRef");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "sales_orders",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sales_orders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "sales_orders",
                newName: "ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "actual_ship_date",
                table: "sales_orders",
                newName: "ActualShipDate");

            migrationBuilder.RenameIndex(
                name: "ix_sales_orders_organization_id_order_number",
                table: "sales_orders",
                newName: "IX_sales_orders_OrganizationId_OrderNumber");

            migrationBuilder.RenameIndex(
                name: "ix_sales_orders_customer_id",
                table: "sales_orders",
                newName: "IX_sales_orders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "sales_order_lines",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "sales_order_lines",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sales_order_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "variant_description",
                table: "sales_order_lines",
                newName: "VariantDescription");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "sales_order_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "sales_order_lines",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "sales_order_lines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "tax_rate",
                table: "sales_order_lines",
                newName: "TaxRate");

            migrationBuilder.RenameColumn(
                name: "sales_order_id",
                table: "sales_order_lines",
                newName: "SalesOrderId");

            migrationBuilder.RenameColumn(
                name: "quantity_shipped",
                table: "sales_order_lines",
                newName: "QuantityShipped");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "sales_order_lines",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "sales_order_lines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "sales_order_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "discount_pct",
                table: "sales_order_lines",
                newName: "DiscountPct");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sales_order_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_sales_order_lines_sales_order_id",
                table: "sales_order_lines",
                newName: "IX_sales_order_lines_SalesOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_sales_order_lines_product_variant_id",
                table: "sales_order_lines",
                newName: "IX_sales_order_lines_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "roles",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "roles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "roles",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_system_role",
                table: "roles",
                newName: "IsSystemRole");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "roles",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "roles",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_roles_organization_id_name",
                table: "roles",
                newName: "IX_roles_OrganizationId_Name");

            migrationBuilder.RenameColumn(
                name: "module",
                table: "role_permissions",
                newName: "Module");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "role_permissions",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "role_permissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "role_permissions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "role_permissions",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "role_permissions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "role_permissions",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_role_permissions_role_id_module_action",
                table: "role_permissions",
                newName: "IX_role_permissions_RoleId_Module_Action");

            migrationBuilder.RenameColumn(
                name: "sequence",
                table: "retail_transaction_staging_tenders",
                newName: "Sequence");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "retail_transaction_staging_tenders",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "retail_transaction_staging_tenders",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "retail_transaction_staging_tenders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "retail_transaction_staging_tenders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "retail_transaction_staging_id",
                table: "retail_transaction_staging_tenders",
                newName: "RetailTransactionStagingId");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "retail_transaction_staging_tenders",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "retail_transaction_staging_tenders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "retail_transaction_staging_tenders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_retail_transaction_staging_tenders_retail_transaction_stagi",
                table: "retail_transaction_staging_tenders",
                newName: "IX_retail_transaction_staging_tenders_RetailTransactionStaging~");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "retail_transaction_staging_lines",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "retail_transaction_staging_lines",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "retail_transaction_staging_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "validation_message",
                table: "retail_transaction_staging_lines",
                newName: "ValidationMessage");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "retail_transaction_staging_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "retail_transaction_staging_lines",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "retail_transaction_staging_lines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "retail_transaction_staging_lines",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "retail_transaction_staging_id",
                table: "retail_transaction_staging_lines",
                newName: "RetailTransactionStagingId");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "retail_transaction_staging_lines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "pos_item_id",
                table: "retail_transaction_staging_lines",
                newName: "PosItemId");

            migrationBuilder.RenameColumn(
                name: "matched_product_variant_id",
                table: "retail_transaction_staging_lines",
                newName: "MatchedProductVariantId");

            migrationBuilder.RenameColumn(
                name: "line_total",
                table: "retail_transaction_staging_lines",
                newName: "LineTotal");

            migrationBuilder.RenameColumn(
                name: "line_sub_total",
                table: "retail_transaction_staging_lines",
                newName: "LineSubTotal");

            migrationBuilder.RenameColumn(
                name: "line_number",
                table: "retail_transaction_staging_lines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "is_return",
                table: "retail_transaction_staging_lines",
                newName: "IsReturn");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "retail_transaction_staging_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "retail_transaction_staging_lines",
                newName: "DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "retail_transaction_staging_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_retail_transaction_staging_lines_retail_transaction_staging",
                table: "retail_transaction_staging_lines",
                newName: "IX_retail_transaction_staging_lines_RetailTransactionStagingId~");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "retail_transaction_staging",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "retail_transaction_staging",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "retail_transaction_staging",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "validation_message",
                table: "retail_transaction_staging",
                newName: "ValidationMessage");

            migrationBuilder.RenameColumn(
                name: "validated_at",
                table: "retail_transaction_staging",
                newName: "ValidatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "retail_transaction_staging",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transaction_number",
                table: "retail_transaction_staging",
                newName: "TransactionNumber");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "retail_transaction_staging",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "tax_total",
                table: "retail_transaction_staging",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "retail_transaction_staging",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "store_code",
                table: "retail_transaction_staging",
                newName: "StoreCode");

            migrationBuilder.RenameColumn(
                name: "source_hash",
                table: "retail_transaction_staging",
                newName: "SourceHash");

            migrationBuilder.RenameColumn(
                name: "source_file",
                table: "retail_transaction_staging",
                newName: "SourceFile");

            migrationBuilder.RenameColumn(
                name: "retail_statement_id",
                table: "retail_transaction_staging",
                newName: "RetailStatementId");

            migrationBuilder.RenameColumn(
                name: "raw_xml",
                table: "retail_transaction_staging",
                newName: "RawXml");

            migrationBuilder.RenameColumn(
                name: "promoted_transaction_id",
                table: "retail_transaction_staging",
                newName: "PromotedTransactionId");

            migrationBuilder.RenameColumn(
                name: "promoted_at",
                table: "retail_transaction_staging",
                newName: "PromotedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "retail_transaction_staging",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "operator_id",
                table: "retail_transaction_staging",
                newName: "OperatorId");

            migrationBuilder.RenameColumn(
                name: "is_return",
                table: "retail_transaction_staging",
                newName: "IsReturn");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "retail_transaction_staging",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "retail_transaction_staging",
                newName: "GrandTotal");

            migrationBuilder.RenameColumn(
                name: "discount_total",
                table: "retail_transaction_staging",
                newName: "DiscountTotal");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "retail_transaction_staging",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "business_date",
                table: "retail_transaction_staging",
                newName: "BusinessDate");

            migrationBuilder.RenameIndex(
                name: "ix_retail_transaction_staging_organization_id_transaction_numb",
                table: "retail_transaction_staging",
                newName: "IX_retail_transaction_staging_OrganizationId_TransactionNumber");

            migrationBuilder.RenameIndex(
                name: "ix_retail_transaction_staging_organization_id_status_created_at",
                table: "retail_transaction_staging",
                newName: "IX_retail_transaction_staging_OrganizationId_Status_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_retail_transaction_staging_organization_id_source_hash",
                table: "retail_transaction_staging",
                newName: "IX_retail_transaction_staging_OrganizationId_SourceHash");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "retail_tender_settlements",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "retail_tender_settlements",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "retail_tender_settlements",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "retail_tender_settlements",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "retail_tender_settlements",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "settled_at",
                table: "retail_tender_settlements",
                newName: "SettledAt");

            migrationBuilder.RenameColumn(
                name: "retail_statement_id",
                table: "retail_tender_settlements",
                newName: "RetailStatementId");

            migrationBuilder.RenameColumn(
                name: "processor_reference",
                table: "retail_tender_settlements",
                newName: "ProcessorReference");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "retail_tender_settlements",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "retail_tender_settlements",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "retail_tender_settlements",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "retail_tender_settlements",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bank_transaction_id",
                table: "retail_tender_settlements",
                newName: "BankTransactionId");

            migrationBuilder.RenameIndex(
                name: "ix_retail_tender_settlements_retail_statement_id",
                table: "retail_tender_settlements",
                newName: "IX_retail_tender_settlements_RetailStatementId");

            migrationBuilder.RenameIndex(
                name: "ix_retail_tender_settlements_organization_id_status",
                table: "retail_tender_settlements",
                newName: "IX_retail_tender_settlements_OrganizationId_Status");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "retail_stores",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "retail_stores",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "retail_stores",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "retail_stores",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "retail_stores",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "store_code",
                table: "retail_stores",
                newName: "StoreCode");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "retail_stores",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "manager_name",
                table: "retail_stores",
                newName: "ManagerName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "retail_stores",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "retail_stores",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "retail_stores",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_retail_stores_organization_id_store_code",
                table: "retail_stores",
                newName: "IX_retail_stores_OrganizationId_StoreCode");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "retail_statements",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "retail_statements",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "retail_statements",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "retail_statements",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transaction_count",
                table: "retail_statements",
                newName: "TransactionCount");

            migrationBuilder.RenameColumn(
                name: "tax_total",
                table: "retail_statements",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "store_id",
                table: "retail_statements",
                newName: "StoreId");

            migrationBuilder.RenameColumn(
                name: "statement_number",
                table: "retail_statements",
                newName: "StatementNumber");

            migrationBuilder.RenameColumn(
                name: "posting_error",
                table: "retail_statements",
                newName: "PostingError");

            migrationBuilder.RenameColumn(
                name: "posted_at",
                table: "retail_statements",
                newName: "PostedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "retail_statements",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "net_sales",
                table: "retail_statements",
                newName: "NetSales");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "retail_statements",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "retail_statements",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "retail_statements",
                newName: "GrandTotal");

            migrationBuilder.RenameColumn(
                name: "discount_total",
                table: "retail_statements",
                newName: "DiscountTotal");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "retail_statements",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cost_total",
                table: "retail_statements",
                newName: "CostTotal");

            migrationBuilder.RenameColumn(
                name: "business_date",
                table: "retail_statements",
                newName: "BusinessDate");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "retail_statements",
                newName: "ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "ar_credit_note_id",
                table: "retail_statements",
                newName: "ARCreditNoteId");

            migrationBuilder.RenameIndex(
                name: "ix_retail_statements_organization_id_store_id_business_date_cu",
                table: "retail_statements",
                newName: "IX_retail_statements_OrganizationId_StoreId_BusinessDate_Curre~");

            migrationBuilder.RenameIndex(
                name: "ix_retail_statements_organization_id_statement_number",
                table: "retail_statements",
                newName: "IX_retail_statements_OrganizationId_StatementNumber");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "purchase_requisitions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "purchase_requisitions",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "purchase_requisitions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "purchase_requisitions",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "purchase_requisitions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "requisition_number",
                table: "purchase_requisitions",
                newName: "RequisitionNumber");

            migrationBuilder.RenameColumn(
                name: "requested_by",
                table: "purchase_requisitions",
                newName: "RequestedBy");

            migrationBuilder.RenameColumn(
                name: "rejection_reason",
                table: "purchase_requisitions",
                newName: "RejectionReason");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "purchase_requisitions",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "needed_by_date",
                table: "purchase_requisitions",
                newName: "NeededByDate");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "purchase_requisitions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "department_code",
                table: "purchase_requisitions",
                newName: "DepartmentCode");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase_requisitions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cost_center_code",
                table: "purchase_requisitions",
                newName: "CostCenterCode");

            migrationBuilder.RenameColumn(
                name: "converted_to_po_id",
                table: "purchase_requisitions",
                newName: "ConvertedToPOId");

            migrationBuilder.RenameColumn(
                name: "converted_at",
                table: "purchase_requisitions",
                newName: "ConvertedAt");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_requisitions_organization_id_requisition_number",
                table: "purchase_requisitions",
                newName: "IX_purchase_requisitions_OrganizationId_RequisitionNumber");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "purchase_requisition_lines",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "purchase_requisition_lines",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "purchase_requisition_lines",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "purchase_requisition_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "purchase_requisition_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "purchase_requisition_lines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "suggested_vendor_id",
                table: "purchase_requisition_lines",
                newName: "SuggestedVendorId");

            migrationBuilder.RenameColumn(
                name: "requisition_id",
                table: "purchase_requisition_lines",
                newName: "RequisitionId");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "purchase_requisition_lines",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "line_number",
                table: "purchase_requisition_lines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "purchase_requisition_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "gl_account_code",
                table: "purchase_requisition_lines",
                newName: "GlAccountCode");

            migrationBuilder.RenameColumn(
                name: "estimated_unit_cost",
                table: "purchase_requisition_lines",
                newName: "EstimatedUnitCost");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase_requisition_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_requisition_lines_requisition_id",
                table: "purchase_requisition_lines",
                newName: "IX_purchase_requisition_lines_RequisitionId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "purchase_orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "purchase_orders",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "purchase_orders",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "purchase_orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "purchase_orders",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "purchase_orders",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "purchase_orders",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "purchase_orders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tax_total",
                table: "purchase_orders",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "purchase_orders",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "rejection_reason",
                table: "purchase_orders",
                newName: "RejectionReason");

            migrationBuilder.RenameColumn(
                name: "po_number",
                table: "purchase_orders",
                newName: "PONumber");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "purchase_orders",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "order_date",
                table: "purchase_orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "is_exported",
                table: "purchase_orders",
                newName: "IsExported");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "purchase_orders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "invoiced_amount",
                table: "purchase_orders",
                newName: "InvoicedAmount");

            migrationBuilder.RenameColumn(
                name: "invoice_status",
                table: "purchase_orders",
                newName: "InvoiceStatus");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "purchase_orders",
                newName: "GrandTotal");

            migrationBuilder.RenameColumn(
                name: "exported_at",
                table: "purchase_orders",
                newName: "ExportedAt");

            migrationBuilder.RenameColumn(
                name: "expected_date",
                table: "purchase_orders",
                newName: "ExpectedDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase_orders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_orders_warehouse_id",
                table: "purchase_orders",
                newName: "IX_purchase_orders_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_orders_vendor_id",
                table: "purchase_orders",
                newName: "IX_purchase_orders_VendorId");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_orders_organization_id_po_number",
                table: "purchase_orders",
                newName: "IX_purchase_orders_OrganizationId_PONumber");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "purchase_order_receipts",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "purchase_order_receipts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "warehouse_location_id",
                table: "purchase_order_receipts",
                newName: "WarehouseLocationId");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "purchase_order_receipts",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "purchase_order_receipts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "received_date",
                table: "purchase_order_receipts",
                newName: "ReceivedDate");

            migrationBuilder.RenameColumn(
                name: "receipt_number",
                table: "purchase_order_receipts",
                newName: "ReceiptNumber");

            migrationBuilder.RenameColumn(
                name: "purchase_order_id",
                table: "purchase_order_receipts",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "purchase_order_receipts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "purchase_order_receipts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase_order_receipts",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_receipts_warehouse_location_id",
                table: "purchase_order_receipts",
                newName: "IX_purchase_order_receipts_WarehouseLocationId");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_receipts_warehouse_id",
                table: "purchase_order_receipts",
                newName: "IX_purchase_order_receipts_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_receipts_receipt_number",
                table: "purchase_order_receipts",
                newName: "IX_purchase_order_receipts_ReceiptNumber");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_receipts_purchase_order_id",
                table: "purchase_order_receipts",
                newName: "IX_purchase_order_receipts_PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "qty",
                table: "purchase_order_receipt_lines",
                newName: "Qty");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "purchase_order_receipt_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "purchase_order_receipt_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "receipt_id",
                table: "purchase_order_receipt_lines",
                newName: "ReceiptId");

            migrationBuilder.RenameColumn(
                name: "purchase_order_line_id",
                table: "purchase_order_receipt_lines",
                newName: "PurchaseOrderLineId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "purchase_order_receipt_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase_order_receipt_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_receipt_lines_receipt_id",
                table: "purchase_order_receipt_lines",
                newName: "IX_purchase_order_receipt_lines_ReceiptId");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_receipt_lines_purchase_order_line_id",
                table: "purchase_order_receipt_lines",
                newName: "IX_purchase_order_receipt_lines_PurchaseOrderLineId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "purchase_order_lines",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "purchase_order_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "purchase_order_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "purchase_order_lines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "unit_cost",
                table: "purchase_order_lines",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "tax_rate",
                table: "purchase_order_lines",
                newName: "TaxRate");

            migrationBuilder.RenameColumn(
                name: "received_qty",
                table: "purchase_order_lines",
                newName: "ReceivedQty");

            migrationBuilder.RenameColumn(
                name: "purchase_order_id",
                table: "purchase_order_lines",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "purchase_order_lines",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "product_code",
                table: "purchase_order_lines",
                newName: "ProductCode");

            migrationBuilder.RenameColumn(
                name: "ordered_qty",
                table: "purchase_order_lines",
                newName: "OrderedQty");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "purchase_order_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "purchase_order_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_lines_purchase_order_id",
                table: "purchase_order_lines",
                newName: "IX_purchase_order_lines_PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_purchase_order_lines_product_variant_id",
                table: "purchase_order_lines",
                newName: "IX_purchase_order_lines_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "promotions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "promotions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "promotions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "promotions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "used_count",
                table: "promotions",
                newName: "UsedCount");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "promotions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "promotions",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "promotions",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "minimum_order_amount",
                table: "promotions",
                newName: "MinimumOrderAmount");

            migrationBuilder.RenameColumn(
                name: "max_uses_total",
                table: "promotions",
                newName: "MaxUsesTotal");

            migrationBuilder.RenameColumn(
                name: "max_uses_per_customer",
                table: "promotions",
                newName: "MaxUsesPerCustomer");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "promotions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "get_quantity",
                table: "promotions",
                newName: "GetQuantity");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "promotions",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "discount_value",
                table: "promotions",
                newName: "DiscountValue");

            migrationBuilder.RenameColumn(
                name: "discount_type",
                table: "promotions",
                newName: "DiscountType");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "promotions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "buy_quantity",
                table: "promotions",
                newName: "BuyQuantity");

            migrationBuilder.RenameColumn(
                name: "apply_to_all_products",
                table: "promotions",
                newName: "ApplyToAllProducts");

            migrationBuilder.RenameColumn(
                name: "applicable_skus",
                table: "promotions",
                newName: "ApplicableSkus");

            migrationBuilder.RenameIndex(
                name: "ix_promotions_organization_id",
                table: "promotions",
                newName: "IX_promotions_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "product_variants",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "product_variants",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "product_variants",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "size",
                table: "product_variants",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "material",
                table: "product_variants",
                newName: "Material");

            migrationBuilder.RenameColumn(
                name: "color",
                table: "product_variants",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "barcode",
                table: "product_variants",
                newName: "Barcode");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "product_variants",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "product_variants",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "product_variants",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "price_override",
                table: "product_variants",
                newName: "PriceOverride");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "product_variants",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "product_variants",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "product_variants",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cost_override",
                table: "product_variants",
                newName: "CostOverride");

            migrationBuilder.RenameColumn(
                name: "additional_attributes",
                table: "product_variants",
                newName: "AdditionalAttributes");

            migrationBuilder.RenameIndex(
                name: "ix_product_variants_barcode",
                table: "product_variants",
                newName: "IX_product_variants_Barcode");

            migrationBuilder.RenameIndex(
                name: "ix_product_variants_product_id",
                table: "product_variants",
                newName: "IX_product_variants_ProductId");

            migrationBuilder.RenameIndex(
                name: "ix_product_variants_organization_id_sku",
                table: "product_variants",
                newName: "IX_product_variants_OrganizationId_Sku");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "pos_transactions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "pos_transactions",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "channel",
                table: "pos_transactions",
                newName: "Channel");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "pos_transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "pos_transactions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transaction_type",
                table: "pos_transactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "transaction_number",
                table: "pos_transactions",
                newName: "TransactionNumber");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "pos_transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "tendered_amount",
                table: "pos_transactions",
                newName: "TenderedAmount");

            migrationBuilder.RenameColumn(
                name: "tax_total",
                table: "pos_transactions",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "pos_transactions",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "store_id",
                table: "pos_transactions",
                newName: "StoreId");

            migrationBuilder.RenameColumn(
                name: "source_file",
                table: "pos_transactions",
                newName: "SourceFile");

            migrationBuilder.RenameColumn(
                name: "retail_statement_id",
                table: "pos_transactions",
                newName: "RetailStatementId");

            migrationBuilder.RenameColumn(
                name: "processing_error",
                table: "pos_transactions",
                newName: "ProcessingError");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "pos_transactions",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "pos_transactions",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "pos_transactions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "pos_transactions",
                newName: "GrandTotal");

            migrationBuilder.RenameColumn(
                name: "fulfillment_status",
                table: "pos_transactions",
                newName: "FulfillmentStatus");

            migrationBuilder.RenameColumn(
                name: "external_ref",
                table: "pos_transactions",
                newName: "ExternalRef");

            migrationBuilder.RenameColumn(
                name: "external_order_ref",
                table: "pos_transactions",
                newName: "ExternalOrderRef");

            migrationBuilder.RenameColumn(
                name: "discount_total",
                table: "pos_transactions",
                newName: "DiscountTotal");

            migrationBuilder.RenameColumn(
                name: "delivery_address",
                table: "pos_transactions",
                newName: "DeliveryAddress");

            migrationBuilder.RenameColumn(
                name: "customer_phone",
                table: "pos_transactions",
                newName: "CustomerPhone");

            migrationBuilder.RenameColumn(
                name: "customer_name",
                table: "pos_transactions",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "customer_email",
                table: "pos_transactions",
                newName: "CustomerEmail");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "pos_transactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "coupon_discount",
                table: "pos_transactions",
                newName: "CouponDiscount");

            migrationBuilder.RenameColumn(
                name: "coupon_code",
                table: "pos_transactions",
                newName: "CouponCode");

            migrationBuilder.RenameColumn(
                name: "channel_notes",
                table: "pos_transactions",
                newName: "ChannelNotes");

            migrationBuilder.RenameColumn(
                name: "change_amount",
                table: "pos_transactions",
                newName: "ChangeAmount");

            migrationBuilder.RenameColumn(
                name: "cashier_name",
                table: "pos_transactions",
                newName: "CashierName");

            migrationBuilder.RenameColumn(
                name: "cashier_id",
                table: "pos_transactions",
                newName: "CashierId");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "pos_transactions",
                newName: "ARInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_pos_transactions_status",
                table: "pos_transactions",
                newName: "IX_pos_transactions_Status");

            migrationBuilder.RenameIndex(
                name: "ix_pos_transactions_retail_statement_id",
                table: "pos_transactions",
                newName: "IX_pos_transactions_RetailStatementId");

            migrationBuilder.RenameIndex(
                name: "ix_pos_transactions_organization_id_transaction_number",
                table: "pos_transactions",
                newName: "IX_pos_transactions_OrganizationId_TransactionNumber");

            migrationBuilder.RenameIndex(
                name: "ix_pos_transactions_organization_id_external_ref",
                table: "pos_transactions",
                newName: "IX_pos_transactions_OrganizationId_ExternalRef");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "pos_transaction_lines",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "pos_transaction_lines",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "pos_transaction_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "pos_transaction_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "pos_transaction_lines",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "pos_transaction_lines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "tax_rate",
                table: "pos_transaction_lines",
                newName: "TaxRate");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "pos_transaction_lines",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "pos_transaction_lines",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "pos_transaction_lines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "pos_transaction_id",
                table: "pos_transaction_lines",
                newName: "POSTransactionId");

            migrationBuilder.RenameColumn(
                name: "line_total",
                table: "pos_transaction_lines",
                newName: "LineTotal");

            migrationBuilder.RenameColumn(
                name: "line_sub_total",
                table: "pos_transaction_lines",
                newName: "LineSubTotal");

            migrationBuilder.RenameColumn(
                name: "is_return",
                table: "pos_transaction_lines",
                newName: "IsReturn");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "pos_transaction_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "discount_pct",
                table: "pos_transaction_lines",
                newName: "DiscountPct");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "pos_transaction_lines",
                newName: "DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "pos_transaction_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_pos_transaction_lines_pos_transaction_id",
                table: "pos_transaction_lines",
                newName: "IX_pos_transaction_lines_POSTransactionId");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "pos_payments",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "pos_payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "pos_payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "pos_payments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "pos_transaction_id",
                table: "pos_payments",
                newName: "POSTransactionId");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "pos_payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "pos_payments",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "pos_payments",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_pos_payments_pos_transaction_id",
                table: "pos_payments",
                newName: "IX_pos_payments_POSTransactionId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "payment_proposals",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "payment_proposals",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "payment_proposals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "payment_proposals",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "proposal_number",
                table: "payment_proposals",
                newName: "ProposalNumber");

            migrationBuilder.RenameColumn(
                name: "proposal_date",
                table: "payment_proposals",
                newName: "ProposalDate");

            migrationBuilder.RenameColumn(
                name: "processed_by",
                table: "payment_proposals",
                newName: "ProcessedBy");

            migrationBuilder.RenameColumn(
                name: "processed_at",
                table: "payment_proposals",
                newName: "ProcessedAt");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "payment_proposals",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "payment_proposals",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "payment_proposals",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "payment_proposals",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "payment_proposals",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bank_account",
                table: "payment_proposals",
                newName: "BankAccount");

            migrationBuilder.RenameIndex(
                name: "ix_payment_proposals_organization_id_proposal_number",
                table: "payment_proposals",
                newName: "IX_payment_proposals_OrganizationId_ProposalNumber");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "payment_proposal_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendor_name",
                table: "payment_proposal_lines",
                newName: "VendorName");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "payment_proposal_lines",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "payment_proposal_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "proposed_amount",
                table: "payment_proposal_lines",
                newName: "ProposedAmount");

            migrationBuilder.RenameColumn(
                name: "proposal_id",
                table: "payment_proposal_lines",
                newName: "ProposalId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "payment_proposal_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "invoice_number",
                table: "payment_proposal_lines",
                newName: "InvoiceNumber");

            migrationBuilder.RenameColumn(
                name: "invoice_due_date",
                table: "payment_proposal_lines",
                newName: "InvoiceDueDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "payment_proposal_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ap_payment_id",
                table: "payment_proposal_lines",
                newName: "APPaymentId");

            migrationBuilder.RenameColumn(
                name: "ap_invoice_id",
                table: "payment_proposal_lines",
                newName: "APInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_payment_proposal_lines_proposal_id",
                table: "payment_proposal_lines",
                newName: "IX_payment_proposal_lines_ProposalId");

            migrationBuilder.RenameIndex(
                name: "ix_payment_proposal_lines_ap_payment_id",
                table: "payment_proposal_lines",
                newName: "IX_payment_proposal_lines_APPaymentId");

            migrationBuilder.RenameIndex(
                name: "ix_payment_proposal_lines_ap_invoice_id",
                table: "payment_proposal_lines",
                newName: "IX_payment_proposal_lines_APInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_organizations_code",
                table: "organizations",
                newName: "IX_organizations_code");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "journal_lines",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "debit",
                table: "journal_lines",
                newName: "Debit");

            migrationBuilder.RenameColumn(
                name: "credit",
                table: "journal_lines",
                newName: "Credit");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "journal_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "journal_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "line_order",
                table: "journal_lines",
                newName: "LineOrder");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "journal_lines",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "journal_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "journal_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "journal_lines",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "ix_journal_lines_journal_entry_id",
                table: "journal_lines",
                newName: "IX_journal_lines_JournalEntryId");

            migrationBuilder.RenameIndex(
                name: "ix_journal_lines_account_id",
                table: "journal_lines",
                newName: "IX_journal_lines_AccountId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "journal_entries",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "journal_entries",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "journal_entries",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "journal_entries",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "journal_entries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "journal_entries",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_debit",
                table: "journal_entries",
                newName: "TotalDebit");

            migrationBuilder.RenameColumn(
                name: "total_credit",
                table: "journal_entries",
                newName: "TotalCredit");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "journal_entries",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "journal_type",
                table: "journal_entries",
                newName: "JournalType");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "journal_entries",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "fiscal_period_id",
                table: "journal_entries",
                newName: "FiscalPeriodId");

            migrationBuilder.RenameColumn(
                name: "entry_number",
                table: "journal_entries",
                newName: "EntryNumber");

            migrationBuilder.RenameColumn(
                name: "entry_date",
                table: "journal_entries",
                newName: "EntryDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "journal_entries",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_journal_entries_organization_id_entry_number",
                table: "journal_entries",
                newName: "IX_journal_entries_OrganizationId_EntryNumber");

            migrationBuilder.RenameIndex(
                name: "ix_journal_entries_fiscal_period_id",
                table: "journal_entries",
                newName: "IX_journal_entries_FiscalPeriodId");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "inventory_transactions",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "inventory_transactions",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "inventory_transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "inventory_transactions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_cost",
                table: "inventory_transactions",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "transaction_type",
                table: "inventory_transactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "inventory_transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "reference_number",
                table: "inventory_transactions",
                newName: "ReferenceNumber");

            migrationBuilder.RenameColumn(
                name: "reference_document_id",
                table: "inventory_transactions",
                newName: "ReferenceDocumentId");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "inventory_transactions",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "inventory_transactions",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "inventory_transactions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "inventory_transactions",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "inventory_transactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "balance_after",
                table: "inventory_transactions",
                newName: "BalanceAfter");

            migrationBuilder.RenameIndex(
                name: "ix_inventory_transactions_product_variant_id",
                table: "inventory_transactions",
                newName: "IX_inventory_transactions_ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "ix_inventory_transactions_organization_id_product_variant_id_t",
                table: "inventory_transactions",
                newName: "IX_inventory_transactions_OrganizationId_ProductVariantId_Tran~");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "inventory_records",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "inventory_records",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "inventory_records",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "reorder_point",
                table: "inventory_records",
                newName: "ReorderPoint");

            migrationBuilder.RenameColumn(
                name: "quantity_reserved",
                table: "inventory_records",
                newName: "QuantityReserved");

            migrationBuilder.RenameColumn(
                name: "quantity_on_order",
                table: "inventory_records",
                newName: "QuantityOnOrder");

            migrationBuilder.RenameColumn(
                name: "quantity_on_hand",
                table: "inventory_records",
                newName: "QuantityOnHand");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "inventory_records",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "inventory_records",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "minimum_stock",
                table: "inventory_records",
                newName: "MinimumStock");

            migrationBuilder.RenameColumn(
                name: "maximum_stock",
                table: "inventory_records",
                newName: "MaximumStock");

            migrationBuilder.RenameColumn(
                name: "last_received_date",
                table: "inventory_records",
                newName: "LastReceivedDate");

            migrationBuilder.RenameColumn(
                name: "last_count_date",
                table: "inventory_records",
                newName: "LastCountDate");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "inventory_records",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "inventory_records",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "average_cost",
                table: "inventory_records",
                newName: "AverageCost");

            migrationBuilder.RenameIndex(
                name: "ix_inventory_records_product_variant_id",
                table: "inventory_records",
                newName: "IX_inventory_records_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "import_jobs",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "import_jobs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "import_jobs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "triggered_by",
                table: "import_jobs",
                newName: "TriggeredBy");

            migrationBuilder.RenameColumn(
                name: "total_rows",
                table: "import_jobs",
                newName: "TotalRows");

            migrationBuilder.RenameColumn(
                name: "success_rows",
                table: "import_jobs",
                newName: "SuccessRows");

            migrationBuilder.RenameColumn(
                name: "started_at",
                table: "import_jobs",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "import_jobs",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "import_jobs",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "file_path",
                table: "import_jobs",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "file_name",
                table: "import_jobs",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "file_format",
                table: "import_jobs",
                newName: "FileFormat");

            migrationBuilder.RenameColumn(
                name: "failed_rows",
                table: "import_jobs",
                newName: "FailedRows");

            migrationBuilder.RenameColumn(
                name: "error_summary",
                table: "import_jobs",
                newName: "ErrorSummary");

            migrationBuilder.RenameColumn(
                name: "entity_type",
                table: "import_jobs",
                newName: "EntityType");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "import_jobs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "completed_at",
                table: "import_jobs",
                newName: "CompletedAt");

            migrationBuilder.RenameIndex(
                name: "ix_import_jobs_status",
                table: "import_jobs",
                newName: "IX_import_jobs_Status");

            migrationBuilder.RenameIndex(
                name: "ix_import_jobs_organization_id",
                table: "import_jobs",
                newName: "IX_import_jobs_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "import_job_rows",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "import_job_rows",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "import_job_rows",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "row_number",
                table: "import_job_rows",
                newName: "RowNumber");

            migrationBuilder.RenameColumn(
                name: "raw_payload",
                table: "import_job_rows",
                newName: "RawPayload");

            migrationBuilder.RenameColumn(
                name: "promoted_entity_id",
                table: "import_job_rows",
                newName: "PromotedEntityId");

            migrationBuilder.RenameColumn(
                name: "promoted_at",
                table: "import_job_rows",
                newName: "PromotedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "import_job_rows",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "import_job_id",
                table: "import_job_rows",
                newName: "ImportJobId");

            migrationBuilder.RenameColumn(
                name: "error_message",
                table: "import_job_rows",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "import_job_rows",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_import_job_rows_import_job_id_status",
                table: "import_job_rows",
                newName: "IX_import_job_rows_ImportJobId_Status");

            migrationBuilder.RenameIndex(
                name: "ix_import_job_rows_import_job_id",
                table: "import_job_rows",
                newName: "IX_import_job_rows_ImportJobId");

            migrationBuilder.RenameColumn(
                name: "supplier",
                table: "fixed_assets",
                newName: "Supplier");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "fixed_assets",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "fixed_assets",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "fixed_assets",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "fixed_assets",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fixed_assets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "useful_life_years",
                table: "fixed_assets",
                newName: "UsefulLifeYears");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fixed_assets",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "units_produced_to_date",
                table: "fixed_assets",
                newName: "UnitsProducedToDate");

            migrationBuilder.RenameColumn(
                name: "total_estimated_units",
                table: "fixed_assets",
                newName: "TotalEstimatedUnits");

            migrationBuilder.RenameColumn(
                name: "serial_number",
                table: "fixed_assets",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "salvage_value",
                table: "fixed_assets",
                newName: "SalvageValue");

            migrationBuilder.RenameColumn(
                name: "purchase_order_ref",
                table: "fixed_assets",
                newName: "PurchaseOrderRef");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "fixed_assets",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "last_depreciation_date",
                table: "fixed_assets",
                newName: "LastDepreciationDate");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "fixed_assets",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "gl_depreciation_expense_account_id",
                table: "fixed_assets",
                newName: "GLDepreciationExpenseAccountId");

            migrationBuilder.RenameColumn(
                name: "gl_asset_account_id",
                table: "fixed_assets",
                newName: "GLAssetAccountId");

            migrationBuilder.RenameColumn(
                name: "gl_accumulated_depreciation_account_id",
                table: "fixed_assets",
                newName: "GLAccumulatedDepreciationAccountId");

            migrationBuilder.RenameColumn(
                name: "depreciation_start_date",
                table: "fixed_assets",
                newName: "DepreciationStartDate");

            migrationBuilder.RenameColumn(
                name: "depreciation_method",
                table: "fixed_assets",
                newName: "DepreciationMethod");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fixed_assets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "asset_name",
                table: "fixed_assets",
                newName: "AssetName");

            migrationBuilder.RenameColumn(
                name: "asset_code",
                table: "fixed_assets",
                newName: "AssetCode");

            migrationBuilder.RenameColumn(
                name: "acquisition_date",
                table: "fixed_assets",
                newName: "AcquisitionDate");

            migrationBuilder.RenameColumn(
                name: "acquisition_cost",
                table: "fixed_assets",
                newName: "AcquisitionCost");

            migrationBuilder.RenameColumn(
                name: "accumulated_depreciation",
                table: "fixed_assets",
                newName: "AccumulatedDepreciation");

            migrationBuilder.RenameIndex(
                name: "ix_fixed_assets_organization_id_asset_code",
                table: "fixed_assets",
                newName: "IX_fixed_assets_OrganizationId_AssetCode");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "fiscal_years",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "fiscal_years",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "fiscal_years",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fiscal_years",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fiscal_years",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "fiscal_years",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "period_count",
                table: "fiscal_years",
                newName: "PeriodCount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "fiscal_years",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "fiscal_years",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "fiscal_years",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fiscal_years",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "calendar_type",
                table: "fiscal_years",
                newName: "CalendarType");

            migrationBuilder.RenameIndex(
                name: "ix_fiscal_years_organization_id_name",
                table: "fiscal_years",
                newName: "IX_fiscal_years_OrganizationId_Name");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "fiscal_periods",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "fiscal_periods",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fiscal_periods",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fiscal_periods",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "fiscal_periods",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "period_number",
                table: "fiscal_periods",
                newName: "PeriodNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "fiscal_periods",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "fiscal_year_id",
                table: "fiscal_periods",
                newName: "FiscalYearId");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "fiscal_periods",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fiscal_periods",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_fiscal_periods_fiscal_year_id",
                table: "fiscal_periods",
                newName: "IX_fiscal_periods_FiscalYearId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "export_job_rows",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "export_job_rows",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "export_job_rows",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "export_job_rows",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "export_job_rows",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "exported_at",
                table: "export_job_rows",
                newName: "ExportedAt");

            migrationBuilder.RenameColumn(
                name: "error_message",
                table: "export_job_rows",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "entity_type",
                table: "export_job_rows",
                newName: "EntityType");

            migrationBuilder.RenameColumn(
                name: "entity_ref",
                table: "export_job_rows",
                newName: "EntityRef");

            migrationBuilder.RenameColumn(
                name: "entity_id",
                table: "export_job_rows",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "export_job_rows",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "blob_name",
                table: "export_job_rows",
                newName: "BlobName");

            migrationBuilder.RenameColumn(
                name: "batch_job_config_id",
                table: "export_job_rows",
                newName: "BatchJobConfigId");

            migrationBuilder.RenameIndex(
                name: "ix_export_job_rows_organization_id",
                table: "export_job_rows",
                newName: "IX_export_job_rows_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "ix_export_job_rows_entity_type_entity_id",
                table: "export_job_rows",
                newName: "IX_export_job_rows_EntityType_EntityId");

            migrationBuilder.RenameIndex(
                name: "ix_export_job_rows_batch_job_config_id_exported_at",
                table: "export_job_rows",
                newName: "IX_export_job_rows_BatchJobConfigId_ExportedAt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "expense_reports",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "purpose",
                table: "expense_reports",
                newName: "Purpose");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "expense_reports",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "department",
                table: "expense_reports",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "expense_reports",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "expense_reports",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "expense_reports",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "expense_reports",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "expense_reports",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "submitted_by",
                table: "expense_reports",
                newName: "SubmittedBy");

            migrationBuilder.RenameColumn(
                name: "submitted_at",
                table: "expense_reports",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "report_number",
                table: "expense_reports",
                newName: "ReportNumber");

            migrationBuilder.RenameColumn(
                name: "rejected_reason",
                table: "expense_reports",
                newName: "RejectedReason");

            migrationBuilder.RenameColumn(
                name: "period_start",
                table: "expense_reports",
                newName: "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "period_end",
                table: "expense_reports",
                newName: "PeriodEnd");

            migrationBuilder.RenameColumn(
                name: "paid_amount",
                table: "expense_reports",
                newName: "PaidAmount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "expense_reports",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "expense_reports",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "employee_name",
                table: "expense_reports",
                newName: "EmployeeName");

            migrationBuilder.RenameColumn(
                name: "employee_email",
                table: "expense_reports",
                newName: "EmployeeEmail");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "expense_reports",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "approved_by",
                table: "expense_reports",
                newName: "ApprovedBy");

            migrationBuilder.RenameColumn(
                name: "approved_at",
                table: "expense_reports",
                newName: "ApprovedAt");

            migrationBuilder.RenameColumn(
                name: "approved_amount",
                table: "expense_reports",
                newName: "ApprovedAmount");

            migrationBuilder.RenameColumn(
                name: "approval_status",
                table: "expense_reports",
                newName: "ApprovalStatus");

            migrationBuilder.RenameIndex(
                name: "ix_expense_reports_organization_id_report_number",
                table: "expense_reports",
                newName: "IX_expense_reports_OrganizationId_ReportNumber");

            migrationBuilder.RenameColumn(
                name: "merchant",
                table: "expense_lines",
                newName: "Merchant");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "expense_lines",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "expense_lines",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "expense_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "expense_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "receipt_url",
                table: "expense_lines",
                newName: "ReceiptUrl");

            migrationBuilder.RenameColumn(
                name: "is_reimbursable",
                table: "expense_lines",
                newName: "IsReimbursable");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "expense_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "expense_report_id",
                table: "expense_lines",
                newName: "ExpenseReportId");

            migrationBuilder.RenameColumn(
                name: "expense_date",
                table: "expense_lines",
                newName: "ExpenseDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "expense_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "expense_lines",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "expense_lines",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_expense_lines_expense_report_id",
                table: "expense_lines",
                newName: "IX_expense_lines_ExpenseReportId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "expense_categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "expense_categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "expense_categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "expense_categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "expense_categories",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "limit_per_claim",
                table: "expense_categories",
                newName: "LimitPerClaim");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "expense_categories",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "expense_categories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "gl_account_id",
                table: "expense_categories",
                newName: "GLAccountId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "expense_categories",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "dunning_records",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "dunning_records",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "dunning_records",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "dunning_records",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "dunning_records",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "sent_date",
                table: "dunning_records",
                newName: "SentDate");

            migrationBuilder.RenameColumn(
                name: "resolved_at",
                table: "dunning_records",
                newName: "ResolvedAt");

            migrationBuilder.RenameColumn(
                name: "resolution_notes",
                table: "dunning_records",
                newName: "ResolutionNotes");

            migrationBuilder.RenameColumn(
                name: "outstanding_amount",
                table: "dunning_records",
                newName: "OutstandingAmount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "dunning_records",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "dunning_records",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "follow_up_date",
                table: "dunning_records",
                newName: "FollowUpDate");

            migrationBuilder.RenameColumn(
                name: "dunning_number",
                table: "dunning_records",
                newName: "DunningNumber");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "dunning_records",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "dunning_records",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "assigned_to",
                table: "dunning_records",
                newName: "AssignedTo");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "dunning_records",
                newName: "ARInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_dunning_records_organization_id_dunning_number",
                table: "dunning_records",
                newName: "IX_dunning_records_OrganizationId_DunningNumber");

            migrationBuilder.RenameIndex(
                name: "ix_dunning_records_customer_id",
                table: "dunning_records",
                newName: "IX_dunning_records_CustomerId");

            migrationBuilder.RenameIndex(
                name: "ix_dunning_records_ar_invoice_id",
                table: "dunning_records",
                newName: "IX_dunning_records_ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "website",
                table: "customers",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "customers",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "customers",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "customers",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "customers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "customers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "customers",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "customers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "customers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "customers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "shipping_address",
                table: "customers",
                newName: "ShippingAddress");

            migrationBuilder.RenameColumn(
                name: "payment_terms_days",
                table: "customers",
                newName: "PaymentTermsDays");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "customers",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "customers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "customer_number",
                table: "customers",
                newName: "CustomerNumber");

            migrationBuilder.RenameColumn(
                name: "credit_limit",
                table: "customers",
                newName: "CreditLimit");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "customers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "billing_address",
                table: "customers",
                newName: "BillingAddress");

            migrationBuilder.RenameIndex(
                name: "ix_customers_organization_id_customer_number",
                table: "customers",
                newName: "IX_customers_OrganizationId_CustomerNumber");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "customer_credit_notes",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "customer_credit_notes",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "customer_credit_notes",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "customer_credit_notes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "customer_credit_notes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "customer_credit_notes",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "customer_credit_notes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "customer_credit_notes",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "customer_credit_notes",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "customer_credit_notes",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "sales_order_id",
                table: "customer_credit_notes",
                newName: "SalesOrderId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "customer_credit_notes",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "customer_credit_notes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "customer_ref",
                table: "customer_credit_notes",
                newName: "CustomerRef");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "customer_credit_notes",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "credit_note_number",
                table: "customer_credit_notes",
                newName: "CreditNoteNumber");

            migrationBuilder.RenameColumn(
                name: "credit_date",
                table: "customer_credit_notes",
                newName: "CreditDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "customer_credit_notes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "customer_credit_notes",
                newName: "ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "applied_amount",
                table: "customer_credit_notes",
                newName: "AppliedAmount");

            migrationBuilder.RenameIndex(
                name: "ix_customer_credit_notes_organization_id_credit_note_number",
                table: "customer_credit_notes",
                newName: "IX_customer_credit_notes_OrganizationId_CreditNoteNumber");

            migrationBuilder.RenameIndex(
                name: "ix_customer_credit_notes_customer_id",
                table: "customer_credit_notes",
                newName: "IX_customer_credit_notes_CustomerId");

            migrationBuilder.RenameIndex(
                name: "ix_customer_credit_notes_ar_invoice_id",
                table: "customer_credit_notes",
                newName: "IX_customer_credit_notes_ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "customer_contacts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "customer_contacts",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "customer_contacts",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "customer_contacts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "mobile",
                table: "customer_contacts",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "customer_contacts",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "customer_contacts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "customer_contacts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "customer_contacts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_primary",
                table: "customer_contacts",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "customer_contacts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "customer_contacts",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "customer_contacts",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_customer_contacts_customer_id",
                table: "customer_contacts",
                newName: "IX_customer_contacts_CustomerId");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "customer_addresses",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "line2",
                table: "customer_addresses",
                newName: "Line2");

            migrationBuilder.RenameColumn(
                name: "line1",
                table: "customer_addresses",
                newName: "Line1");

            migrationBuilder.RenameColumn(
                name: "label",
                table: "customer_addresses",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "customer_addresses",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "customer_addresses",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "customer_addresses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "customer_addresses",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "postal_code",
                table: "customer_addresses",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "customer_addresses",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_primary",
                table: "customer_addresses",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "customer_addresses",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "customer_addresses",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "customer_addresses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "address_type",
                table: "customer_addresses",
                newName: "AddressType");

            migrationBuilder.RenameIndex(
                name: "ix_customer_addresses_customer_id",
                table: "customer_addresses",
                newName: "IX_customer_addresses_CustomerId");

            migrationBuilder.RenameColumn(
                name: "symbol",
                table: "currencies",
                newName: "Symbol");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "currencies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "currencies",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "currencies",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "currencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "currencies",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "rate_updated_at",
                table: "currencies",
                newName: "RateUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "currencies",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "numeric_code",
                table: "currencies",
                newName: "NumericCode");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "currencies",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_base",
                table: "currencies",
                newName: "IsBase");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "currencies",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "exchange_rate",
                table: "currencies",
                newName: "ExchangeRate");

            migrationBuilder.RenameColumn(
                name: "decimal_places",
                table: "currencies",
                newName: "DecimalPlaces");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "currencies",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_currencies_organization_id_code",
                table: "currencies",
                newName: "IX_currencies_OrganizationId_Code");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "coupons",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "coupons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "used_count",
                table: "coupons",
                newName: "UsedCount");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "coupons",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "promotion_id",
                table: "coupons",
                newName: "PromotionId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "coupons",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "max_uses",
                table: "coupons",
                newName: "MaxUses");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "coupons",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "coupons",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "expires_at",
                table: "coupons",
                newName: "ExpiresAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "coupons",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_coupons_promotion_id",
                table: "coupons",
                newName: "IX_coupons_PromotionId");

            migrationBuilder.RenameIndex(
                name: "ix_coupons_organization_id_code",
                table: "coupons",
                newName: "IX_coupons_OrganizationId_Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "coupon_redemptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "coupon_redemptions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "redeemed_at",
                table: "coupon_redemptions",
                newName: "RedeemedAt");

            migrationBuilder.RenameColumn(
                name: "pos_transaction_id",
                table: "coupon_redemptions",
                newName: "POSTransactionId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "coupon_redemptions",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "coupon_redemptions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "discount_applied",
                table: "coupon_redemptions",
                newName: "DiscountApplied");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "coupon_redemptions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "coupon_id",
                table: "coupon_redemptions",
                newName: "CouponId");

            migrationBuilder.RenameIndex(
                name: "ix_coupon_redemptions_pos_transaction_id",
                table: "coupon_redemptions",
                newName: "IX_coupon_redemptions_POSTransactionId");

            migrationBuilder.RenameIndex(
                name: "ix_coupon_redemptions_coupon_id",
                table: "coupon_redemptions",
                newName: "IX_coupon_redemptions_CouponId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "categories",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tax_rate",
                table: "categories",
                newName: "TaxRate");

            migrationBuilder.RenameColumn(
                name: "tax_code",
                table: "categories",
                newName: "TaxCode");

            migrationBuilder.RenameColumn(
                name: "parent_category_id",
                table: "categories",
                newName: "ParentCategoryId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "categories",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "categories",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "categories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "display_order",
                table: "categories",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "categories",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_categories_parent_category_id",
                table: "categories",
                newName: "IX_categories_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_categories_organization_id_code",
                table: "categories",
                newName: "IX_categories_OrganizationId_Code");

            migrationBuilder.RenameColumn(
                name: "tags",
                table: "catalog_products",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "catalog_products",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "sku",
                table: "catalog_products",
                newName: "Sku");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "catalog_products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "catalog_products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "catalog_products",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "catalog_products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "catalog_products",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "catalog_products",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "tax_rate_override",
                table: "catalog_products",
                newName: "TaxRateOverride");

            migrationBuilder.RenameColumn(
                name: "product_type",
                table: "catalog_products",
                newName: "ProductType");

            migrationBuilder.RenameColumn(
                name: "preferred_vendor_id",
                table: "catalog_products",
                newName: "PreferredVendorId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "catalog_products",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "long_description",
                table: "catalog_products",
                newName: "LongDescription");

            migrationBuilder.RenameColumn(
                name: "is_exported",
                table: "catalog_products",
                newName: "IsExported");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "catalog_products",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "catalog_products",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "gender_target",
                table: "catalog_products",
                newName: "GenderTarget");

            migrationBuilder.RenameColumn(
                name: "exported_at",
                table: "catalog_products",
                newName: "ExportedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "catalog_products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "catalog_products",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "brand_id",
                table: "catalog_products",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "base_price",
                table: "catalog_products",
                newName: "BasePrice");

            migrationBuilder.RenameColumn(
                name: "base_cost",
                table: "catalog_products",
                newName: "BaseCost");

            migrationBuilder.RenameIndex(
                name: "ix_catalog_products_preferred_vendor_id",
                table: "catalog_products",
                newName: "IX_catalog_products_PreferredVendorId");

            migrationBuilder.RenameIndex(
                name: "ix_catalog_products_organization_id_sku",
                table: "catalog_products",
                newName: "IX_catalog_products_OrganizationId_Sku");

            migrationBuilder.RenameIndex(
                name: "ix_catalog_products_category_id",
                table: "catalog_products",
                newName: "IX_catalog_products_CategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_catalog_products_brand_id",
                table: "catalog_products",
                newName: "IX_catalog_products_BrandId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "cash_journals",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "cash_journals",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "cash_journals",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "cash_journals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "cash_journals",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_debits",
                table: "cash_journals",
                newName: "TotalDebits");

            migrationBuilder.RenameColumn(
                name: "total_credits",
                table: "cash_journals",
                newName: "TotalCredits");

            migrationBuilder.RenameColumn(
                name: "posted_by",
                table: "cash_journals",
                newName: "PostedBy");

            migrationBuilder.RenameColumn(
                name: "posted_at",
                table: "cash_journals",
                newName: "PostedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "cash_journals",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "journal_number",
                table: "cash_journals",
                newName: "JournalNumber");

            migrationBuilder.RenameColumn(
                name: "journal_date",
                table: "cash_journals",
                newName: "JournalDate");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "cash_journals",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "cash_journals",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bank_account_id",
                table: "cash_journals",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "ix_cash_journals_status",
                table: "cash_journals",
                newName: "IX_cash_journals_Status");

            migrationBuilder.RenameIndex(
                name: "ix_cash_journals_organization_id_journal_number",
                table: "cash_journals",
                newName: "IX_cash_journals_OrganizationId_JournalNumber");

            migrationBuilder.RenameIndex(
                name: "ix_cash_journals_bank_account_id",
                table: "cash_journals",
                newName: "IX_cash_journals_BankAccountId");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "cash_journal_lines",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "cash_journal_lines",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "debit",
                table: "cash_journal_lines",
                newName: "Debit");

            migrationBuilder.RenameColumn(
                name: "credit",
                table: "cash_journal_lines",
                newName: "Credit");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "cash_journal_lines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "cash_journal_lines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "journal_id",
                table: "cash_journal_lines",
                newName: "JournalId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "cash_journal_lines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "gl_account_id",
                table: "cash_journal_lines",
                newName: "GLAccountId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "cash_journal_lines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_cash_journal_lines_journal_id",
                table: "cash_journal_lines",
                newName: "IX_cash_journal_lines_JournalId");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Campaigns",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "tags",
                table: "Campaigns",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Campaigns",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Campaigns",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Campaigns",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "budget",
                table: "Campaigns",
                newName: "Budget");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Campaigns",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Campaigns",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "target_audience",
                table: "Campaigns",
                newName: "TargetAudience");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Campaigns",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "reach_count",
                table: "Campaigns",
                newName: "ReachCount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "Campaigns",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "linked_promotion_id",
                table: "Campaigns",
                newName: "LinkedPromotionId");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Campaigns",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Campaigns",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "conversion_count",
                table: "Campaigns",
                newName: "ConversionCount");

            migrationBuilder.RenameColumn(
                name: "actual_spend",
                table: "Campaigns",
                newName: "ActualSpend");

            migrationBuilder.RenameIndex(
                name: "ix_campaigns_status",
                table: "Campaigns",
                newName: "IX_Campaigns_Status");

            migrationBuilder.RenameIndex(
                name: "ix_campaigns_organization_id",
                table: "Campaigns",
                newName: "IX_Campaigns_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "website",
                table: "brands",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "brands",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "brands",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "brands",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "brands",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "brands",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "brands",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "brands",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "logo_url",
                table: "brands",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "brands",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "brands",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "brands",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_brands_organization_id_code",
                table: "brands",
                newName: "IX_brands_OrganizationId_Code");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "batch_job_configs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "batch_job_configs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "batch_job_configs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "batch_job_configs",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "local_processed_path",
                table: "batch_job_configs",
                newName: "LocalProcessedPath");

            migrationBuilder.RenameColumn(
                name: "local_inbox_path",
                table: "batch_job_configs",
                newName: "LocalInboxPath");

            migrationBuilder.RenameColumn(
                name: "local_export_path",
                table: "batch_job_configs",
                newName: "LocalExportPath");

            migrationBuilder.RenameColumn(
                name: "local_error_path",
                table: "batch_job_configs",
                newName: "LocalErrorPath");

            migrationBuilder.RenameColumn(
                name: "last_run_status",
                table: "batch_job_configs",
                newName: "LastRunStatus");

            migrationBuilder.RenameColumn(
                name: "last_run_rows_promoted",
                table: "batch_job_configs",
                newName: "LastRunRowsPromoted");

            migrationBuilder.RenameColumn(
                name: "last_run_message",
                table: "batch_job_configs",
                newName: "LastRunMessage");

            migrationBuilder.RenameColumn(
                name: "last_run_files_processed",
                table: "batch_job_configs",
                newName: "LastRunFilesProcessed");

            migrationBuilder.RenameColumn(
                name: "last_run_at",
                table: "batch_job_configs",
                newName: "LastRunAt");

            migrationBuilder.RenameColumn(
                name: "job_type",
                table: "batch_job_configs",
                newName: "JobType");

            migrationBuilder.RenameColumn(
                name: "is_enabled",
                table: "batch_job_configs",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "batch_job_configs",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "file_format",
                table: "batch_job_configs",
                newName: "FileFormat");

            migrationBuilder.RenameColumn(
                name: "export_file_name_pattern",
                table: "batch_job_configs",
                newName: "ExportFileNamePattern");

            migrationBuilder.RenameColumn(
                name: "cron_expression",
                table: "batch_job_configs",
                newName: "CronExpression");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "batch_job_configs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "auto_confirm_sales_orders",
                table: "batch_job_configs",
                newName: "AutoConfirmSalesOrders");

            migrationBuilder.RenameIndex(
                name: "ix_batch_job_configs_organization_id_is_enabled",
                table: "batch_job_configs",
                newName: "IX_batch_job_configs_OrganizationId_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "ix_batch_job_configs_organization_id",
                table: "batch_job_configs",
                newName: "IX_batch_job_configs_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "bank_transactions",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "bank_transactions",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "bank_transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "bank_transactions",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "bank_transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "bank_transactions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transfer_to_account_id",
                table: "bank_transactions",
                newName: "TransferToAccountId");

            migrationBuilder.RenameColumn(
                name: "transaction_type",
                table: "bank_transactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "transaction_status",
                table: "bank_transactions",
                newName: "TransactionStatus");

            migrationBuilder.RenameColumn(
                name: "transaction_number",
                table: "bank_transactions",
                newName: "TransactionNumber");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "bank_transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "reconciliation_id",
                table: "bank_transactions",
                newName: "ReconciliationId");

            migrationBuilder.RenameColumn(
                name: "reconciled_at",
                table: "bank_transactions",
                newName: "ReconciledAt");

            migrationBuilder.RenameColumn(
                name: "posted_by",
                table: "bank_transactions",
                newName: "PostedBy");

            migrationBuilder.RenameColumn(
                name: "posted_at",
                table: "bank_transactions",
                newName: "PostedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "bank_transactions",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "bank_transactions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "bank_transactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "counterparty_name",
                table: "bank_transactions",
                newName: "CounterpartyName");

            migrationBuilder.RenameColumn(
                name: "bank_account_id",
                table: "bank_transactions",
                newName: "BankAccountId");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "bank_transactions",
                newName: "ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "ap_invoice_id",
                table: "bank_transactions",
                newName: "APInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_bank_transactions_transaction_status",
                table: "bank_transactions",
                newName: "IX_bank_transactions_TransactionStatus");

            migrationBuilder.RenameIndex(
                name: "ix_bank_transactions_organization_id_transaction_number",
                table: "bank_transactions",
                newName: "IX_bank_transactions_OrganizationId_TransactionNumber");

            migrationBuilder.RenameIndex(
                name: "ix_bank_transactions_bank_account_id_transaction_date",
                table: "bank_transactions",
                newName: "IX_bank_transactions_BankAccountId_TransactionDate");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "bank_reconciliations",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "bank_reconciliations",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "bank_reconciliations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "bank_reconciliations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "system_opening_balance",
                table: "bank_reconciliations",
                newName: "SystemOpeningBalance");

            migrationBuilder.RenameColumn(
                name: "statement_start_date",
                table: "bank_reconciliations",
                newName: "StatementStartDate");

            migrationBuilder.RenameColumn(
                name: "statement_opening_balance",
                table: "bank_reconciliations",
                newName: "StatementOpeningBalance");

            migrationBuilder.RenameColumn(
                name: "statement_end_date",
                table: "bank_reconciliations",
                newName: "StatementEndDate");

            migrationBuilder.RenameColumn(
                name: "statement_closing_balance",
                table: "bank_reconciliations",
                newName: "StatementClosingBalance");

            migrationBuilder.RenameColumn(
                name: "reconciliation_number",
                table: "bank_reconciliations",
                newName: "ReconciliationNumber");

            migrationBuilder.RenameColumn(
                name: "reconciled_amount",
                table: "bank_reconciliations",
                newName: "ReconciledAmount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "bank_reconciliations",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "bank_reconciliations",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "bank_reconciliations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "completed_by",
                table: "bank_reconciliations",
                newName: "CompletedBy");

            migrationBuilder.RenameColumn(
                name: "completed_at",
                table: "bank_reconciliations",
                newName: "CompletedAt");

            migrationBuilder.RenameColumn(
                name: "bank_account_id",
                table: "bank_reconciliations",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "ix_bank_reconciliations_organization_id_reconciliation_number",
                table: "bank_reconciliations",
                newName: "IX_bank_reconciliations_OrganizationId_ReconciliationNumber");

            migrationBuilder.RenameIndex(
                name: "ix_bank_reconciliations_bank_account_id",
                table: "bank_reconciliations",
                newName: "IX_bank_reconciliations_BankAccountId");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "bank_accounts",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "iban",
                table: "bank_accounts",
                newName: "IBAN");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "bank_accounts",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "bank_accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "bank_accounts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "swift_code",
                table: "bank_accounts",
                newName: "SwiftCode");

            migrationBuilder.RenameColumn(
                name: "routing_number",
                table: "bank_accounts",
                newName: "RoutingNumber");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "bank_accounts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "last_reconciled_balance",
                table: "bank_accounts",
                newName: "LastReconciledBalance");

            migrationBuilder.RenameColumn(
                name: "last_reconciled_at",
                table: "bank_accounts",
                newName: "LastReconciledAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "bank_accounts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "gl_account_id",
                table: "bank_accounts",
                newName: "GLAccountId");

            migrationBuilder.RenameColumn(
                name: "current_balance",
                table: "bank_accounts",
                newName: "CurrentBalance");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "bank_accounts",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bank_name",
                table: "bank_accounts",
                newName: "BankName");

            migrationBuilder.RenameColumn(
                name: "bank_branch",
                table: "bank_accounts",
                newName: "BankBranch");

            migrationBuilder.RenameColumn(
                name: "account_type",
                table: "bank_accounts",
                newName: "AccountType");

            migrationBuilder.RenameColumn(
                name: "account_status",
                table: "bank_accounts",
                newName: "AccountStatus");

            migrationBuilder.RenameColumn(
                name: "account_number",
                table: "bank_accounts",
                newName: "AccountNumber");

            migrationBuilder.RenameColumn(
                name: "account_name",
                table: "bank_accounts",
                newName: "AccountName");

            migrationBuilder.RenameColumn(
                name: "account_code",
                table: "bank_accounts",
                newName: "AccountCode");

            migrationBuilder.RenameIndex(
                name: "ix_bank_accounts_organization_id_account_code",
                table: "bank_accounts",
                newName: "IX_bank_accounts_OrganizationId_AccountCode");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "audit_logs",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "module",
                table: "audit_logs",
                newName: "Module");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "audit_logs",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "audit_logs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "audit_logs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "audit_logs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "audit_logs",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "old_values",
                table: "audit_logs",
                newName: "OldValues");

            migrationBuilder.RenameColumn(
                name: "occurred_at",
                table: "audit_logs",
                newName: "OccurredAt");

            migrationBuilder.RenameColumn(
                name: "new_values",
                table: "audit_logs",
                newName: "NewValues");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "audit_logs",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "audit_logs",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "entity_type",
                table: "audit_logs",
                newName: "EntityType");

            migrationBuilder.RenameColumn(
                name: "entity_id",
                table: "audit_logs",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "audit_logs",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_audit_logs_user_id",
                table: "audit_logs",
                newName: "IX_audit_logs_UserId");

            migrationBuilder.RenameIndex(
                name: "ix_audit_logs_organization_id_occurred_at",
                table: "audit_logs",
                newName: "IX_audit_logs_OrganizationId_OccurredAt");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "asset_transfers",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "asset_transfers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "asset_transfers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "transferred_by",
                table: "asset_transfers",
                newName: "TransferredBy");

            migrationBuilder.RenameColumn(
                name: "transfer_date",
                table: "asset_transfers",
                newName: "TransferDate");

            migrationBuilder.RenameColumn(
                name: "to_location",
                table: "asset_transfers",
                newName: "ToLocation");

            migrationBuilder.RenameColumn(
                name: "to_department",
                table: "asset_transfers",
                newName: "ToDepartment");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "asset_transfers",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "asset_transfers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "from_location",
                table: "asset_transfers",
                newName: "FromLocation");

            migrationBuilder.RenameColumn(
                name: "from_department",
                table: "asset_transfers",
                newName: "FromDepartment");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "asset_transfers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "asset_id",
                table: "asset_transfers",
                newName: "AssetId");

            migrationBuilder.RenameIndex(
                name: "ix_asset_transfers_asset_id",
                table: "asset_transfers",
                newName: "IX_asset_transfers_AssetId");

            migrationBuilder.RenameColumn(
                name: "vendor",
                table: "asset_maintenances",
                newName: "Vendor");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "asset_maintenances",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "cost",
                table: "asset_maintenances",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "asset_maintenances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "asset_maintenances",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "performed_by",
                table: "asset_maintenances",
                newName: "PerformedBy");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "asset_maintenances",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "next_maintenance_due",
                table: "asset_maintenances",
                newName: "NextMaintenanceDue");

            migrationBuilder.RenameColumn(
                name: "maintenance_type",
                table: "asset_maintenances",
                newName: "MaintenanceType");

            migrationBuilder.RenameColumn(
                name: "maintenance_date",
                table: "asset_maintenances",
                newName: "MaintenanceDate");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "asset_maintenances",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "asset_maintenances",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "capitalize_cost",
                table: "asset_maintenances",
                newName: "CapitalizeCost");

            migrationBuilder.RenameColumn(
                name: "asset_id",
                table: "asset_maintenances",
                newName: "AssetId");

            migrationBuilder.RenameIndex(
                name: "ix_asset_maintenances_asset_id",
                table: "asset_maintenances",
                newName: "IX_asset_maintenances_AssetId");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "asset_disposals",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "asset_disposals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "asset_disposals",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "asset_disposals",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "net_book_value_at_disposal",
                table: "asset_disposals",
                newName: "NetBookValueAtDisposal");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "asset_disposals",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "asset_disposals",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "gl_gain_loss_account_id",
                table: "asset_disposals",
                newName: "GLGainLossAccountId");

            migrationBuilder.RenameColumn(
                name: "disposed_by",
                table: "asset_disposals",
                newName: "DisposedBy");

            migrationBuilder.RenameColumn(
                name: "disposal_type",
                table: "asset_disposals",
                newName: "DisposalType");

            migrationBuilder.RenameColumn(
                name: "disposal_proceeds",
                table: "asset_disposals",
                newName: "DisposalProceeds");

            migrationBuilder.RenameColumn(
                name: "disposal_date",
                table: "asset_disposals",
                newName: "DisposalDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "asset_disposals",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "buyer_name",
                table: "asset_disposals",
                newName: "BuyerName");

            migrationBuilder.RenameColumn(
                name: "asset_id",
                table: "asset_disposals",
                newName: "AssetId");

            migrationBuilder.RenameIndex(
                name: "ix_asset_disposals_asset_id",
                table: "asset_disposals",
                newName: "IX_asset_disposals_AssetId");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "asset_depreciations",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "asset_depreciations",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "asset_depreciations",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "asset_depreciations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "asset_depreciations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "running_nbv",
                table: "asset_depreciations",
                newName: "RunningNBV");

            migrationBuilder.RenameColumn(
                name: "posted_by",
                table: "asset_depreciations",
                newName: "PostedBy");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "asset_depreciations",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "asset_depreciations",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "asset_depreciations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "asset_id",
                table: "asset_depreciations",
                newName: "AssetId");

            migrationBuilder.RenameIndex(
                name: "ix_asset_depreciations_asset_id",
                table: "asset_depreciations",
                newName: "IX_asset_depreciations_AssetId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ar_payments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "ar_payments",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "ar_payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ar_payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ar_payments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "payment_number",
                table: "ar_payments",
                newName: "PaymentNumber");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "ar_payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "ar_payments",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "ar_payments",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "ar_payments",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ar_payments",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "ar_payments",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ar_payments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ar_invoice_id",
                table: "ar_payments",
                newName: "ARInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_ar_payments_customer_id",
                table: "ar_payments",
                newName: "IX_ar_payments_CustomerId");

            migrationBuilder.RenameIndex(
                name: "ix_ar_payments_ar_invoice_id",
                table: "ar_payments",
                newName: "IX_ar_payments_ARInvoiceId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ar_invoices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ar_invoices",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ar_invoices",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "ar_invoices",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ar_invoices",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "ar_invoices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "ar_invoices",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "ar_invoices",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "sales_order_id",
                table: "ar_invoices",
                newName: "SalesOrderId");

            migrationBuilder.RenameColumn(
                name: "paid_amount",
                table: "ar_invoices",
                newName: "PaidAmount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "ar_invoices",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "ar_invoices",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ar_invoices",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "invoice_number",
                table: "ar_invoices",
                newName: "InvoiceNumber");

            migrationBuilder.RenameColumn(
                name: "invoice_date",
                table: "ar_invoices",
                newName: "InvoiceDate");

            migrationBuilder.RenameColumn(
                name: "due_date",
                table: "ar_invoices",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "ar_invoices",
                newName: "DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "ar_invoices",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ar_invoices",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_ar_invoices_sales_order_id",
                table: "ar_invoices",
                newName: "IX_ar_invoices_SalesOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_ar_invoices_organization_id_invoice_number",
                table: "ar_invoices",
                newName: "IX_ar_invoices_OrganizationId_InvoiceNumber");

            migrationBuilder.RenameIndex(
                name: "ix_ar_invoices_customer_id",
                table: "ar_invoices",
                newName: "IX_ar_invoices_CustomerId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "app_users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "app_users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "app_users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "app_users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "app_users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "refresh_token_expiry",
                table: "app_users",
                newName: "RefreshTokenExpiry");

            migrationBuilder.RenameColumn(
                name: "refresh_token",
                table: "app_users",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "app_users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "app_users",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "locked_until",
                table: "app_users",
                newName: "LockedUntil");

            migrationBuilder.RenameColumn(
                name: "last_login_at",
                table: "app_users",
                newName: "LastLoginAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "app_users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "app_users",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "failed_login_attempts",
                table: "app_users",
                newName: "FailedLoginAttempts");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "app_users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_app_users_organization_id_username",
                table: "app_users",
                newName: "IX_app_users_OrganizationId_Username");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ap_payments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reference",
                table: "ap_payments",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "ap_payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ap_payments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "ap_payments",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ap_payments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "payment_number",
                table: "ap_payments",
                newName: "PaymentNumber");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "ap_payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "ap_payments",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "ap_payments",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "ap_payments",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ap_payments",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ap_payments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ap_invoice_id",
                table: "ap_payments",
                newName: "APInvoiceId");

            migrationBuilder.RenameIndex(
                name: "ix_ap_payments_vendor_id",
                table: "ap_payments",
                newName: "IX_ap_payments_VendorId");

            migrationBuilder.RenameIndex(
                name: "ix_ap_payments_ap_invoice_id",
                table: "ap_payments",
                newName: "IX_ap_payments_APInvoiceId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ap_invoices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ap_invoices",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ap_invoices",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workflow_instance_id",
                table: "ap_invoices",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "vendor_invoice_ref",
                table: "ap_invoices",
                newName: "VendorInvoiceRef");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "ap_invoices",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ap_invoices",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "ap_invoices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "ap_invoices",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "ap_invoices",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "purchase_order_id",
                table: "ap_invoices",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "prepayment_applied",
                table: "ap_invoices",
                newName: "PrepaymentApplied");

            migrationBuilder.RenameColumn(
                name: "paid_amount",
                table: "ap_invoices",
                newName: "PaidAmount");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "ap_invoices",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "match_status",
                table: "ap_invoices",
                newName: "MatchStatus");

            migrationBuilder.RenameColumn(
                name: "match_notes",
                table: "ap_invoices",
                newName: "MatchNotes");

            migrationBuilder.RenameColumn(
                name: "linked_prepayment_invoice_id",
                table: "ap_invoices",
                newName: "LinkedPrepaymentInvoiceId");

            migrationBuilder.RenameColumn(
                name: "journal_entry_id",
                table: "ap_invoices",
                newName: "JournalEntryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ap_invoices",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "invoice_type",
                table: "ap_invoices",
                newName: "InvoiceType");

            migrationBuilder.RenameColumn(
                name: "invoice_number",
                table: "ap_invoices",
                newName: "InvoiceNumber");

            migrationBuilder.RenameColumn(
                name: "invoice_date",
                table: "ap_invoices",
                newName: "InvoiceDate");

            migrationBuilder.RenameColumn(
                name: "due_date",
                table: "ap_invoices",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ap_invoices",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bypass_reason",
                table: "ap_invoices",
                newName: "BypassReason");

            migrationBuilder.RenameIndex(
                name: "ix_ap_invoices_vendor_id",
                table: "ap_invoices",
                newName: "IX_ap_invoices_VendorId");

            migrationBuilder.RenameIndex(
                name: "ix_ap_invoices_purchase_order_id",
                table: "ap_invoices",
                newName: "IX_ap_invoices_PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_ap_invoices_organization_id_invoice_number",
                table: "ap_invoices",
                newName: "IX_ap_invoices_OrganizationId_InvoiceNumber");

            migrationBuilder.RenameIndex(
                name: "ix_ap_invoices_linked_prepayment_invoice_id",
                table: "ap_invoices",
                newName: "IX_ap_invoices_LinkedPrepaymentInvoiceId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "accounts",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "accounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "accounts",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "accounts",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "accounts",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "accounts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "parent_account_id",
                table: "accounts",
                newName: "ParentAccountId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "accounts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_header_account",
                table: "accounts",
                newName: "IsHeaderAccount");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "accounts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "accounts",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "allow_manual_entry",
                table: "accounts",
                newName: "AllowManualEntry");

            migrationBuilder.RenameColumn(
                name: "account_type_id",
                table: "accounts",
                newName: "AccountTypeId");

            migrationBuilder.RenameColumn(
                name: "account_number",
                table: "accounts",
                newName: "AccountNumber");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_parent_account_id",
                table: "accounts",
                newName: "IX_accounts_ParentAccountId");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_organization_id_account_number",
                table: "accounts",
                newName: "IX_accounts_OrganizationId_AccountNumber");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_account_type_id",
                table: "accounts",
                newName: "IX_accounts_AccountTypeId");

            migrationBuilder.RenameColumn(
                name: "nature",
                table: "account_types",
                newName: "Nature");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "account_types",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "account_types",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "account_types",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "account_types",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "account_types",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "display_order",
                table: "account_types",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "account_types",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "WarehouseTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "WarehouseTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "WarehouseTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "WarehouseTypes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "WarehouseTypes",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "WarehouseTypes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "WarehouseTypes",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "WarehouseTypes",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_warehouse_types_organization_id_name",
                table: "WarehouseTypes",
                newName: "IX_WarehouseTypes_OrganizationId_Name");

            migrationBuilder.RenameColumn(
                name: "zone",
                table: "WarehouseLocations",
                newName: "Zone");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "WarehouseLocations",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "WarehouseLocations",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "bin",
                table: "WarehouseLocations",
                newName: "Bin");

            migrationBuilder.RenameColumn(
                name: "bay",
                table: "WarehouseLocations",
                newName: "Bay");

            migrationBuilder.RenameColumn(
                name: "aisle",
                table: "WarehouseLocations",
                newName: "Aisle");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "WarehouseLocations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "WarehouseLocations",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "WarehouseLocations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_receivable",
                table: "WarehouseLocations",
                newName: "IsReceivable");

            migrationBuilder.RenameColumn(
                name: "is_pickable",
                table: "WarehouseLocations",
                newName: "IsPickable");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "WarehouseLocations",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "WarehouseLocations",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "WarehouseLocations",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_warehouse_locations_warehouse_id_code",
                table: "WarehouseLocations",
                newName: "IX_WarehouseLocations_WarehouseId_Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "WarehouseInventoryBalances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "warehouse_location_id",
                table: "WarehouseInventoryBalances",
                newName: "WarehouseLocationId");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "WarehouseInventoryBalances",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "WarehouseInventoryBalances",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "quantity_reserved",
                table: "WarehouseInventoryBalances",
                newName: "QuantityReserved");

            migrationBuilder.RenameColumn(
                name: "quantity_on_hand",
                table: "WarehouseInventoryBalances",
                newName: "QuantityOnHand");

            migrationBuilder.RenameColumn(
                name: "product_variant_id",
                table: "WarehouseInventoryBalances",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "WarehouseInventoryBalances",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "WarehouseInventoryBalances",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "WarehouseInventoryBalances",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_warehouse_inventory_balances_warehouse_location_id",
                table: "WarehouseInventoryBalances",
                newName: "IX_WarehouseInventoryBalances_WarehouseLocationId");

            migrationBuilder.RenameIndex(
                name: "ix_warehouse_inventory_balances_warehouse_id",
                table: "WarehouseInventoryBalances",
                newName: "IX_WarehouseInventoryBalances_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "ix_warehouse_inventory_balances_product_variant_id",
                table: "WarehouseInventoryBalances",
                newName: "IX_WarehouseInventoryBalances_ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "ix_warehouse_inventory_balances_organization_id_product_varian",
                table: "WarehouseInventoryBalances",
                newName: "IX_WarehouseInventoryBalances_OrganizationId_ProductVariantId_W");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "TransferOrders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "TransferOrders",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TransferOrders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "TransferOrders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "to_warehouse_id",
                table: "TransferOrders",
                newName: "ToWarehouseId");

            migrationBuilder.RenameColumn(
                name: "shipped_date",
                table: "TransferOrders",
                newName: "ShippedDate");

            migrationBuilder.RenameColumn(
                name: "requested_date",
                table: "TransferOrders",
                newName: "RequestedDate");

            migrationBuilder.RenameColumn(
                name: "received_date",
                table: "TransferOrders",
                newName: "ReceivedDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "TransferOrders",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "TransferOrders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "TransferOrders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "from_warehouse_id",
                table: "TransferOrders",
                newName: "FromWarehouseId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "TransferOrders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_transfer_orders_to_warehouse_id",
                table: "TransferOrders",
                newName: "IX_TransferOrders_ToWarehouseId");

            migrationBuilder.RenameIndex(
                name: "ix_transfer_orders_organization_id_order_number",
                table: "TransferOrders",
                newName: "IX_TransferOrders_OrganizationId_OrderNumber");

            migrationBuilder.RenameIndex(
                name: "ix_transfer_orders_from_warehouse_id",
                table: "TransferOrders",
                newName: "IX_TransferOrders_FromWarehouseId");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "TransferOrderLines",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TransferOrderLines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "TransferOrderLines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "TransferOrderLines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "transfer_order_id",
                table: "TransferOrderLines",
                newName: "TransferOrderId");

            migrationBuilder.RenameColumn(
                name: "to_location_id",
                table: "TransferOrderLines",
                newName: "ToLocationId");

            migrationBuilder.RenameColumn(
                name: "shipped_quantity",
                table: "TransferOrderLines",
                newName: "ShippedQuantity");

            migrationBuilder.RenameColumn(
                name: "requested_quantity",
                table: "TransferOrderLines",
                newName: "RequestedQuantity");

            migrationBuilder.RenameColumn(
                name: "received_quantity",
                table: "TransferOrderLines",
                newName: "ReceivedQuantity");

            migrationBuilder.RenameColumn(
                name: "product_sku",
                table: "TransferOrderLines",
                newName: "ProductSku");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "TransferOrderLines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "TransferOrderLines",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "lot_number",
                table: "TransferOrderLines",
                newName: "LotNumber");

            migrationBuilder.RenameColumn(
                name: "line_number",
                table: "TransferOrderLines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "TransferOrderLines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "from_location_id",
                table: "TransferOrderLines",
                newName: "FromLocationId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "TransferOrderLines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_transfer_order_lines_transfer_order_id",
                table: "TransferOrderLines",
                newName: "IX_TransferOrderLines_TransferOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_transfer_order_lines_to_location_id",
                table: "TransferOrderLines",
                newName: "IX_TransferOrderLines_ToLocationId");

            migrationBuilder.RenameIndex(
                name: "ix_transfer_order_lines_from_location_id",
                table: "TransferOrderLines",
                newName: "IX_TransferOrderLines_FromLocationId");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "PriceAgreements",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "PriceAgreements",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "PriceAgreements",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "PriceAgreements",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "PriceAgreements",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "PriceAgreements",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PriceAgreements",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "variant_id",
                table: "PriceAgreements",
                newName: "VariantId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "PriceAgreements",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "PriceAgreements",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "PriceAgreements",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "price_type",
                table: "PriceAgreements",
                newName: "PriceType");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "PriceAgreements",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "PriceAgreements",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "PriceAgreements",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "PriceAgreements",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "PriceAgreements",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_price_agreements_variant_id_price_type_is_active",
                table: "PriceAgreements",
                newName: "IX_PriceAgreements_VariantId_PriceType_IsActive");

            migrationBuilder.RenameIndex(
                name: "ix_price_agreements_product_id_price_type_is_active",
                table: "PriceAgreements",
                newName: "IX_PriceAgreements_ProductId_PriceType_IsActive");

            migrationBuilder.RenameIndex(
                name: "ix_price_agreements_organization_id_is_active_start_date_end_d",
                table: "PriceAgreements",
                newName: "IX_PriceAgreements_OrganizationId_IsActive_StartDate_EndDate");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "OutboundOrders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "OutboundOrders",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "carrier",
                table: "OutboundOrders",
                newName: "Carrier");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OutboundOrders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "OutboundOrders",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "OutboundOrders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "tracking_number",
                table: "OutboundOrders",
                newName: "TrackingNumber");

            migrationBuilder.RenameColumn(
                name: "shipped_date",
                table: "OutboundOrders",
                newName: "ShippedDate");

            migrationBuilder.RenameColumn(
                name: "ship_to_address",
                table: "OutboundOrders",
                newName: "ShipToAddress");

            migrationBuilder.RenameColumn(
                name: "sales_order_id",
                table: "OutboundOrders",
                newName: "SalesOrderId");

            migrationBuilder.RenameColumn(
                name: "requested_date",
                table: "OutboundOrders",
                newName: "RequestedDate");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "OutboundOrders",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "OutboundOrders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "OutboundOrders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "customer_name",
                table: "OutboundOrders",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "OutboundOrders",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "OutboundOrders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_outbound_orders_warehouse_id",
                table: "OutboundOrders",
                newName: "IX_OutboundOrders_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "ix_outbound_orders_organization_id_order_number",
                table: "OutboundOrders",
                newName: "IX_OutboundOrders_OrganizationId_OrderNumber");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "OutboundOrderLines",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OutboundOrderLines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "OutboundOrderLines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "OutboundOrderLines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "shipped_quantity",
                table: "OutboundOrderLines",
                newName: "ShippedQuantity");

            migrationBuilder.RenameColumn(
                name: "requested_quantity",
                table: "OutboundOrderLines",
                newName: "RequestedQuantity");

            migrationBuilder.RenameColumn(
                name: "product_sku",
                table: "OutboundOrderLines",
                newName: "ProductSku");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "OutboundOrderLines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "OutboundOrderLines",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "picked_quantity",
                table: "OutboundOrderLines",
                newName: "PickedQuantity");

            migrationBuilder.RenameColumn(
                name: "outbound_order_id",
                table: "OutboundOrderLines",
                newName: "OutboundOrderId");

            migrationBuilder.RenameColumn(
                name: "lot_number",
                table: "OutboundOrderLines",
                newName: "LotNumber");

            migrationBuilder.RenameColumn(
                name: "line_number",
                table: "OutboundOrderLines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "OutboundOrderLines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "from_location_id",
                table: "OutboundOrderLines",
                newName: "FromLocationId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "OutboundOrderLines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_outbound_order_lines_outbound_order_id",
                table: "OutboundOrderLines",
                newName: "IX_OutboundOrderLines_OutboundOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_outbound_order_lines_from_location_id",
                table: "OutboundOrderLines",
                newName: "IX_OutboundOrderLines_FromLocationId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "OperationalSites",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "OperationalSites",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "OperationalSites",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "OperationalSites",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "OperationalSites",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OperationalSites",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "OperationalSites",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "OperationalSites",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_warehouse",
                table: "OperationalSites",
                newName: "IsWarehouse");

            migrationBuilder.RenameColumn(
                name: "is_return_center",
                table: "OperationalSites",
                newName: "IsReturnCenter");

            migrationBuilder.RenameColumn(
                name: "is_retail_store",
                table: "OperationalSites",
                newName: "IsRetailStore");

            migrationBuilder.RenameColumn(
                name: "is_fulfillment_center",
                table: "OperationalSites",
                newName: "IsFulfillmentCenter");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "OperationalSites",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "OperationalSites",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "OperationalSites",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_operational_sites_organization_id_code",
                table: "OperationalSites",
                newName: "IX_OperationalSites_OrganizationId_Code");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "LoyaltyPrograms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "LoyaltyPrograms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "LoyaltyPrograms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "LoyaltyPrograms",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "silver_threshold",
                table: "LoyaltyPrograms",
                newName: "SilverThreshold");

            migrationBuilder.RenameColumn(
                name: "redemption_threshold",
                table: "LoyaltyPrograms",
                newName: "RedemptionThreshold");

            migrationBuilder.RenameColumn(
                name: "points_per_dollar",
                table: "LoyaltyPrograms",
                newName: "PointsPerDollar");

            migrationBuilder.RenameColumn(
                name: "platinum_threshold",
                table: "LoyaltyPrograms",
                newName: "PlatinumThreshold");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "LoyaltyPrograms",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "LoyaltyPrograms",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "gold_threshold",
                table: "LoyaltyPrograms",
                newName: "GoldThreshold");

            migrationBuilder.RenameColumn(
                name: "dollar_per_point",
                table: "LoyaltyPrograms",
                newName: "DollarPerPoint");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "LoyaltyPrograms",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_loyalty_programs_organization_id",
                table: "LoyaltyPrograms",
                newName: "IX_LoyaltyPrograms_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "InboundOrders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "InboundOrders",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "InboundOrders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                table: "InboundOrders",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "vendor_name",
                table: "InboundOrders",
                newName: "VendorName");

            migrationBuilder.RenameColumn(
                name: "vendor_id",
                table: "InboundOrders",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "InboundOrders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "received_date",
                table: "InboundOrders",
                newName: "ReceivedDate");

            migrationBuilder.RenameColumn(
                name: "purchase_order_id",
                table: "InboundOrders",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "InboundOrders",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "InboundOrders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "InboundOrders",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "expected_date",
                table: "InboundOrders",
                newName: "ExpectedDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "InboundOrders",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_inbound_orders_warehouse_id",
                table: "InboundOrders",
                newName: "IX_InboundOrders_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "ix_inbound_orders_organization_id_order_number",
                table: "InboundOrders",
                newName: "IX_InboundOrders_OrganizationId_OrderNumber");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "InboundOrderLines",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "InboundOrderLines",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "InboundOrderLines",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "unit_of_measure",
                table: "InboundOrderLines",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "received_quantity",
                table: "InboundOrderLines",
                newName: "ReceivedQuantity");

            migrationBuilder.RenameColumn(
                name: "product_sku",
                table: "InboundOrderLines",
                newName: "ProductSku");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "InboundOrderLines",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "InboundOrderLines",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ordered_quantity",
                table: "InboundOrderLines",
                newName: "OrderedQuantity");

            migrationBuilder.RenameColumn(
                name: "lot_number",
                table: "InboundOrderLines",
                newName: "LotNumber");

            migrationBuilder.RenameColumn(
                name: "location_id",
                table: "InboundOrderLines",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "line_number",
                table: "InboundOrderLines",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "InboundOrderLines",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "inbound_order_id",
                table: "InboundOrderLines",
                newName: "InboundOrderId");

            migrationBuilder.RenameColumn(
                name: "expiry_date",
                table: "InboundOrderLines",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "InboundOrderLines",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_inbound_order_lines_location_id",
                table: "InboundOrderLines",
                newName: "IX_InboundOrderLines_LocationId");

            migrationBuilder.RenameIndex(
                name: "ix_inbound_order_lines_inbound_order_id",
                table: "InboundOrderLines",
                newName: "IX_InboundOrderLines_InboundOrderId");

            migrationBuilder.RenameColumn(
                name: "tier",
                table: "CustomerLoyaltyAccounts",
                newName: "Tier");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CustomerLoyaltyAccounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "CustomerLoyaltyAccounts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "total_points",
                table: "CustomerLoyaltyAccounts",
                newName: "TotalPoints");

            migrationBuilder.RenameColumn(
                name: "redeemed_points",
                table: "CustomerLoyaltyAccounts",
                newName: "RedeemedPoints");

            migrationBuilder.RenameColumn(
                name: "organization_id",
                table: "CustomerLoyaltyAccounts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "loyalty_program_id",
                table: "CustomerLoyaltyAccounts",
                newName: "LoyaltyProgramId");

            migrationBuilder.RenameColumn(
                name: "last_activity_at",
                table: "CustomerLoyaltyAccounts",
                newName: "LastActivityAt");

            migrationBuilder.RenameColumn(
                name: "customer_name",
                table: "CustomerLoyaltyAccounts",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "CustomerLoyaltyAccounts",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "customer_email",
                table: "CustomerLoyaltyAccounts",
                newName: "CustomerEmail");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "CustomerLoyaltyAccounts",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_customer_loyalty_accounts_organization_id_customer_id",
                table: "CustomerLoyaltyAccounts",
                newName: "IX_CustomerLoyaltyAccounts_OrganizationId_CustomerId");

            migrationBuilder.RenameIndex(
                name: "ix_customer_loyalty_accounts_loyalty_program_id",
                table: "CustomerLoyaltyAccounts",
                newName: "IX_CustomerLoyaltyAccounts_LoyaltyProgramId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workflow_templates",
                table: "workflow_templates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workflow_template_steps",
                table: "workflow_template_steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workflow_instances",
                table: "workflow_instances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workflow_approval_steps",
                table: "workflow_approval_steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendors",
                table: "vendors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendor_credit_notes",
                table: "vendor_credit_notes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendor_contacts",
                table: "vendor_contacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendor_addresses",
                table: "vendor_addresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_quotations",
                table: "sales_quotations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_quotation_lines",
                table: "sales_quotation_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_orders",
                table: "sales_orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_order_lines",
                table: "sales_order_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                table: "roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permissions",
                table: "role_permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_retail_transaction_staging_tenders",
                table: "retail_transaction_staging_tenders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_retail_transaction_staging_lines",
                table: "retail_transaction_staging_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_retail_transaction_staging",
                table: "retail_transaction_staging",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_retail_tender_settlements",
                table: "retail_tender_settlements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_retail_stores",
                table: "retail_stores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_retail_statements",
                table: "retail_statements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase_requisitions",
                table: "purchase_requisitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase_requisition_lines",
                table: "purchase_requisition_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase_orders",
                table: "purchase_orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase_order_receipts",
                table: "purchase_order_receipts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase_order_receipt_lines",
                table: "purchase_order_receipt_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_purchase_order_lines",
                table: "purchase_order_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_promotions",
                table: "promotions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_variants",
                table: "product_variants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pos_transactions",
                table: "pos_transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pos_transaction_lines",
                table: "pos_transaction_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pos_payments",
                table: "pos_payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payment_proposals",
                table: "payment_proposals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payment_proposal_lines",
                table: "payment_proposal_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations",
                table: "organizations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_journal_lines",
                table: "journal_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_journal_entries",
                table: "journal_entries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventory_transactions",
                table: "inventory_transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventory_records",
                table: "inventory_records",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_import_jobs",
                table: "import_jobs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_import_job_rows",
                table: "import_job_rows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fixed_assets",
                table: "fixed_assets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fiscal_years",
                table: "fiscal_years",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fiscal_periods",
                table: "fiscal_periods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_export_job_rows",
                table: "export_job_rows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expense_reports",
                table: "expense_reports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expense_lines",
                table: "expense_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expense_categories",
                table: "expense_categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dunning_records",
                table: "dunning_records",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer_credit_notes",
                table: "customer_credit_notes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer_contacts",
                table: "customer_contacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer_addresses",
                table: "customer_addresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_currencies",
                table: "currencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_coupons",
                table: "coupons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_coupon_redemptions",
                table: "coupon_redemptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_catalog_products",
                table: "catalog_products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cash_journals",
                table: "cash_journals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cash_journal_lines",
                table: "cash_journal_lines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_brands",
                table: "brands",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_batch_job_configs",
                table: "batch_job_configs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_transactions",
                table: "bank_transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_reconciliations",
                table: "bank_reconciliations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_accounts",
                table: "bank_accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_audit_logs",
                table: "audit_logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asset_transfers",
                table: "asset_transfers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asset_maintenances",
                table: "asset_maintenances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asset_disposals",
                table: "asset_disposals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asset_depreciations",
                table: "asset_depreciations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ar_payments",
                table: "ar_payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ar_invoices",
                table: "ar_invoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_app_users",
                table: "app_users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ap_payments",
                table: "ap_payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ap_invoices",
                table: "ap_invoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_account_types",
                table: "account_types",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseTypes",
                table: "WarehouseTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseLocations",
                table: "WarehouseLocations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseInventoryBalances",
                table: "WarehouseInventoryBalances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransferOrders",
                table: "TransferOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransferOrderLines",
                table: "TransferOrderLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceAgreements",
                table: "PriceAgreements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboundOrders",
                table: "OutboundOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboundOrderLines",
                table: "OutboundOrderLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperationalSites",
                table: "OperationalSites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoyaltyPrograms",
                table: "LoyaltyPrograms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboundOrders",
                table: "InboundOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboundOrderLines",
                table: "InboundOrderLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerLoyaltyAccounts",
                table: "CustomerLoyaltyAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_account_types_AccountTypeId",
                table: "accounts",
                column: "AccountTypeId",
                principalTable: "account_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_accounts_ParentAccountId",
                table: "accounts",
                column: "ParentAccountId",
                principalTable: "accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ap_invoices_ap_invoices_LinkedPrepaymentInvoiceId",
                table: "ap_invoices",
                column: "LinkedPrepaymentInvoiceId",
                principalTable: "ap_invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ap_invoices_purchase_orders_PurchaseOrderId",
                table: "ap_invoices",
                column: "PurchaseOrderId",
                principalTable: "purchase_orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ap_invoices_vendors_VendorId",
                table: "ap_invoices",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ap_payments_ap_invoices_APInvoiceId",
                table: "ap_payments",
                column: "APInvoiceId",
                principalTable: "ap_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ap_payments_vendors_VendorId",
                table: "ap_payments",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ar_invoices_customers_CustomerId",
                table: "ar_invoices",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ar_invoices_sales_orders_SalesOrderId",
                table: "ar_invoices",
                column: "SalesOrderId",
                principalTable: "sales_orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ar_payments_ar_invoices_ARInvoiceId",
                table: "ar_payments",
                column: "ARInvoiceId",
                principalTable: "ar_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ar_payments_customers_CustomerId",
                table: "ar_payments",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_depreciations_fixed_assets_AssetId",
                table: "asset_depreciations",
                column: "AssetId",
                principalTable: "fixed_assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_disposals_fixed_assets_AssetId",
                table: "asset_disposals",
                column: "AssetId",
                principalTable: "fixed_assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_maintenances_fixed_assets_AssetId",
                table: "asset_maintenances",
                column: "AssetId",
                principalTable: "fixed_assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_asset_transfers_fixed_assets_AssetId",
                table: "asset_transfers",
                column: "AssetId",
                principalTable: "fixed_assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bank_reconciliations_bank_accounts_BankAccountId",
                table: "bank_reconciliations",
                column: "BankAccountId",
                principalTable: "bank_accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bank_transactions_bank_accounts_BankAccountId",
                table: "bank_transactions",
                column: "BankAccountId",
                principalTable: "bank_accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cash_journal_lines_cash_journals_JournalId",
                table: "cash_journal_lines",
                column: "JournalId",
                principalTable: "cash_journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cash_journals_bank_accounts_BankAccountId",
                table: "cash_journals",
                column: "BankAccountId",
                principalTable: "bank_accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_products_brands_BrandId",
                table: "catalog_products",
                column: "BrandId",
                principalTable: "brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_products_categories_CategoryId",
                table: "catalog_products",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_products_vendors_PreferredVendorId",
                table: "catalog_products",
                column: "PreferredVendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_ParentCategoryId",
                table: "categories",
                column: "ParentCategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_coupons_promotions_PromotionId",
                table: "coupons",
                column: "PromotionId",
                principalTable: "promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_addresses_customers_CustomerId",
                table: "customer_addresses",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_contacts_customers_CustomerId",
                table: "customer_contacts",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_credit_notes_ar_invoices_ARInvoiceId",
                table: "customer_credit_notes",
                column: "ARInvoiceId",
                principalTable: "ar_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_credit_notes_customers_CustomerId",
                table: "customer_credit_notes",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerLoyaltyAccounts_LoyaltyPrograms_LoyaltyProgramId",
                table: "CustomerLoyaltyAccounts",
                column: "LoyaltyProgramId",
                principalTable: "LoyaltyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dunning_records_ar_invoices_ARInvoiceId",
                table: "dunning_records",
                column: "ARInvoiceId",
                principalTable: "ar_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dunning_records_customers_CustomerId",
                table: "dunning_records",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expense_lines_expense_reports_ExpenseReportId",
                table: "expense_lines",
                column: "ExpenseReportId",
                principalTable: "expense_reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fiscal_periods_fiscal_years_FiscalYearId",
                table: "fiscal_periods",
                column: "FiscalYearId",
                principalTable: "fiscal_years",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_import_job_rows_import_jobs_ImportJobId",
                table: "import_job_rows",
                column: "ImportJobId",
                principalTable: "import_jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboundOrderLines_InboundOrders_InboundOrderId",
                table: "InboundOrderLines",
                column: "InboundOrderId",
                principalTable: "InboundOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboundOrderLines_WarehouseLocations_LocationId",
                table: "InboundOrderLines",
                column: "LocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundOrders_Warehouses_WarehouseId",
                table: "InboundOrders",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_records_product_variants_ProductVariantId",
                table: "inventory_records",
                column: "ProductVariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_transactions_product_variants_ProductVariantId",
                table: "inventory_transactions",
                column: "ProductVariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_journal_entries_fiscal_periods_FiscalPeriodId",
                table: "journal_entries",
                column: "FiscalPeriodId",
                principalTable: "fiscal_periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_journal_lines_accounts_AccountId",
                table: "journal_lines",
                column: "AccountId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_journal_lines_journal_entries_JournalEntryId",
                table: "journal_lines",
                column: "JournalEntryId",
                principalTable: "journal_entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrderLines_OutboundOrders_OutboundOrderId",
                table: "OutboundOrderLines",
                column: "OutboundOrderId",
                principalTable: "OutboundOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrderLines_WarehouseLocations_FromLocationId",
                table: "OutboundOrderLines",
                column: "FromLocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutboundOrders_Warehouses_WarehouseId",
                table: "OutboundOrders",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_proposal_lines_ap_invoices_APInvoiceId",
                table: "payment_proposal_lines",
                column: "APInvoiceId",
                principalTable: "ap_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_proposal_lines_ap_payments_APPaymentId",
                table: "payment_proposal_lines",
                column: "APPaymentId",
                principalTable: "ap_payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_proposal_lines_payment_proposals_ProposalId",
                table: "payment_proposal_lines",
                column: "ProposalId",
                principalTable: "payment_proposals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pos_payments_pos_transactions_POSTransactionId",
                table: "pos_payments",
                column: "POSTransactionId",
                principalTable: "pos_transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pos_transaction_lines_pos_transactions_POSTransactionId",
                table: "pos_transaction_lines",
                column: "POSTransactionId",
                principalTable: "pos_transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceAgreements_catalog_products_ProductId",
                table: "PriceAgreements",
                column: "ProductId",
                principalTable: "catalog_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceAgreements_product_variants_VariantId",
                table: "PriceAgreements",
                column: "VariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_product_variants_catalog_products_ProductId",
                table: "product_variants",
                column: "ProductId",
                principalTable: "catalog_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_lines_product_variants_ProductVariantId",
                table: "purchase_order_lines",
                column: "ProductVariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_lines_purchase_orders_PurchaseOrderId",
                table: "purchase_order_lines",
                column: "PurchaseOrderId",
                principalTable: "purchase_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_receipt_lines_purchase_order_lines_PurchaseO~",
                table: "purchase_order_receipt_lines",
                column: "PurchaseOrderLineId",
                principalTable: "purchase_order_lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_receipt_lines_purchase_order_receipts_Receip~",
                table: "purchase_order_receipt_lines",
                column: "ReceiptId",
                principalTable: "purchase_order_receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_receipts_WarehouseLocations_WarehouseLocation",
                table: "purchase_order_receipts",
                column: "WarehouseLocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_receipts_Warehouses_WarehouseId",
                table: "purchase_order_receipts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_order_receipts_purchase_orders_PurchaseOrderId",
                table: "purchase_order_receipts",
                column: "PurchaseOrderId",
                principalTable: "purchase_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_orders_Warehouses_WarehouseId",
                table: "purchase_orders",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_orders_vendors_VendorId",
                table: "purchase_orders",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_requisition_lines_purchase_requisitions_Requisitio~",
                table: "purchase_requisition_lines",
                column: "RequisitionId",
                principalTable: "purchase_requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_retail_transaction_staging_lines_retail_transaction_staging~",
                table: "retail_transaction_staging_lines",
                column: "RetailTransactionStagingId",
                principalTable: "retail_transaction_staging",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_retail_transaction_staging_tenders_retail_transaction_stagi~",
                table: "retail_transaction_staging_tenders",
                column: "RetailTransactionStagingId",
                principalTable: "retail_transaction_staging",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permissions_roles_RoleId",
                table: "role_permissions",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_order_lines_product_variants_ProductVariantId",
                table: "sales_order_lines",
                column: "ProductVariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_order_lines_sales_orders_SalesOrderId",
                table: "sales_order_lines",
                column: "SalesOrderId",
                principalTable: "sales_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_orders_customers_CustomerId",
                table: "sales_orders",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_quotation_lines_sales_quotations_QuotationId",
                table: "sales_quotation_lines",
                column: "QuotationId",
                principalTable: "sales_quotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_quotations_customers_CustomerId",
                table: "sales_quotations",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrderLines_TransferOrders_TransferOrderId",
                table: "TransferOrderLines",
                column: "TransferOrderId",
                principalTable: "TransferOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrderLines_WarehouseLocations_FromLocationId",
                table: "TransferOrderLines",
                column: "FromLocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrderLines_WarehouseLocations_ToLocationId",
                table: "TransferOrderLines",
                column: "ToLocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrders_Warehouses_FromWarehouseId",
                table: "TransferOrders",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrders_Warehouses_ToWarehouseId",
                table: "TransferOrders",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_app_users_UserId",
                table: "user_roles",
                column: "UserId",
                principalTable: "app_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_roles_RoleId",
                table: "user_roles",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vendor_addresses_vendors_VendorId",
                table: "vendor_addresses",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vendor_contacts_vendors_VendorId",
                table: "vendor_contacts",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vendor_credit_notes_ap_invoices_APInvoiceId",
                table: "vendor_credit_notes",
                column: "APInvoiceId",
                principalTable: "ap_invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_vendor_credit_notes_vendors_VendorId",
                table: "vendor_credit_notes",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseInventoryBalances_WarehouseLocations_WarehouseLocat",
                table: "WarehouseInventoryBalances",
                column: "WarehouseLocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseInventoryBalances_Warehouses_WarehouseId",
                table: "WarehouseInventoryBalances",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseInventoryBalances_product_variants_ProductVariantId",
                table: "WarehouseInventoryBalances",
                column: "ProductVariantId",
                principalTable: "product_variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseLocations_Warehouses_WarehouseId",
                table: "WarehouseLocations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_OperationalSites_SiteId",
                table: "Warehouses",
                column: "SiteId",
                principalTable: "OperationalSites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_WarehouseTypes_WarehouseTypeId",
                table: "Warehouses",
                column: "WarehouseTypeId",
                principalTable: "WarehouseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_approval_steps_workflow_instances_WorkflowInstance~",
                table: "workflow_approval_steps",
                column: "WorkflowInstanceId",
                principalTable: "workflow_instances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_instances_workflow_templates_TemplateId",
                table: "workflow_instances",
                column: "TemplateId",
                principalTable: "workflow_templates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_workflow_template_steps_workflow_templates_WorkflowTemplate~",
                table: "workflow_template_steps",
                column: "WorkflowTemplateId",
                principalTable: "workflow_templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
