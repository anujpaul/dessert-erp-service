namespace DessertERP.Application.Modules.AccountsReceivable.DTOs;

// ── Customer Address / Contact ────────────────────────────────────────────────
public record CustomerAddressDto(
    Guid Id, string Label, string AddressType, bool IsPrimary,
    string Line1, string? Line2, string City, string? State,
    string? PostalCode, string Country, string SingleLine);

public record CustomerContactDto(
    Guid Id, string Name, string? Title,
    string? Email, string? Phone, string? Mobile,
    bool IsPrimary, string? Notes);

public record SaveCustomerAddressRequest(
    string Label, string AddressType, string Line1, string? Line2,
    string City, string? State, string? PostalCode, string Country = "US");

public record SaveCustomerContactRequest(
    string Name, string? Title, string? Email,
    string? Phone, string? Mobile, string? Notes);

// ── Customer ──────────────────────────────────────────────────────────────────
public record CustomerDto(
    Guid Id, string CustomerNumber, string Name,
    string? Email, string? Phone,
    string? BillingAddress, string? ShippingAddress,
    string? Website, string? Notes,
    string Currency, int PaymentTermsDays, decimal CreditLimit,
    // Balance snapshot (computed on fetch)
    decimal OutstandingBalance, decimal CreditUsed, decimal CreditAvailable,
    string Status, DateTime CreatedAt,
    IReadOnlyList<CustomerAddressDto>? Addresses = null,
    IReadOnlyList<CustomerContactDto>? Contacts = null);

public record CreateCustomerRequest(
    string Name, string? Email = null, string? Phone = null,
    string? BillingAddress = null, string? ShippingAddress = null,
    string Currency = "USD", int PaymentTermsDays = 30, decimal CreditLimit = 10000m,
    string? Website = null, string? Notes = null);

public record UpdateCustomerRequest(
    string Name, string? Email, string? Phone,
    string? BillingAddress, string? ShippingAddress,
    int PaymentTermsDays, decimal CreditLimit,
    string? Website = null, string? Notes = null);

// ── Customer Ledger ───────────────────────────────────────────────────────────
public record CustomerLedgerEntryDto(
    string EntryType,          // "Invoice" or "Payment"
    string Reference,          // invoice/payment number
    DateTime Date,
    decimal Debit,             // invoice total (charge)
    decimal Credit,            // payment amount
    decimal RunningBalance,
    string Status,
    string? SalesOrderNumber);

public record CustomerLedgerDto(
    Guid CustomerId, string CustomerName, string CustomerNumber,
    decimal TotalInvoiced, decimal TotalPaid, decimal OutstandingBalance,
    IReadOnlyList<CustomerLedgerEntryDto> Entries);

// ── Sales Order ───────────────────────────────────────────────────────────────
public record SalesOrderLineDto(Guid Id, Guid ProductVariantId, string Sku,
    string ProductName, string? VariantDescription, string UnitOfMeasure,
    decimal Quantity, decimal UnitPrice, decimal DiscountPct, decimal TaxRate,
    decimal LineSubTotal, decimal DiscountAmount, decimal TaxAmount, decimal LineTotal);

public record SalesOrderSummaryDto(Guid Id, string OrderNumber, Guid CustomerId,
    string CustomerName, DateTime OrderDate, DateTime? RequestedShipDate,
    string CustomerRef, string Status, decimal GrandTotal, int LineCount, DateTime CreatedAt,
    bool IsExported, DateTime? ExportedAt);

public record SalesOrderDto(Guid Id, string OrderNumber, Guid CustomerId, string CustomerName,
    DateTime OrderDate, DateTime? RequestedShipDate, DateTime? ActualShipDate,
    string Description, string CustomerRef, string Currency, string Status,
    decimal SubTotal, decimal TaxTotal, decimal DiscountTotal, decimal GrandTotal,
    Guid? ARInvoiceId, DateTime CreatedAt, IReadOnlyList<SalesOrderLineDto> Lines,
    bool IsExported, DateTime? ExportedAt);

public record CreateSalesOrderRequest(
    Guid CustomerId, DateTime OrderDate, string Description,
    string CustomerRef = "", string Currency = "USD",
    DateTime? RequestedShipDate = null);

public record AddSalesOrderLineRequest(
    Guid ProductVariantId, decimal Quantity,
    decimal? OverrideUnitPrice = null, decimal DiscountPct = 0);

public record ShipOrderRequest(DateTime ShipDate);

public record ApplyDiscountRequest(decimal DiscountPct);

// ── AR Invoice ────────────────────────────────────────────────────────────────
public record ARInvoiceDto(Guid Id, string InvoiceNumber, Guid CustomerId,
    string CustomerName, Guid? SalesOrderId, string? SalesOrderNumber,
    DateTime InvoiceDate, DateTime DueDate, string Description,
    decimal SubTotal, decimal TaxAmount, decimal DiscountAmount, decimal TotalAmount,
    decimal PaidAmount, decimal OutstandingAmount, string Status,
    int DaysOutstanding, DateTime CreatedAt);

public record CreateARInvoiceRequest(
    Guid CustomerId, DateTime InvoiceDate, DateTime DueDate,
    string Description, decimal SubTotal, decimal TaxAmount,
    decimal DiscountAmount = 0, Guid? SalesOrderId = null);

// ── AR Payment ────────────────────────────────────────────────────────────────
public record ARPaymentDto(Guid Id, string PaymentNumber, Guid CustomerId,
    string CustomerName, Guid ARInvoiceId, string InvoiceNumber,
    DateTime PaymentDate, decimal Amount, string PaymentMethod,
    string? Reference, string Status, DateTime CreatedAt);

public record CreateARPaymentRequest(
    Guid CustomerId, Guid ARInvoiceId, DateTime PaymentDate,
    decimal Amount, string PaymentMethod = "BankTransfer", string? Reference = null);

// ── Reports ───────────────────────────────────────────────────────────────────
public record ARAgingDto(string CustomerNumber, string CustomerName,
    decimal Current, decimal Days1_30, decimal Days31_60, decimal Days61_90,
    decimal Over90, decimal Total);
