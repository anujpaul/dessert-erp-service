namespace DessertERP.Application.Modules.DataManagement.DTOs;

// ── Import Job ─────────────────────────────────────────────────────────────────

public record ImportJobDto(
    Guid   Id,
    string EntityType,
    string FileFormat,
    string FileName,
    string Status,
    int    TotalRows,
    int    SuccessRows,
    int    FailedRows,
    int    StagedRows,
    int    ValidRows,
    int    InvalidRows,
    int    PromotedRows,
    string? ErrorSummary,
    string? TriggeredBy,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    DateTime  CreatedAt
);

public record ImportJobRowDto(
    Guid    Id,
    int     RowNumber,
    string  Status,         // Pending | Valid | Invalid | Promoted | Skipped
    string? ErrorMessage,
    Guid?   PromotedEntityId,
    DateTime? PromotedAt
);

public record StartImportRequest(
    string EntityType,   // Vendor | Product | SalesOrder | PurchaseOrder
    string FileFormat    // Csv | Json | Xml
);

public record ExportRequest(
    string EntityType,
    string FileFormat
);

public record ExportUnexportedResult(
    byte[]      Data,
    string      FileName,
    string      ContentType,
    int         EntityCount,
    List<(Guid Id, string Ref)> ExportedEntities
);

// ── Legacy row result (kept for backward compatibility with API response) ──────

public class RowResult
{
    public int    Row     { get; init; }
    public bool   Success { get; init; }
    public string? Error  { get; init; }
}

public class ImportResult
{
    public int           TotalRows   { get; set; }
    public int           SuccessRows { get; set; }
    public List<RowResult> RowResults { get; set; } = new();
    public int FailedRows   => RowResults.Count(r => !r.Success);
    public string? ErrorSummary => FailedRows == 0 ? null
        : string.Join("; ", RowResults.Where(r => !r.Success).Take(10).Select(r => $"Row {r.Row}: {r.Error}"));
}

// ── Vendor import/export fields ────────────────────────────────────────────────
// Used for CSV/JSON/XML templates and export column headers.
// The staging parser converts any file format to Dictionary<string, string>
// using these field names as keys.

public static class VendorFields
{
    public const string VendorNumber      = "VendorNumber";
    public const string Name              = "Name";
    public const string ContactName       = "ContactName";
    public const string Email             = "Email";
    public const string Phone             = "Phone";
    public const string Address           = "Address";
    public const string City              = "City";
    public const string State             = "State";
    public const string PostalCode        = "PostalCode";
    public const string Country           = "Country";
    public const string Website           = "Website";
    public const string Currency          = "Currency";
    public const string PaymentTermsDays  = "PaymentTermsDays";
    public const string TaxId             = "TaxId";
    public const string BankAccountName   = "BankAccountName";
    public const string BankAccountNumber = "BankAccountNumber";
    public const string BankRoutingNumber = "BankRoutingNumber";
    public const string CreditLimit       = "CreditLimit";
    public const string Notes             = "Notes";

    public static readonly string[] All =
    [
        VendorNumber, Name, ContactName, Email, Phone,
        Address, City, State, PostalCode, Country, Website,
        Currency, PaymentTermsDays, TaxId,
        BankAccountName, BankAccountNumber, BankRoutingNumber,
        CreditLimit, Notes
    ];
}

public static class ProductFields
{
    public const string Sku           = "Sku";
    public const string Name          = "Name";
    public const string Description   = "Description";
    public const string LongDescription = "LongDescription";
    public const string CategoryCode  = "CategoryCode";
    public const string BrandCode     = "BrandCode";
    public const string ProductType   = "ProductType";   // Clothing | Footwear | Accessory | Food | PersonalCare | Other
    public const string GenderTarget  = "GenderTarget";  // Men | Women | Unisex | Kids | None
    public const string BasePrice     = "BasePrice";
    public const string BaseCost      = "BaseCost";
    public const string TaxRate       = "TaxRate";
    public const string UnitOfMeasure = "UnitOfMeasure";
    public const string Currency      = "Currency";
    public const string Tags          = "Tags";
    public const string ImageUrl      = "ImageUrl";
    public const string Status        = "Status";        // Active | Inactive | Discontinued
    // Variant fields (optional — if present, a variant is created for the product)
    public const string VariantSize      = "VariantSize";
    public const string VariantColor     = "VariantColor";
    public const string VariantMaterial  = "VariantMaterial";
    public const string VariantBarcode   = "VariantBarcode";
    public const string VariantPriceOverride = "VariantPriceOverride";
    public const string VariantCostOverride  = "VariantCostOverride";

    public static readonly string[] All =
    [
        Sku, Name, Description, LongDescription, CategoryCode, BrandCode,
        ProductType, GenderTarget, BasePrice, BaseCost, TaxRate,
        UnitOfMeasure, Currency, Tags, ImageUrl, Status,
        VariantSize, VariantColor, VariantMaterial, VariantBarcode,
        VariantPriceOverride, VariantCostOverride
    ];
}

public static class SalesOrderFields
{
    // Order-level
    public const string CustomerNumber    = "CustomerNumber";
    public const string OrderDate         = "OrderDate";
    public const string RequestedShipDate = "RequestedShipDate";
    public const string Description       = "Description";
    public const string CustomerRef       = "CustomerRef";
    public const string Currency          = "Currency";
    public const string SalesRep          = "SalesRep";
    public const string ShippingAddress   = "ShippingAddress";
    public const string Notes             = "Notes";
    public const string Priority          = "Priority";   // Low | Normal | High | Urgent
    // Line-level (one row per line, repeated order fields group the order)
    public const string VariantSku        = "VariantSku";
    public const string Quantity          = "Quantity";
    public const string UnitPrice         = "UnitPrice";
    public const string DiscountPct       = "DiscountPct";
    public const string TaxRate           = "TaxRate";
    public const string LineNotes         = "LineNotes";

    public static readonly string[] All =
    [
        CustomerNumber, OrderDate, RequestedShipDate, Description,
        CustomerRef, Currency, SalesRep, ShippingAddress, Notes, Priority,
        VariantSku, Quantity, UnitPrice, DiscountPct, TaxRate, LineNotes
    ];
}

public static class PurchaseOrderFields
{
    // Order-level
    public const string VendorNumber     = "VendorNumber";
    public const string OrderDate        = "OrderDate";
    public const string ExpectedDate     = "ExpectedDate";
    public const string Description      = "Description";
    public const string Currency         = "Currency";
    public const string ShippingAddress  = "ShippingAddress";
    public const string ApprovedBy       = "ApprovedBy";
    public const string Notes            = "Notes";
    // Line-level
    public const string VariantSku       = "VariantSku";
    public const string OrderedQty       = "OrderedQty";
    public const string UnitCost         = "UnitCost";
    public const string TaxRate          = "TaxRate";
    public const string LineNotes        = "LineNotes";

    public static readonly string[] All =
    [
        VendorNumber, OrderDate, ExpectedDate, Description, Currency,
        ShippingAddress, ApprovedBy, Notes,
        VariantSku, OrderedQty, UnitCost, TaxRate, LineNotes
    ];
}
