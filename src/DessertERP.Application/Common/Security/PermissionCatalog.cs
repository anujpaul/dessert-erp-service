namespace DessertERP.Application.Common.Security;

public static class PermissionKeys
{
    public const string GlAccess = "gl:access";
    public const string GlJournalView = "gl.journal:view";
    public const string GlJournalManage = "gl.journal:manage";
    public const string GlJournalPost = "gl.journal:post";

    public const string ArAccess = "ar:access";
    public const string ArCustomerView = "ar.customer:view";
    public const string ArCustomerManage = "ar.customer:manage";
    public const string ArSalesOrderView = "ar.sales-order:view";
    public const string ArSalesOrderManage = "ar.sales-order:manage";
    public const string ArSalesOrderConfirm = "ar.sales-order:confirm";
    public const string ArSalesOrderShip = "ar.sales-order:ship";
    public const string ArInvoiceView = "ar.invoice:view";
    public const string ArInvoiceManage = "ar.invoice:manage";
    public const string ArInvoicePost = "ar.invoice:post";

    public const string ApAccess = "ap:access";
    public const string ApVendorView = "ap.vendor:view";
    public const string ApVendorManage = "ap.vendor:manage";
    public const string ApPurchaseOrderView = "ap.purchase-order:view";
    public const string ApPurchaseOrderManage = "ap.purchase-order:manage";
    public const string ApPurchaseOrderApprove = "ap.purchase-order:approve";
    public const string ApPurchaseOrderReceive = "ap.purchase-order:receive";
    public const string ApInvoiceView = "ap.invoice:view";
    public const string ApInvoiceManage = "ap.invoice:manage";
    public const string ApInvoiceApprove = "ap.invoice:approve";

    public const string ProductAccess = "product:access";
    public const string ProductCatalogView = "product.catalog:view";
    public const string ProductCatalogManage = "product.catalog:manage";

    public const string InventoryAccess = "inventory:access";
    public const string InventoryStockView = "inventory.stock:view";
    public const string InventoryStockAdjust = "inventory.stock:adjust";
    public const string WarehouseView = "inventory.warehouse:view";
    public const string WarehouseManage = "inventory.warehouse:manage";
    public const string WarehouseOperate = "inventory.warehouse:operate";

    public const string DataAccess = "data:access";
    public const string DataImport = "data.import:manage";
    public const string DataExport = "data.export:view";
    public const string DataJobsManage = "data.jobs:manage";

    public const string MarketingAccess = "marketing:access";
    public const string OmniChannelAccess = "omnichannel:access";
    public const string WorkflowAccess = "workflow:access";
    public const string ExpenseAccess = "expense:access";
    public const string CashBankAccess = "cash-bank:access";
    public const string FixedAssetsAccess = "fixed-assets:access";

    public const string SystemAccess = "system:access";
    public const string SystemUsersView = "system.users:view";
    public const string SystemUsersManage = "system.users:manage";
    public const string SystemRolesView = "system.roles:view";
    public const string SystemRolesManage = "system.roles:manage";
    public const string SystemAuditView = "system.audit:view";
    public const string SystemSettingsManage = "system.settings:manage";
}

public sealed record PermissionDefinition(
    string Key,
    string Area,
    string Resource,
    string Action,
    string Description);

public static class PermissionCatalog
{
    public static readonly IReadOnlyList<string> ModuleAccessKeys =
    [
        PermissionKeys.GlAccess,
        PermissionKeys.ArAccess,
        PermissionKeys.ApAccess,
        PermissionKeys.ProductAccess,
        PermissionKeys.InventoryAccess,
        PermissionKeys.DataAccess,
        PermissionKeys.MarketingAccess,
        PermissionKeys.OmniChannelAccess,
        PermissionKeys.WorkflowAccess,
        PermissionKeys.ExpenseAccess,
        PermissionKeys.CashBankAccess,
        PermissionKeys.FixedAssetsAccess,
        PermissionKeys.SystemAccess
    ];

