namespace DessertERP.Application.Modules.AccountsPayable.DTOs;

// ── Vendor Address / Contact ──────────────────────────────────────────────────
public record VendorAddressDto(
    Guid Id, string Label, string AddressType, bool IsPrimary,
    string Line1, string? Line2, string City, string? State,
    string? PostalCode, string Country, string SingleLine);

public record VendorContactDto(
    Guid Id, string Name, string? Title,
    string? Email, string? Phone, string? Mobile,
    bool IsPrimary, string? Notes);

public record SaveVendorAddressRequest(
    string Label, string AddressType, string Line1, string? Line2,
    string City, string? State, string? PostalCode, string Country = "US");

public record SaveVendorContactRequest(
    string Name, string? Title, string? Email,
    string? Phone, string? Mobile, string? Notes);

// ── Vendor ────────────────────────────────────────────────────────────────────
public record VendorDto(
    Guid Id, string VendorNumber, string Name,
    string? Email, string? Phone,
    string? BillingAddress, string? ShippingAddress,
    string? Website, string? Notes,
    string Currency, int PaymentTermsDays, string? TaxId,
    string? BankAccountName, string? BankAccountNumber, string? BankRoutingNumber,
    // Balance snapshot
    decimal OutstandingPayable,
    string Status, DateTime CreatedAt,
    IReadOnlyList<VendorAddressDto>? Addresses = null,
    IReadOnlyList<VendorContactDto>? Contacts = null);

public record CreateVendorRequest(
    string Name, string? Email = null, string? Phone = null,
    string? BillingAddress = null, string? ShippingAddress = null,
    string Currency = "USD", int PaymentTermsDays = 30, string? TaxId = null,
    string? Website = null, string? Notes = null);

public record UpdateVendorRequest(
    string Name, string? Email, string? Phone,
    string? BillingAddress, string? ShippingAddress,
    int PaymentTermsDays, string? BankAccountName, string? BankAccountNumber,
    string? BankRoutingNumber = null, string? Website = null, string? Notes = null);

// ── Vendor Ledger ─────────────────────────────────────────────────────────────
public record VendorLedgerEntryDto(
    string EntryType,          // "Invoice" or "Payment"
    string Reference,          // invoice/payment number
    DateTime Date,
    decimal Debit,             // payment amount (money out)
    decimal Credit,            // invoice total (liability)
    decimal RunningBalance,
    string Status,
    string? PONumber);

public record VendorLedgerDto(
    Guid VendorId, string VendorName, string VendorNumber,
    decimal TotalInvoiced, decimal TotalPaid, decimal OutstandingPayable,
    IReadOnlyList<VendorLedgerEntryDto> Entries);

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
public record APInvoiceDto(
    Guid Id, string InvoiceNumber, Guid VendorId, string VendorName,
    Guid? PurchaseOrderId, string? PONumber,
    DateTime InvoiceDate, DateTime DueDate, string Description, string VendorInvoiceRef,
    decimal SubTotal, decimal TaxAmount, decimal TotalAmount,
    decimal PaidAmount, decimal PrepaymentApplied, decimal OutstandingAmount,
    string Status, string InvoiceType, string MatchStatus,
    string? MatchNotes, string? BypassReason,
    Guid? LinkedPrepaymentInvoiceId, string? LinkedPrepaymentNumber,
    int DaysOutstanding, DateTime CreatedAt);

public record CreateAPInvoiceRequest(
    Guid VendorId, DateTime InvoiceDate, DateTime DueDate,
    string Description, string VendorInvoiceRef,
    decimal SubTotal, decimal TaxAmount, Guid? PurchaseOrderId = null);

public record CreatePrepaymentInvoiceRequest(
    Guid VendorId, Guid PurchaseOrderId,
    string VendorInvoiceRef, DateTime InvoiceDate, DateTime DueDate,
    decimal Amount, decimal TaxAmount = 0, string? Description = null);

public record ThreeWayMatchDto(
    Guid InvoiceId, string MatchStatus,
    decimal ReceivedValue, decimal PreviouslyInvoiced,
    decimal UninvoicedReceived, decimal InvoiceSubTotal,
    decimal VariancePct, decimal TolerancePct,
    bool QtyException, bool PriceException);

public record BypassMatchRequest(string Reason);

public record ApplyPrepaymentRequest(Guid PrepaymentInvoiceId);

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

