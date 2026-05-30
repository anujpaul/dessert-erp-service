namespace DessertERP.Application.Modules.AccountsReceivable.DTOs;

// ── Customer ──────────────────────────────────────────────────────────────────
public record CustomerDto(Guid Id, string CustomerNumber, string Name,
    string? Email, string? Phone, string? Address,
    string Currency, int PaymentTermsDays, decimal CreditLimit,
    string Status, DateTime CreatedAt);

public record CreateCustomerRequest(
    string Name, string? Email = null, string? Phone = null,
    string? Address = null, string Currency = "USD",
    int PaymentTermsDays = 30, decimal CreditLimit = 10000m);

// ── Sales Order ───────────────────────────────────────────────────────────────
public record SalesOrderLineDto(Guid Id, Guid ProductVariantId, string Sku,
    string ProductName, string? VariantDescription, string UnitOfMeasure,
    decimal Quantity, decimal UnitPrice, decimal DiscountPct, decimal TaxRate,
    decimal LineSubTotal, decimal DiscountAmount, decimal TaxAmount, decimal LineTotal);

public record SalesOrderSummaryDto(Guid Id, string OrderNumber, Guid CustomerId,
    string CustomerName, DateTime OrderDate, DateTime? RequestedShipDate,
    string CustomerRef, string Status, decimal GrandTotal, int LineCount, DateTime CreatedAt);

public record SalesOrderDto(Guid Id, string OrderNumber, Guid CustomerId, string CustomerName,
    DateTime OrderDate, DateTime? RequestedShipDate, DateTime? ActualShipDate,
    string Description, string CustomerRef, string Currency, string Status,
    decimal SubTotal, decimal TaxTotal, decimal DiscountTotal, decimal GrandTotal,
    Guid? ARInvoiceId, DateTime CreatedAt, IReadOnlyList<SalesOrderLineDto> Lines);

public record CreateSalesOrderRequest(
    Guid CustomerId, DateTime OrderDate, string Description,
    string CustomerRef = "", string Currency = "USD",
    DateTime? RequestedShipDate = null);

public record AddSalesOrderLineRequest(
    Guid ProductVariantId, decimal Quantity,
    decimal? OverrideUnitPrice = null, decimal DiscountPct = 0);

public record ShipOrderRequest(DateTime ShipDate);

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
