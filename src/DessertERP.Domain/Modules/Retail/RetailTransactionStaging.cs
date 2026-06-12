using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.Retail;

public enum RetailStagingStatus
{
    Staged,
    Valid,
    Invalid,
    Promoted,
    Failed
}

public class RetailTransactionStaging : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string SourceFile { get; private set; } = string.Empty;
    public string SourceHash { get; private set; } = string.Empty;
    public string RawXml { get; private set; } = string.Empty;
    public RetailStagingStatus Status { get; private set; } = RetailStagingStatus.Staged;
    public string StoreCode { get; private set; } = string.Empty;
    public string TransactionNumber { get; private set; } = string.Empty;
    public string OperatorId { get; private set; } = string.Empty;
    public DateTime BusinessDate { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string Currency { get; private set; } = "USD";
    public bool IsReturn { get; private set; }
    public decimal SubTotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal TaxTotal { get; private set; }
    public decimal GrandTotal { get; private set; }
    public string? ValidationMessage { get; private set; }
    public Guid? PromotedTransactionId { get; private set; }
    public Guid? RetailStatementId { get; private set; }
    public DateTime? ValidatedAt { get; private set; }
    public DateTime? PromotedAt { get; private set; }

    private readonly List<RetailTransactionStagingLine> _lines = new();
    private readonly List<RetailTransactionStagingTender> _tenders = new();
    public IReadOnlyList<RetailTransactionStagingLine> Lines => _lines.AsReadOnly();
    public IReadOnlyList<RetailTransactionStagingTender> Tenders => _tenders.AsReadOnly();

    private RetailTransactionStaging() { }

    public RetailTransactionStaging(Guid organizationId, string sourceFile, string sourceHash,
        string rawXml, string storeCode, string transactionNumber, string operatorId,
        DateTime businessDate, DateTime transactionDate, string currency, bool isReturn,
        decimal subTotal, decimal discountTotal, decimal taxTotal, decimal grandTotal)
    {
        OrganizationId = organizationId;
        SourceFile = sourceFile;
        SourceHash = sourceHash;
        RawXml = rawXml;
        StoreCode = storeCode;
        TransactionNumber = transactionNumber;
        OperatorId = operatorId;
        BusinessDate = businessDate.Date;
        TransactionDate = transactionDate;
        Currency = currency.ToUpperInvariant();
        IsReturn = isReturn;
        SubTotal = subTotal;
        DiscountTotal = discountTotal;
        TaxTotal = taxTotal;
        GrandTotal = grandTotal;
    }

    public void AddLine(RetailTransactionStagingLine line) => _lines.Add(line);
    public void AddTender(RetailTransactionStagingTender tender) => _tenders.Add(tender);

    public void MarkValid(string? warning = null)
    {
        Status = RetailStagingStatus.Valid;
        ValidationMessage = warning;
        ValidatedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void MarkInvalid(string error)
    {
        Status = RetailStagingStatus.Invalid;
        ValidationMessage = error;
        ValidatedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void MarkPromoted(Guid transactionId, Guid statementId)
    {
        Status = RetailStagingStatus.Promoted;
        PromotedTransactionId = transactionId;
        RetailStatementId = statementId;
        PromotedAt = DateTime.UtcNow;
        ValidationMessage = null;
        SetUpdated();
    }

    public void MarkFailed(string error)
    {
        Status = RetailStagingStatus.Failed;
        ValidationMessage = error;
        SetUpdated();
    }
}

public class RetailTransactionStagingLine : BaseEntity
{
    public Guid RetailTransactionStagingId { get; private set; }
    public int LineNumber { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string? PosItemId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal LineSubTotal { get; private set; }
    public decimal LineTotal { get; private set; }
    public string UnitOfMeasure { get; private set; } = "EA";
    public bool IsReturn { get; private set; }
    public Guid? MatchedProductVariantId { get; private set; }
    public string? ValidationMessage { get; private set; }

    private RetailTransactionStagingLine() { }

    public RetailTransactionStagingLine(Guid stagingId, int lineNumber, string sku,
        string? posItemId, string productName, decimal quantity, decimal unitPrice,
        decimal discountAmount, decimal taxAmount, decimal lineSubTotal,
        decimal lineTotal, string unitOfMeasure, bool isReturn)
    {
        RetailTransactionStagingId = stagingId;
        LineNumber = lineNumber;
        Sku = sku;
        PosItemId = posItemId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        TaxAmount = taxAmount;
        LineSubTotal = lineSubTotal;
        LineTotal = lineTotal;
        UnitOfMeasure = unitOfMeasure;
        IsReturn = isReturn;
    }

    public void SetProductMatch(Guid? productVariantId, string? message = null)
    {
        MatchedProductVariantId = productVariantId;
        ValidationMessage = message;
        SetUpdated();
    }
}

public class RetailTransactionStagingTender : BaseEntity
{
    public Guid RetailTransactionStagingId { get; private set; }
    public int Sequence { get; private set; }
    public POSPaymentMethod PaymentMethod { get; private set; }
    public decimal Amount { get; private set; }
    public string? Reference { get; private set; }

    private RetailTransactionStagingTender() { }

    public RetailTransactionStagingTender(Guid stagingId, int sequence,
        POSPaymentMethod paymentMethod, decimal amount, string? reference)
    {
        RetailTransactionStagingId = stagingId;
        Sequence = sequence;
        PaymentMethod = paymentMethod;
        Amount = amount;
        Reference = reference;
    }
}
