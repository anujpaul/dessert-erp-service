using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsReceivable;

public enum SalesOrderStatus { Draft, Confirmed, Picking, Shipped, Invoiced, Closed, Cancelled }

public class SalesOrder : BaseEntity
{
    // public Guid Id { get; set; }
    public Guid OrganizationId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? RequestedShipDate { get; private set; }
    public DateTime? ActualShipDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string CustomerRef { get; private set; } = string.Empty;
    public string Currency { get; private set; } = "USD";
    public SalesOrderStatus Status { get; private set; } = SalesOrderStatus.Draft;
    public decimal SubTotal { get; private set; }
    public decimal TaxTotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal GrandTotal { get; private set; }
    public Guid? ARInvoiceId { get; private set; }

    public Customer? Customer { get; private set; }

    private readonly List<SalesOrderLine> _lines = new();
    public IReadOnlyCollection<SalesOrderLine> Lines => _lines.AsReadOnly();

    private SalesOrder() { }

    public SalesOrder(Guid organizationId, string orderNumber, Guid customerId, DateTime orderDate,
        string description, string customerRef, string currency = "USD",
        DateTime? requestedShipDate = null)
    {
        OrganizationId = organizationId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderDate = orderDate;
        Description = description;
        CustomerRef = customerRef;
        Currency = currency;
        RequestedShipDate = requestedShipDate;
    }

    public SalesOrderLine AddLine(Guid productVariantId, string sku, string productName,
        string? variantDescription, string uom,
        decimal quantity, decimal unitPrice, decimal taxRate, decimal discountPct = 0)
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to a Draft order.");

        var line = new SalesOrderLine(Id, productVariantId, sku, productName,
            variantDescription, uom, quantity, unitPrice, taxRate, discountPct);
        _lines.Add(line);
        RecalcTotals();
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft order.");

        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        RecalcTotals();
        SetUpdated();
    }

    public void Confirm()
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Only a Draft order can be confirmed.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot confirm an order with no lines.");
        Status = SalesOrderStatus.Confirmed;
        SetUpdated();
    }

    public void StartPicking()
    {
        if (Status != SalesOrderStatus.Confirmed)
            throw new InvalidOperationException("Only a Confirmed order can be moved to Picking.");
        Status = SalesOrderStatus.Picking;
        SetUpdated();
    }

    public void Ship(DateTime shipDate)
    {
        if (Status != SalesOrderStatus.Picking)
            throw new InvalidOperationException("Only a Picking order can be shipped.");
        Status = SalesOrderStatus.Shipped;
        ActualShipDate = shipDate;
        SetUpdated();
    }

    public void Invoice(Guid arInvoiceId)
    {
        if (Status != SalesOrderStatus.Shipped)
            throw new InvalidOperationException("Only a Shipped order can be invoiced.");
        Status = SalesOrderStatus.Invoiced;
        ARInvoiceId = arInvoiceId;
        SetUpdated();
    }

    public void Close()
    {
        if (Status != SalesOrderStatus.Invoiced)
            throw new InvalidOperationException("Only an Invoiced order can be closed.");
        Status = SalesOrderStatus.Closed;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == SalesOrderStatus.Shipped || Status == SalesOrderStatus.Invoiced || Status == SalesOrderStatus.Closed)
            throw new InvalidOperationException("Cannot cancel an order that has been shipped or invoiced.");
        Status = SalesOrderStatus.Cancelled;
        SetUpdated();
    }

    /// <summary>Called by the service layer when removing a line without loading the collection.</summary>
    public void ValidateCanRemoveLine()
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft order.");
    }

    /// <summary>Recalculates totals from an externally-supplied list of remaining lines.</summary>
    public void RecalcTotalsFromLines(IEnumerable<SalesOrderLine> lines)
    {
        var list = lines.ToList();
        SubTotal = list.Sum(l => l.LineSubTotal);
        DiscountTotal = list.Sum(l => l.DiscountAmount);
        TaxTotal = list.Sum(l => l.TaxAmount);
        GrandTotal = list.Sum(l => l.LineTotal);
        SetUpdated();
    }

    private void RecalcTotals()
    {
        SubTotal = _lines.Sum(l => l.LineSubTotal);
        DiscountTotal = _lines.Sum(l => l.DiscountAmount);
        TaxTotal = _lines.Sum(l => l.TaxAmount);
        GrandTotal = _lines.Sum(l => l.LineTotal);
    }
}
