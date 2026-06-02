namespace DessertERP.Application.Modules.Retail.DTOs;

// ── Store ──────────────────────────────────────────────────────────────────────
public record RetailStoreDto(Guid Id, string StoreCode, string Name,
    string? Address, string? Phone, string? ManagerName, bool IsActive, DateTime CreatedAt);

public record CreateStoreRequest(string StoreCode, string Name,
    string? Address = null, string? Phone = null, string? ManagerName = null);

public record UpdateStoreRequest(string Name,
    string? Address = null, string? Phone = null, string? ManagerName = null);

// ── POS Transaction ───────────────────────────────────────────────────────────
public record POSTransactionLineDto(Guid Id, Guid? ProductVariantId, string Sku,
    string ProductName, string UnitOfMeasure, decimal Quantity, decimal UnitPrice,
    decimal DiscountPct, decimal DiscountAmount, decimal TaxRate,
    decimal LineSubTotal, decimal TaxAmount, decimal LineTotal, bool IsReturn);

public record POSPaymentDto(Guid Id, string PaymentMethod, decimal Amount, string? Reference);

public record POSTransactionSummaryDto(Guid Id, string TransactionNumber, string StoreName,
    string? ExternalRef, DateTime TransactionDate, string TransactionType,
    string Status, string Channel, string FulfillmentStatus,
    decimal GrandTotal, int LineCount, string? CouponCode,
    string? CustomerName, string? ExternalOrderRef,
    Guid? ARInvoiceId, DateTime CreatedAt);

public record POSTransactionDto(Guid Id, string TransactionNumber, Guid StoreId,
    string StoreName, string? ExternalRef, string? CashierId, string? CashierName,
    DateTime TransactionDate, string TransactionType, string Status,
    string Channel, string FulfillmentStatus,
    string? CustomerName, string? CustomerEmail, string? CustomerPhone,
    string? DeliveryAddress, string? ExternalOrderRef, string? ChannelNotes,
    string Currency,
    decimal SubTotal, decimal DiscountTotal, decimal TaxTotal, decimal GrandTotal,
    decimal TenderedAmount, decimal ChangeAmount,
    string? CouponCode, decimal CouponDiscount,
    Guid? ARInvoiceId, Guid? JournalEntryId, string? ProcessingError, string? SourceFile,
    DateTime CreatedAt,
    IReadOnlyList<POSTransactionLineDto> Lines,
    IReadOnlyList<POSPaymentDto> Payments);

public record POSTransactionLineRequest(
    string Sku, string ProductName, decimal Quantity, decimal UnitPrice,
    decimal DiscountPct = 0, decimal TaxRate = 0,
    Guid? ProductVariantId = null, string UnitOfMeasure = "EA", bool IsReturn = false);

public record POSPaymentRequest(string PaymentMethod, decimal Amount, string? Reference = null);

public record CreatePOSTransactionRequest(
    Guid StoreId, string TransactionNumber, DateTime TransactionDate,
    List<POSTransactionLineRequest> Lines,
    List<POSPaymentRequest> Payments,
    string TransactionType   = "Sale",
    string Channel           = "InStore",
    string? ExternalRef      = null,
    string? CashierId        = null,
    string? CashierName      = null,
    string  Currency         = "USD",
    string? CouponCode       = null,
    string? SourceFile       = null,
    string? CustomerName     = null,
    string? CustomerEmail    = null,
    string? CustomerPhone    = null,
    string? DeliveryAddress  = null,
    string? ExternalOrderRef = null,
    string? ChannelNotes     = null);

// Online / delivery order intake — simplified public-facing request
public record OnlineOrderRequest(
    Guid   StoreId,
    string CustomerName,
    string CustomerEmail,
    List<POSTransactionLineRequest> Lines,
    string? CustomerPhone    = null,
    string? DeliveryAddress  = null,
    string? CouponCode       = null,
    string? ChannelNotes     = null,
    string  Channel          = "Online",
    string  Currency         = "USD");

public record UpdateFulfillmentStatusRequest(string FulfillmentStatus);

// ── Coupon validation ─────────────────────────────────────────────────────────
public record ValidateCouponRequest(string Code, decimal OrderAmount);

public record CouponValidationResult(
    bool    IsValid,
    string? Message,
    string? PromotionName,
    string? DiscountType,
    decimal DiscountValue,
    decimal DiscountAmount,   // calculated discount for this order
    DateTime? ExpiresAt,
    int     RemainingUses);

// ── Promotion ─────────────────────────────────────────────────────────────────
public record PromotionDto(Guid Id, string Name, string? Description,
    string DiscountType, string Status, decimal DiscountValue,
    int? BuyQuantity, int? GetQuantity,
    decimal MinimumOrderAmount, int MaxUsesTotal, int MaxUsesPerCustomer, int UsedCount,
    DateTime StartDate, DateTime? EndDate,
    bool ApplyToAllProducts, string? ApplicableSkus, DateTime CreatedAt);

public record CreatePromotionRequest(
    string Name, string DiscountType, decimal DiscountValue,
    DateTime StartDate,
    string?  Description          = null,
    DateTime? EndDate             = null,
    decimal  MinimumOrderAmount   = 0,
    int      MaxUsesTotal         = 0,
    int      MaxUsesPerCustomer   = 0,
    int?     BuyQuantity          = null,
    int?     GetQuantity          = null,
    bool     ApplyToAllProducts   = true,
    string?  ApplicableSkus       = null);

// ── Coupon ────────────────────────────────────────────────────────────────────
public record CouponDto(Guid Id, Guid PromotionId, string PromotionName,
    string Code, bool IsActive, int MaxUses, int UsedCount,
    int RemainingUses, DateTime? ExpiresAt, DateTime CreatedAt);

public record CreateCouponRequest(
    Guid PromotionId, string Code,
    int MaxUses = 1, DateTime? ExpiresAt = null);

public record BulkCreateCouponsRequest(
    Guid PromotionId, int Count, string Prefix = "",
    int MaxUsesEach = 1, DateTime? ExpiresAt = null);

// ── Reports ───────────────────────────────────────────────────────────────────
public record RetailSummaryDto(
    int TotalTransactions, int ProcessedTransactions, int FailedTransactions,
    decimal TotalRevenue, decimal TotalDiscounts, decimal TotalTax,
    int TotalItemsSold, string TopStore);
