namespace DessertERP.Application.Modules.AccountsPayable.DTOs;

// ── Vendor ────────────────────────────────────────────────────────────────────
public record VendorDto(Guid Id, string VendorNumber, string Name,
    string? Email, string? Phone, string? Address,
    string Currency, int PaymentTermsDays, string? TaxId,
    string? BankAccountName, string? BankAccountNumber, string Status, DateTime CreatedAt);

public record CreateVendorRequest(
    string Name, string? Email = null, string? Phone = null,
    string? Address = null, string Currency = "USD",
    int PaymentTermsDays = 30, string? TaxId = null);

public record UpdateVendorRequest(
    string Name, string? Email, string? Phone, string? Address,
    int PaymentTermsDays, string? BankAccountName, string? BankAccountNumber);

// ── Purchase Order ────────────────────────────────────────────────────────────
public record PurchaseOrderLineDto(
    Guid Id, Guid ProductVariantId, string ProductCode, string Description,
    string UnitOfMeasure, decimal OrderedQty, decimal ReceivedQty,
    decimal UnitCost, decimal TaxRate, decimal LineTotal,
    bool IsFullyReceived, decimal OutstandingQty);

public record PurchaseOrderSummaryDto(Guid Id, string PONumber, Guid VendorId,
    string VendorName, DateTime OrderDate, DateTime? ExpectedDate,
    string Status, string InvoiceStatus, decimal GrandTotal, decimal InvoicedAmount,
    int LineCount, DateTime CreatedAt);

public record PurchaseOrderDto(Guid Id, string PONumber, Guid VendorId, string VendorName,
    DateTime OrderDate, DateTime? ExpectedDate, string Description, string Currency,
    string Status, string InvoiceStatus, decimal SubTotal, decimal TaxTotal, decimal GrandTotal,
    decimal InvoicedAmount, bool CanReceive, DateTime CreatedAt,
    IReadOnlyList<PurchaseOrderLineDto> Lines);

public record CreatePurchaseOrderRequest(
    Guid VendorId, DateTime OrderDate, string Description,
    string Currency = "USD", DateTime? ExpectedDate = null);

// AddPOLineRequest now requires a ProductVariantId — links the line to the product catalog
public record AddPOLineRequest(
    Guid ProductVariantId,
    string ProductCode,   // denormalized SKU (filled by frontend from variant lookup)
    string Description,
    string UnitOfMeasure,
    decimal Quantity,
    decimal UnitCost,
    decimal TaxRate = 0);

/// <summary>One line in a receive-goods request — identifies the PO line and qty being received.</summary>
public record ReceiveLineRequest(Guid LineId, decimal Qty);

/// <summary>Request to record a goods receipt against a PO (may be partial).</summary>
public record RecordReceiptRequest(
    IReadOnlyList<ReceiveLineRequest> Lines,
    DateTime? ReceivedDate = null,
    string? Notes = null);

// ── Receipts ──────────────────────────────────────────────────────────────────
public record ReceiptLineDto(Guid Id, Guid PurchaseOrderLineId, string ProductCode, string Description, decimal Qty);

public record ReceiptDto(Guid Id, string ReceiptNumber, DateTime ReceivedDate,
    string? Notes, DateTime CreatedAt, IReadOnlyList<ReceiptLineDto> Lines);

// ── AP Invoice ────────────────────────────────────────────────────────────────
public record APInvoiceDto(Guid Id, string InvoiceNumber, Guid VendorId,
    string VendorName, Guid? PurchaseOrderId, string? PONumber,
    DateTime InvoiceDate, DateTime DueDate, string Description,
    string VendorInvoiceRef, decimal SubTotal, decimal TaxAmount,
    decimal TotalAmount, decimal PaidAmount, decimal OutstandingAmount,
    string Status, int DaysOutstanding, DateTime CreatedAt);

public record CreateAPInvoiceRequest(
    Guid VendorId, DateTime InvoiceDate, DateTime DueDate,
    string Description, string VendorInvoiceRef,
    decimal SubTotal, decimal TaxAmount, Guid? PurchaseOrderId = null);

// ── AP Payment ────────────────────────────────────────────────────────────────
public record APPaymentDto(Guid Id, string PaymentNumber, Guid VendorId,
    string VendorName, Guid APInvoiceId, string InvoiceNumber,
    DateTime PaymentDate, decimal Amount, string PaymentMethod,
    string? Reference, string Status, DateTime CreatedAt);

public record CreateAPPaymentRequest(
    Guid VendorId, Guid APInvoiceId, DateTime PaymentDate,
    decimal Amount, string PaymentMethod = "BankTransfer", string? Reference = null);

// ── Reports ───────────────────────────────────────────────────────────────────
public record APAgingDto(string VendorNumber, string VendorName,
    decimal Current, decimal Days1_30, decimal Days31_60, decimal Days61_90,
    decimal Over90, decimal Total);