    public static readonly IReadOnlyList<PermissionDefinition> All =
    [
        P(PermissionKeys.GlJournalView, "General Ledger", "Journals", "View", "View accounts, journals, periods, and reports"),
        P(PermissionKeys.GlJournalManage, "General Ledger", "Journals", "Manage", "Create and edit accounts, periods, and journals"),
        P(PermissionKeys.GlJournalPost, "General Ledger", "Journals", "Post", "Post or void journal entries"),

        P(PermissionKeys.ArCustomerView, "Accounts Receivable", "Customers", "View", "View customers and customer ledgers"),
        P(PermissionKeys.ArCustomerManage, "Accounts Receivable", "Customers", "Manage", "Create and edit customers"),
        P(PermissionKeys.ArSalesOrderView, "Accounts Receivable", "Sales orders", "View", "View sales orders and history"),
        P(PermissionKeys.ArSalesOrderManage, "Accounts Receivable", "Sales orders", "Manage", "Create and edit sales orders"),
        P(PermissionKeys.ArSalesOrderConfirm, "Accounts Receivable", "Sales orders", "Confirm", "Confirm, approve, reject, or cancel sales orders"),
        P(PermissionKeys.ArSalesOrderShip, "Accounts Receivable", "Sales orders", "Ship", "Pick and ship sales orders"),
        P(PermissionKeys.ArInvoiceView, "Accounts Receivable", "Invoices", "View", "View invoices, payments, aging, and credit activity"),
        P(PermissionKeys.ArInvoiceManage, "Accounts Receivable", "Invoices", "Manage", "Create invoices and payments"),
        P(PermissionKeys.ArInvoicePost, "Accounts Receivable", "Invoices", "Post", "Issue or void invoices"),

        P(PermissionKeys.ApVendorView, "Accounts Payable", "Vendors", "View", "View vendors and vendor ledgers"),
        P(PermissionKeys.ApVendorManage, "Accounts Payable", "Vendors", "Manage", "Create and edit vendors"),
        P(PermissionKeys.ApPurchaseOrderView, "Accounts Payable", "Purchase orders", "View", "View purchase orders and receipts"),
        P(PermissionKeys.ApPurchaseOrderManage, "Accounts Payable", "Purchase orders", "Manage", "Create and edit purchase orders"),
        P(PermissionKeys.ApPurchaseOrderApprove, "Accounts Payable", "Purchase orders", "Approve", "Approve, send, close, or cancel purchase orders"),
        P(PermissionKeys.ApPurchaseOrderReceive, "Accounts Payable", "Purchase orders", "Receive", "Receive purchase orders into inventory"),
        P(PermissionKeys.ApInvoiceView, "Accounts Payable", "Invoices", "View", "View invoices, payments, and aging"),
        P(PermissionKeys.ApInvoiceManage, "Accounts Payable", "Invoices", "Manage", "Create, match, and pay invoices"),
        P(PermissionKeys.ApInvoiceApprove, "Accounts Payable", "Invoices", "Approve", "Approve or void supplier invoices"),

        P(PermissionKeys.ProductCatalogView, "Product Management", "Catalog", "View", "View products, variants, brands, and categories"),
        P(PermissionKeys.ProductCatalogManage, "Product Management", "Catalog", "Manage", "Maintain products, variants, brands, and categories"),
        P(PermissionKeys.InventoryStockView, "Inventory", "Stock", "View", "View inventory balances and transactions"),
        P(PermissionKeys.InventoryStockAdjust, "Inventory", "Stock", "Adjust", "Adjust stock and inventory thresholds"),
        P(PermissionKeys.WarehouseView, "Inventory", "Warehouses", "View", "View warehouses and warehouse orders"),
        P(PermissionKeys.WarehouseManage, "Inventory", "Warehouses", "Manage", "Maintain warehouses and locations"),
        P(PermissionKeys.WarehouseOperate, "Inventory", "Warehouses", "Operate", "Receive, pick, ship, and transfer stock"),

        P(PermissionKeys.DataImport, "Data Management", "Imports", "Manage", "Upload, stage, and promote imports"),
        P(PermissionKeys.DataExport, "Data Management", "Exports", "View", "Export business data and templates"),
        P(PermissionKeys.DataJobsManage, "Data Management", "Batch jobs", "Manage", "Create, schedule, and trigger batch jobs"),

        P(PermissionKeys.MarketingAccess, "Marketing", "Marketing", "Access", "Use marketing and trade agreement features"),
        P(PermissionKeys.OmniChannelAccess, "OmniChannel", "OmniChannel", "Access", "Use channel order and fulfillment features"),
        P(PermissionKeys.WorkflowAccess, "Approvals", "Workflow", "Access", "Use approval inbox and workflow actions"),
        P(PermissionKeys.ExpenseAccess, "Expenses", "Expenses", "Access", "Use expense management"),
        P(PermissionKeys.CashBankAccess, "Cash and Bank", "Cash and bank", "Access", "Use cash and bank management"),
        P(PermissionKeys.FixedAssetsAccess, "Fixed Assets", "Fixed assets", "Access", "Use fixed asset management"),

        P(PermissionKeys.SystemUsersView, "System Administration", "Users", "View", "View users"),
        P(PermissionKeys.SystemUsersManage, "System Administration", "Users", "Manage", "Create, edit, activate, and reset users"),
        P(PermissionKeys.SystemRolesView, "System Administration", "Roles", "View", "View roles and permissions"),
        P(PermissionKeys.SystemRolesManage, "System Administration", "Roles", "Manage", "Create and edit roles"),
        P(PermissionKeys.SystemAuditView, "System Administration", "Audit", "View", "View audit history"),
        P(PermissionKeys.SystemSettingsManage, "System Administration", "Settings", "Manage", "Maintain organization settings")
    ];

