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
    decimal Quantity, decimal QuantityShipped, decimal UnitPrice, decimal DiscountPct, decimal TaxRate,
    decimal LineSubTotal, decimal DiscountAmount, decimal TaxAmount, decimal LineTotal);

public record SalesOrderSummaryDto(Guid Id, string OrderNumber, Guid CustomerId,
    string CustomerName, DateTime OrderDate, DateTime? RequestedShipDate,
    string CustomerRef, string Status, decimal GrandTotal, int LineCount, DateTime CreatedAt,
    bool IsExported, DateTime? ExportedAt, Guid? WorkflowInstanceId, string? RejectionReason);

public record SalesOrderDto(Guid Id, string OrderNumber, Guid CustomerId, string CustomerName,
    DateTime OrderDate, DateTime? RequestedShipDate, DateTime? ActualShipDate,
    string Description, string CustomerRef, string Currency, string Status,
    decimal SubTotal, decimal TaxTotal, decimal DiscountTotal, decimal GrandTotal,
    Guid? ARInvoiceId, DateTime CreatedAt, IReadOnlyList<SalesOrderLineDto> Lines,
    bool IsExported, DateTime? ExportedAt,
    Guid? WorkflowInstanceId, string? RejectionReason,
    DateTime? DeliveredAt, string? DeliveryReference);

public record CreateSalesOrderRequest(
    Guid CustomerId, DateTime OrderDate, string Description,
    string CustomerRef = "", string Currency = "USD",
    DateTime? RequestedShipDate = null);

public record AddSalesOrderLineRequest(
    Guid ProductVariantId, decimal Quantity,
    decimal? OverrideUnitPrice = null, decimal DiscountPct = 0);

public record UpdateSalesOrderLineRequest(
    decimal Quantity, decimal? UnitPrice = null, decimal? DiscountPct = null);

public record ConfirmSalesOrderRequest(decimal BackorderLimit = 0m);

public record ShipOrderLineRequest(Guid LineId, decimal Quantity);

public record ShipOrderRequest(
    DateTime? ShipDate = null,
    IReadOnlyList<ShipOrderLineRequest>? Lines = null,
    string? TrackingNumber = null);

public record ApplyDiscountRequest(decimal DiscountPct);

// ── AR Invoice ────────────────────────────────────────────────────────────────
public record ARInvoiceDto(Guid Id, string InvoiceNumber, Guid CustomerId,
    string CustomerName, Guid? SalesOrderId, string? SalesOrderNumber,
    DateTime InvoiceDate, DateTime DueDate, string Description,
    decimal SubTotal, decimal TaxAmount, decimal DiscountAmount, decimal TotalAmount,
    decimal PaidAmount, decimal OutstandingAmount, string Status,
    int DaysOutstanding, DateTime CreatedAt,
    Guid? WorkflowInstanceId = null, bool IsSubmittedForApproval = false);

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

// ── Sales Quotations ──────────────────────────────────────────────────────────
public record QuotationLineDto(
    Guid Id, int LineNumber, Guid ProductVariantId, string Sku,
    string ProductName, string? VariantDescription, string UnitOfMeasure,
    decimal Quantity, decimal UnitPrice, decimal DiscountPct, decimal TaxRate,
    decimal LineSubTotal, decimal DiscountAmount, decimal TaxAmount, decimal LineTotal);

public record QuotationSummaryDto(
    Guid Id, string QuotationNumber, Guid CustomerId, string CustomerName,
    DateTime QuotationDate, DateTime ValidUntil, string CustomerRef,
    string Status, decimal GrandTotal, int LineCount,
    Guid? WorkflowInstanceId, Guid? ConvertedToSOId, DateTime CreatedAt);

public record QuotationDto(
    Guid Id, string QuotationNumber, Guid CustomerId, string CustomerName,
    DateTime QuotationDate, DateTime ValidUntil, string Description,
    string CustomerRef, string Currency, string Status,
    decimal SubTotal, decimal TaxTotal, decimal DiscountTotal, decimal GrandTotal,
    Guid? WorkflowInstanceId, string? RejectionReason,
    Guid? ConvertedToSOId, DateTime? ConvertedAt, string? Notes, DateTime CreatedAt,
    IReadOnlyList<QuotationLineDto> Lines);

public record CreateQuotationRequest(
    Guid CustomerId, DateTime QuotationDate, DateTime ValidUntil,
    string Description, string CustomerRef = "", string Currency = "USD",
    string? Notes = null);

public record AddQuotationLineRequest(
    Guid ProductVariantId, decimal Quantity,
    decimal? OverrideUnitPrice = null, decimal DiscountPct = 0);

public record ConvertQuotationToSORequest(
    DateTime? OrderDate = null, string? Description = null);

// ── SO Workflow ───────────────────────────────────────────────────────────────
public record SubmitSOForApprovalRequest(string SubmittedBy);
public record SOWorkflowOutcomeRequest(string? Reason = null);

// ── AR Invoice Workflow ───────────────────────────────────────────────────────
public record SubmitARInvoiceForApprovalRequest(string SubmittedBy);

// ── Delivery Confirmation ─────────────────────────────────────────────────────
public record ConfirmDeliveryRequest(DateTime? DeliveredAt = null, string? Reference = null);

// ── Customer Credit Notes ─────────────────────────────────────────────────────
public record ARCreditNoteSummaryDto(
    Guid Id, string CreditNoteNumber, Guid CustomerId, string CustomerName,
    Guid? ARInvoiceId, DateTime CreditDate, string Reason,
    decimal TotalAmount, decimal AvailableCredit, string Status, DateTime CreatedAt);

public record ARCreditNoteDto(
    Guid Id, string CreditNoteNumber, Guid CustomerId, string CustomerName,
    Guid? ARInvoiceId, string? InvoiceNumber, Guid? SalesOrderId, string? SalesOrderNumber,
    DateTime CreditDate, string Description, string? CustomerRef,
    decimal SubTotal, decimal TaxAmount, decimal TotalAmount,
    decimal AppliedAmount, decimal AvailableCredit,
    string Status, string Reason, string? Notes,
    Guid? WorkflowInstanceId, DateTime CreatedAt);

public record CreateARCreditNoteRequest(
    Guid CustomerId, DateTime CreditDate, string Description,
    decimal SubTotal, decimal TaxAmount = 0,
    string Reason = "Other",
    Guid? ARInvoiceId = null, Guid? SalesOrderId = null,
    string? CustomerRef = null, string? Notes = null);

public record ApplyCreditNoteRequest(Guid ARInvoiceId, decimal Amount);
public record SubmitCreditNoteForApprovalRequest(string SubmittedBy);

// ── Dunning Records ───────────────────────────────────────────────────────────
public record DunningRecordDto(
    Guid Id, string DunningNumber, Guid CustomerId, string CustomerName,
    Guid ARInvoiceId, string InvoiceNumber,
    string Level, string Status,
    DateTime SentDate, DateTime FollowUpDate, decimal OutstandingAmount,
    string? AssignedTo, string? Notes,
    DateTime? ResolvedAt, string? ResolutionNotes, DateTime CreatedAt);

public record CreateDunningRequest(
    Guid CustomerId, Guid ARInvoiceId, string Level,
    DateTime? FollowUpDate = null, string? AssignedTo = null, string? Notes = null);
