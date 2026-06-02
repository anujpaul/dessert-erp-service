using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Retail;

public enum POSTransactionStatus { Pending, Processed, Failed, Voided }
public enum POSTransactionType   { Sale, Return, Exchange }
public enum POSPaymentMethod     { Cash, CreditCard, DebitCard, GiftCard, StoreCredit, Mixed }
public enum SalesChannel         { InStore, Online, Marketplace, Wholesale }
public enum FulfillmentStatus    { Pending, Ready, Dispatched, Delivered, Cancelled }

public class POSTransaction : BaseEntity
{
    public Guid   OrganizationId  { get; private set; }
    public Guid   StoreId         { get; private set; }
    public string TransactionNumber { get; private set; } = string.Empty;
    public string? ExternalRef    { get; private set; }   // POS system's own reference
    public string? CashierId      { get; private set; }
    public string? CashierName    { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public POSTransactionType   TransactionType   { get; private set; } = POSTransactionType.Sale;
    public POSTransactionStatus Status            { get; private set; } = POSTransactionStatus.Pending;
    public SalesChannel         Channel           { get; private set; } = SalesChannel.InStore;
    public FulfillmentStatus    FulfillmentStatus { get; private set; } = FulfillmentStatus.Pending;

    // Channel-specific metadata
    public string? CustomerName    { get; private set; }   // for online/delivery orders
    public string? CustomerEmail   { get; private set; }
    public string? CustomerPhone   { get; private set; }
    public string? DeliveryAddress { get; private set; }
    public string? ExternalOrderRef { get; private set; }  // e.g. Uber Eats order ID
    public string? ChannelNotes    { get; private set; }

    // Totals
    public decimal SubTotal      { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal TaxTotal      { get; private set; }
    public decimal GrandTotal    { get; private set; }
    public decimal TenderedAmount { get; private set; }
    public decimal ChangeAmount  { get; private set; }
    public string  Currency      { get; private set; } = "USD";

    // Coupon applied
    public string? CouponCode    { get; private set; }
    public decimal CouponDiscount { get; private set; }

    // Processing results
    public Guid?   ARInvoiceId   { get; private set; }
    public Guid?   JournalEntryId { get; private set; }
    public string? ProcessingError { get; private set; }
    public string? SourceFile    { get; private set; }   // if imported via batch

    public IReadOnlyList<POSTransactionLine> Lines    => _lines.AsReadOnly();
    public IReadOnlyList<POSPayment>         Payments => _payments.AsReadOnly();

    private readonly List<POSTransactionLine> _lines    = new();
    private readonly List<POSPayment>         _payments = new();

    private POSTransaction() { }

    public POSTransaction(Guid organizationId, Guid storeId, string transactionNumber,
        DateTime transactionDate, POSTransactionType type = POSTransactionType.Sale,
        string? externalRef = null, string? cashierId = null, string? cashierName = null,
        string currency = "USD", string? couponCode = null, string? sourceFile = null,
        SalesChannel channel = SalesChannel.InStore,
        string? customerName = null, string? customerEmail = null, string? customerPhone = null,
        string? deliveryAddress = null, string? externalOrderRef = null, string? channelNotes = null)
    {
        OrganizationId    = organizationId;
        StoreId           = storeId;
        TransactionNumber = transactionNumber.Trim();
        TransactionDate   = transactionDate;
        TransactionType   = type;
        ExternalRef       = externalRef?.Trim();
        CashierId         = cashierId?.Trim();
        CashierName       = cashierName?.Trim();
        Currency          = currency;
        CouponCode        = couponCode?.Trim().ToUpperInvariant();
        SourceFile        = sourceFile;
        Channel           = channel;
        CustomerName      = customerName?.Trim();
        CustomerEmail     = customerEmail?.Trim();
        CustomerPhone     = customerPhone?.Trim();
        DeliveryAddress   = deliveryAddress?.Trim();
        ExternalOrderRef  = externalOrderRef?.Trim();
        ChannelNotes      = channelNotes?.Trim();
        FulfillmentStatus = channel == SalesChannel.InStore
            ? FulfillmentStatus.Delivered   // in-store = immediate fulfilment
            : FulfillmentStatus.Pending;
    }

    public void UpdateFulfillmentStatus(FulfillmentStatus status)
    {
        FulfillmentStatus = status;
        SetUpdated();
    }

    public void AddLine(POSTransactionLine line) => _lines.Add(line);

    public void AddPayment(POSPayment payment) => _payments.Add(payment);

    public void RecalculateTotals()
    {
        SubTotal      = _lines.Sum(l => l.LineSubTotal);
        DiscountTotal = _lines.Sum(l => l.DiscountAmount) + CouponDiscount;
        TaxTotal      = _lines.Sum(l => l.TaxAmount);
        GrandTotal    = SubTotal - DiscountTotal + TaxTotal;
        TenderedAmount = _payments.Sum(p => p.Amount);
        ChangeAmount  = Math.Max(0, TenderedAmount - GrandTotal);
    }

    public void ApplyCouponDiscount(decimal discountAmount)
    {
        CouponDiscount = discountAmount;
        RecalculateTotals();
    }

    public void MarkProcessed(Guid arInvoiceId, Guid? journalEntryId = null)
    {
        Status         = POSTransactionStatus.Processed;
        ARInvoiceId    = arInvoiceId;
        JournalEntryId = journalEntryId;
        ProcessingError = null;
        SetUpdated();
    }

    public void MarkFailed(string error)
    {
        Status          = POSTransactionStatus.Failed;
        ProcessingError = error;
        SetUpdated();
    }

    public void Void()
    {
        Status = POSTransactionStatus.Voided;
        SetUpdated();
    }
}

public class POSTransactionLine : BaseEntity
{
    public Guid    POSTransactionId  { get; private set; }
    public Guid?   ProductVariantId  { get; private set; }
    public string  Sku               { get; private set; } = string.Empty;
    public string  ProductName       { get; private set; } = string.Empty;
    public string  UnitOfMeasure     { get; private set; } = "EA";
    public decimal Quantity          { get; private set; }
    public decimal UnitPrice         { get; private set; }
    public decimal DiscountPct       { get; private set; }
    public decimal DiscountAmount    { get; private set; }
    public decimal TaxRate           { get; private set; }
    public decimal LineSubTotal      { get; private set; }
    public decimal TaxAmount         { get; private set; }
    public decimal LineTotal         { get; private set; }
    public bool    IsReturn          { get; private set; }

    private POSTransactionLine() { }

    public POSTransactionLine(Guid posTransactionId, string sku, string productName,
        decimal quantity, decimal unitPrice, decimal discountPct = 0, decimal taxRate = 0,
        Guid? productVariantId = null, string unitOfMeasure = "EA", bool isReturn = false)
    {
        POSTransactionId = posTransactionId;
        ProductVariantId = productVariantId;
        Sku              = sku.Trim();
        ProductName      = productName.Trim();
        UnitOfMeasure    = unitOfMeasure;
        Quantity         = quantity;
        UnitPrice        = unitPrice;
        DiscountPct      = discountPct;
        TaxRate          = taxRate;
        IsReturn         = isReturn;
        Calculate();
    }

    private void Calculate()
    {
        LineSubTotal  = Quantity * UnitPrice;
        DiscountAmount = LineSubTotal * (DiscountPct / 100m);
        var afterDiscount = LineSubTotal - DiscountAmount;
        TaxAmount     = afterDiscount * (TaxRate / 100m);
        LineTotal     = afterDiscount + TaxAmount;
    }
}

public class POSPayment : BaseEntity
{
    public Guid             POSTransactionId { get; private set; }
    public POSPaymentMethod PaymentMethod    { get; private set; }
    public decimal          Amount           { get; private set; }
    public string?          Reference        { get; private set; } // card last 4, gift card #, etc.

    private POSPayment() { }

    public POSPayment(Guid posTransactionId, POSPaymentMethod method, decimal amount, string? reference = null)
    {
        POSTransactionId = posTransactionId;
        PaymentMethod    = method;
        Amount           = amount;
        Reference        = reference?.Trim();
    }
}