    private static readonly IReadOnlyDictionary<string, string[]> ModuleCapabilities =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["GL"] = KeysFor("gl."),
            ["AR"] = KeysFor("ar."),
            ["AP"] = KeysFor("ap."),
            ["PM"] = [.. KeysFor("product."), .. KeysFor("inventory.")],
            ["SysAdmin"] = KeysFor("system.")
        };

    private static readonly IReadOnlyDictionary<string, string> AccessByPrefix =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["gl"] = PermissionKeys.GlAccess,
            ["ar"] = PermissionKeys.ArAccess,
            ["ap"] = PermissionKeys.ApAccess,
            ["product"] = PermissionKeys.ProductAccess,
            ["inventory"] = PermissionKeys.InventoryAccess,
            ["data"] = PermissionKeys.DataAccess,
            ["marketing"] = PermissionKeys.MarketingAccess,
            ["omnichannel"] = PermissionKeys.OmniChannelAccess,
            ["workflow"] = PermissionKeys.WorkflowAccess,
            ["expense"] = PermissionKeys.ExpenseAccess,
            ["cash-bank"] = PermissionKeys.CashBankAccess,
            ["fixed-assets"] = PermissionKeys.FixedAssetsAccess,
            ["system"] = PermissionKeys.SystemAccess
        };

    public static IReadOnlyCollection<string> Expand(IEnumerable<string> storedPermissions)
    {
        var expanded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var stored in storedPermissions)
        {
            var separator = stored.LastIndexOf(':');
            if (separator <= 0) continue;

            var resource = stored[..separator];
            var action = stored[(separator + 1)..];

            if (ModuleCapabilities.TryGetValue(resource, out var legacyKeys))
            {
                foreach (var key in ExpandLegacy(legacyKeys, action))
                    expanded.Add(key);
                continue;
            }

            expanded.Add($"{resource.ToLowerInvariant()}:{action.ToLowerInvariant()}");
        }

        var definedKeys = All.Select(permission => permission.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var permission in expanded.ToArray())
        {
            var separator = permission.LastIndexOf(':');
            if (separator <= 0 || permission.EndsWith(":view", StringComparison.OrdinalIgnoreCase))
                continue;

            var viewPermission = $"{permission[..separator]}:view";
            if (definedKeys.Contains(viewPermission))
                expanded.Add(viewPermission);
        }

        foreach (var prefix in expanded
                     .Select(KeyPrefix)
                     .Where(prefix => prefix is not null)
                     .Distinct(StringComparer.OrdinalIgnoreCase)
                     .ToArray())
        {
            if (prefix is not null && AccessByPrefix.TryGetValue(prefix, out var access))
                expanded.Add(access);
        }

        return expanded;
    }

    public static IReadOnlyCollection<string> ExpandForRoles(
        IEnumerable<string> storedPermissions,
        IEnumerable<string> roles)
    {
        if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            return PolicyKeys;

        return Expand(storedPermissions);
    }

    public static IReadOnlyCollection<string> PolicyKeys =>
        All.Select(p => p.Key).Concat(ModuleAccessKeys).Distinct().ToArray();

    private static IEnumerable<string> ExpandLegacy(IEnumerable<string> keys, string action) =>
        action.ToLowerInvariant() switch
        {
            "read" => keys.Where(IsViewLike),
            "write" => keys.Where(key => !IsViewLike(key)),
            "delete" => keys.Where(key => key.EndsWith(":manage", StringComparison.OrdinalIgnoreCase)),
            "approve" => keys.Where(key =>
                key.EndsWith(":approve", StringComparison.OrdinalIgnoreCase) ||
                key.EndsWith(":confirm", StringComparison.OrdinalIgnoreCase) ||
                key.EndsWith(":post", StringComparison.OrdinalIgnoreCase)),
            _ => []
        };

    private static bool IsViewLike(string key) =>
        key.EndsWith(":view", StringComparison.OrdinalIgnoreCase) ||
        key.EndsWith(":access", StringComparison.OrdinalIgnoreCase);

    private static string? KeyPrefix(string key)
    {
        var separator = key.IndexOfAny(['.', ':']);
        return separator > 0 ? key[..separator] : null;
    }

    private static string[] KeysFor(string prefix) =>
        All.Where(p => p.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(p => p.Key)
            .ToArray();

    private static PermissionDefinition P(
        string key, string area, string resource, string action, string description) =>
        new(key, area, resource, action, description);
}
