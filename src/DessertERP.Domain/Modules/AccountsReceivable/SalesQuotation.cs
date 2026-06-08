using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum QuotationStatus
{
    Draft     = 1,   // being built
    Sent      = 2,   // sent to customer
    Pending   = 3,   // awaiting internal approval before sending
    Accepted  = 4,   // customer accepted
    Rejected  = 5,   // customer rejected
    Expired   = 6,   // past validity date
    Converted = 7,   // converted to a Sales Order
    Cancelled = 8
}

/// <summary>
/// Sales Quotation — a priced offer sent to a customer before a Sales Order is raised.
/// Lifecycle: Draft → (optional workflow) Pending → Sent → Accepted → Converted (to SO).
/// </summary>
public class SalesQuotation : BaseEntity
{
    public Guid   OrganizationId   { get; private set; }
    public string QuotationNumber  { get; private set; } = string.Empty;
    public Guid   CustomerId       { get; private set; }
    public DateTime QuotationDate  { get; private set; }
    public DateTime ValidUntil     { get; private set; }
    public string   Description    { get; private set; } = string.Empty;
    public string   CustomerRef    { get; private set; } = string.Empty;
    public string   Currency       { get; private set; } = "USD";
    public QuotationStatus Status  { get; private set; } = QuotationStatus.Draft;

    // Amounts
    public decimal SubTotal        { get; private set; }
    public decimal TaxTotal        { get; private set; }
    public decimal DiscountTotal   { get; private set; }
    public decimal GrandTotal      { get; private set; }

    // Workflow linkage
    public Guid?  WorkflowInstanceId { get; private set; }
    public string? RejectionReason   { get; private set; }

    // Conversion linkage
    public Guid? ConvertedToSOId   { get; private set; }
    public DateTime? ConvertedAt   { get; private set; }

    public string? Notes           { get; private set; }

    public Customer? Customer      { get; private set; }

    private readonly List<SalesQuotationLine> _lines = new();
    public IReadOnlyCollection<SalesQuotationLine> Lines => _lines.AsReadOnly();

    private SalesQuotation() { }

    public SalesQuotation(Guid organizationId, string quotationNumber, Guid customerId,
        DateTime quotationDate, DateTime validUntil, string description,
        string customerRef = "", string currency = "USD", string? notes = null)
    {
        OrganizationId = organizationId;
        QuotationNumber = quotationNumber.Trim();
        CustomerId      = customerId;
        QuotationDate   = quotationDate;
        ValidUntil      = validUntil;
        Description     = description.Trim();
        CustomerRef     = customerRef;
        Currency        = currency;
        Notes           = notes;
    }