// ── Purchase Requisitions ─────────────────────────────────────────────────────
public record PRSummaryDto(
    Guid Id, string RequisitionNumber, string RequestedBy,
    string? DepartmentCode, DateTime NeededByDate,
    string Status, decimal TotalEstimatedCost, int LineCount,
    Guid? WorkflowInstanceId, Guid? ConvertedToPOId, DateTime CreatedAt);

public record PRLineDto(
    Guid Id, int LineNumber, Guid? ProductId,
    string Description, decimal Quantity, string UnitOfMeasure,
    decimal EstimatedUnitCost, decimal EstimatedTotalCost,
    Guid? SuggestedVendorId, string? GlAccountCode, string? Notes);

public record PRDto(
    Guid Id, string RequisitionNumber, string RequestedBy,
    string? DepartmentCode, string? CostCenterCode,
    DateTime NeededByDate, string Status, decimal TotalEstimatedCost,
    Guid? WorkflowInstanceId, Guid? ConvertedToPOId,
    string? RejectionReason, string? Notes, DateTime CreatedAt,
    IReadOnlyList<PRLineDto> Lines);

public record CreatePRRequest(
    string RequestedBy, DateTime NeededByDate,
    string? DepartmentCode = null, string? CostCenterCode = null,
    string? Notes = null,
    IReadOnlyList<AddPRLineRequest>? Lines = null);

public record AddPRLineRequest(
    string Description, decimal Quantity, string UnitOfMeasure = "EA",
    decimal EstimatedUnitCost = 0, Guid? ProductId = null,
    Guid? SuggestedVendorId = null, string? GlAccountCode = null,
    string? Notes = null);

/// <summary>Maps PR line Ids → ProductVariantIds when converting a PR to a PO.</summary>
public record ConvertPRToPORequest(
    Guid VendorId, DateTime OrderDate,
    /// <summary>Maps each PRLineId to the ProductVariantId to use on the PO line.</summary>
    Dictionary<Guid, Guid>? ProductVariantIds = null,
    string? Currency = "USD", DateTime? ExpectedDate = null);

// ── Payment Proposals ─────────────────────────────────────────────────────────
public record PaymentProposalSummaryDto(
    Guid Id, string ProposalNumber, DateTime ProposalDate, DateTime PaymentDate,
    string PaymentMethod, string Status, decimal TotalAmount,
    int LineCount, DateTime? ProcessedAt, string? ProcessedBy, DateTime CreatedAt);

public record PaymentProposalLineDto(
    Guid Id, Guid APInvoiceId, string InvoiceNumber,
    Guid VendorId, string VendorName, decimal ProposedAmount,
    DateTime InvoiceDueDate, Guid? APPaymentId);

public record PaymentProposalDto(
    Guid Id, string ProposalNumber, DateTime ProposalDate, DateTime PaymentDate,
    string PaymentMethod, string? BankAccount, string Status, decimal TotalAmount,
    DateTime? ProcessedAt, string? ProcessedBy, string? Notes, DateTime CreatedAt,
    IReadOnlyList<PaymentProposalLineDto> Lines);

public record CreatePaymentProposalRequest(
    DateTime ProposalDate, DateTime PaymentDate,
    string PaymentMethod = "BankTransfer",
    string? BankAccount = null, string? Notes = null);

// ── Vendor Credit Notes ───────────────────────────────────────────────────────
public record CreditNoteDto(
    Guid Id, string CreditNoteNumber, Guid VendorId, string VendorName,
    Guid? APInvoiceId, Guid? PurchaseOrderId,
    DateTime CreditDate, string Description, string? VendorCNRef,
    decimal SubTotal, decimal TaxAmount, decimal TotalAmount,
    decimal AppliedAmount, decimal AvailableCredit,
    string Status, string Reason, string? Notes, DateTime CreatedAt);

public record CreateCreditNoteRequest(
    Guid VendorId, DateTime CreditDate, string Description,
    decimal SubTotal, decimal TaxAmount = 0,
    string Reason = "Other", Guid? APInvoiceId = null,
    Guid? PurchaseOrderId = null, string? VendorCNRef = null, string? Notes = null);

// ── PO / Invoice workflow action DTOs ────────────────────────────────────────
public record SubmitForApprovalRequest(string SubmittedBy);
public record WorkflowOutcomeRequest(string? Reason = null);