    public SalesQuotationLine AddLine(Guid productVariantId, string sku, string productName,
        string? variantDescription, string uom,
        decimal quantity, decimal unitPrice, decimal taxRate, decimal discountPct = 0)
    {
        if (Status != QuotationStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to a Draft quotation.");
        var line = new SalesQuotationLine(Id, productVariantId, sku, productName,
            variantDescription, uom, quantity, unitPrice, taxRate, discountPct);
        _lines.Add(line);
        RecalcTotals();
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != QuotationStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft quotation.");
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        RecalcTotals();
        SetUpdated();
    }

    /// <summary>Submit for internal approval before sending to customer.</summary>
    public void SubmitForApproval(Guid workflowInstanceId)
    {
        if (Status != QuotationStatus.Draft)
            throw new InvalidOperationException("Only Draft quotations can be submitted for approval.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot submit a quotation with no lines.");
        Status             = QuotationStatus.Pending;
        WorkflowInstanceId = workflowInstanceId;
        SetUpdated();
    }

    /// <summary>Workflow approved — quotation is ready to be sent to the customer.</summary>
    public void WorkflowApproved()
    {
        if (Status != QuotationStatus.Pending)
            throw new InvalidOperationException("Quotation is not pending approval.");
        Status = QuotationStatus.Sent;
        SetUpdated();
    }

    /// <summary>Workflow rejected — quotation returns to Draft for revision.</summary>
    public void WorkflowRejected(string reason)
    {
        if (Status != QuotationStatus.Pending)
            throw new InvalidOperationException("Quotation is not pending approval.");
        Status          = QuotationStatus.Draft;
        RejectionReason = reason;
        SetUpdated();
    }

    /// <summary>Send directly to customer (no approval required).</summary>
    public void Send()
    {
        if (Status != QuotationStatus.Draft)
            throw new InvalidOperationException("Only a Draft quotation can be sent directly.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot send a quotation with no lines.");
        Status = QuotationStatus.Sent;
        SetUpdated();
    }

    /// <summary>Customer accepted the quotation.</summary>
    public void Accept()
    {
        if (Status != QuotationStatus.Sent)
            throw new InvalidOperationException("Only a Sent quotation can be accepted.");
        Status = QuotationStatus.Accepted;
        SetUpdated();
    }

    /// <summary>Customer rejected the quotation.</summary>
    public void Reject(string? reason = null)
    {
        if (Status != QuotationStatus.Sent)
            throw new InvalidOperationException("Only a Sent quotation can be rejected.");
        Status          = QuotationStatus.Rejected;
        RejectionReason = reason;
        SetUpdated();
    }

    /// <summary>Mark expired when ValidUntil has passed.</summary>
    public void Expire()
    {
        if (Status == QuotationStatus.Converted || Status == QuotationStatus.Cancelled)
            return;
        Status = QuotationStatus.Expired;
        SetUpdated();
    }

    /// <summary>Mark as converted once a Sales Order is created.</summary>
    public void MarkConverted(Guid salesOrderId)
    {
        if (Status != QuotationStatus.Accepted)
            throw new InvalidOperationException("Only Accepted quotations can be converted to a Sales Order.");
        Status          = QuotationStatus.Converted;
        ConvertedToSOId = salesOrderId;
        ConvertedAt     = DateTime.UtcNow;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == QuotationStatus.Converted)
            throw new InvalidOperationException("A converted quotation cannot be cancelled.");
        Status = QuotationStatus.Cancelled;
        SetUpdated();
    }

    private void RecalcTotals()
    {
        SubTotal      = _lines.Sum(l => l.LineSubTotal);
        DiscountTotal = _lines.Sum(l => l.DiscountAmount);
        TaxTotal      = _lines.Sum(l => l.TaxAmount);
        GrandTotal    = _lines.Sum(l => l.LineTotal);
    }
}

public class SalesQuotationLine : BaseEntity
{
    public Guid    QuotationId       { get; private set; }
    public int     LineNumber        { get; private set; }
    public Guid    ProductVariantId  { get; private set; }
    public string  Sku               { get; private set; } = string.Empty;
    public string  ProductName       { get; private set; } = string.Empty;
    public string? VariantDescription { get; private set; }
    public string  UnitOfMeasure     { get; private set; } = "EA";
    public decimal Quantity          { get; private set; }
    public decimal UnitPrice         { get; private set; }
    public decimal TaxRate           { get; private set; }   // %
    public decimal DiscountPct       { get; private set; }   // %

    public decimal LineSubTotal  => Math.Round(Quantity * UnitPrice, 4);
    public decimal DiscountAmount => Math.Round(LineSubTotal * DiscountPct / 100, 4);
    public decimal TaxableAmount  => LineSubTotal - DiscountAmount;
    public decimal TaxAmount      => Math.Round(TaxableAmount * TaxRate / 100, 4);
    public decimal LineTotal      => TaxableAmount + TaxAmount;

    public SalesQuotation? Quotation { get; private set; }

    private SalesQuotationLine() { }

    public SalesQuotationLine(Guid quotationId, Guid productVariantId, string sku,
        string productName, string? variantDescription, string uom,
        decimal quantity, decimal unitPrice, decimal taxRate, decimal discountPct = 0)
    {
        QuotationId        = quotationId;
        ProductVariantId   = productVariantId;
        Sku                = sku;
        ProductName        = productName;
        VariantDescription = variantDescription;
        UnitOfMeasure      = uom;
        Quantity           = quantity;
        UnitPrice          = unitPrice;
        TaxRate            = taxRate;
        DiscountPct        = discountPct;
    }

    public void Update(decimal quantity, decimal unitPrice, decimal taxRate, decimal discountPct)
    {
        Quantity    = quantity;
        UnitPrice   = unitPrice;
        TaxRate     = taxRate;
        DiscountPct = discountPct;
        SetUpdated();
    }
}
