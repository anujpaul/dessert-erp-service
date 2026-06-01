using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public enum PurchaseOrderStatus { Draft, Sent, PartiallyReceived, FullyReceived, Invoiced, Closed, Cancelled }

public class PurchaseOrder : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string PONumber { get; private set; } = string.Empty;
    public Guid VendorId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? ExpectedDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Currency { get; private set; } = "USD";
    public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Draft;
    public decimal SubTotal { get; private set; }
    public decimal TaxTotal { get; private set; }
    public decimal GrandTotal { get; private set; }
    public Guid? APInvoiceId { get; private set; }

    // Export tracking
    public bool      IsExported { get; private set; }
    public DateTime? ExportedAt { get; private set; }

    public Vendor? Vendor { get; private set; }

    private readonly List<PurchaseOrderLine> _lines = new();
    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private PurchaseOrder() { }

    public PurchaseOrder(Guid organizationId, string poNumber, Guid vendorId, DateTime orderDate,
        string description, string currency = "USD", DateTime? expectedDate = null)
    {
        OrganizationId = organizationId;
        PONumber = poNumber;
        VendorId = vendorId;
        OrderDate = orderDate;
        Description = description;
        Currency = currency;
        ExpectedDate = expectedDate;
    }

    public PurchaseOrderLine AddLine(Guid productVariantId, string productCode, string description,
        string uom, decimal qty, decimal unitCost, decimal taxRate = 0)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be added to a Draft PO.");
        var line = new PurchaseOrderLine(Id, productVariantId, productCode, description, uom, qty, unitCost, taxRate);
        _lines.Add(line);
        RecalcTotals();
        SetUpdated();
        return line;
    }

    public void RemoveLine(Guid lineId)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft PO.");
        var line = _lines.FirstOrDefault(l => l.Id == lineId)
            ?? throw new InvalidOperationException("Line not found.");
        _lines.Remove(line);
        RecalcTotals();
        SetUpdated();
    }

    public void Send()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only a Draft PO can be sent.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot send a PO with no lines.");
        Status = PurchaseOrderStatus.Sent;
        SetUpdated();
    }

    public void UpdateReceiptStatus()
    {
        if (Status == PurchaseOrderStatus.Sent || Status == PurchaseOrderStatus.PartiallyReceived)
        {
            bool allReceived = _lines.All(l => l.IsFullyReceived);
            Status = allReceived ? PurchaseOrderStatus.FullyReceived : PurchaseOrderStatus.PartiallyReceived;
            SetUpdated();
        }
    }

    public void Invoice(Guid apInvoiceId)
    {
        if (Status != PurchaseOrderStatus.FullyReceived && Status != PurchaseOrderStatus.PartiallyReceived)
            throw new InvalidOperationException("PO must have received goods before invoicing.");
        Status = PurchaseOrderStatus.Invoiced;
        APInvoiceId = apInvoiceId;
        SetUpdated();
    }

    public void Close()
    {
        if (Status != PurchaseOrderStatus.Invoiced)
            throw new InvalidOperationException("Only an Invoiced PO can be closed.");
        Status = PurchaseOrderStatus.Closed;
        SetUpdated();
    }

    public void MarkExported()
    {
        IsExported = true;
        ExportedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void ResetExport()
    {
        IsExported = false;
        ExportedAt = null;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == PurchaseOrderStatus.FullyReceived || Status == PurchaseOrderStatus.Invoiced || Status == PurchaseOrderStatus.Closed)
            throw new InvalidOperationException("Cannot cancel a PO that has been received or invoiced.");
        Status = PurchaseOrderStatus.Cancelled;
        SetUpdated();
    }

    /// <summary>Called by the service layer when removing a line without loading the collection.</summary>
    public void ValidateCanRemoveLine()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be removed from a Draft PO.");
    }

    /// <summary>Recalculates totals from an externally-supplied list of remaining lines.</summary>
    public void RecalcTotalsFromLines(IEnumerable<PurchaseOrderLine> lines)
    {
        var list = lines.ToList();
        SubTotal = list.Sum(l => Math.Round(l.OrderedQty * l.UnitCost, 4));
        TaxTotal = list.Sum(l => Math.Round(l.OrderedQty * l.UnitCost * l.TaxRate / 100, 4));
        GrandTotal = SubTotal + TaxTotal;
        SetUpdated();
    }

    private void RecalcTotals()
    {
        SubTotal = _lines.Sum(l => Math.Round(l.OrderedQty * l.UnitCost, 4));
        TaxTotal = _lines.Sum(l => Math.Round(l.OrderedQty * l.UnitCost * l.TaxRate / 100, 4));
        GrandTotal = SubTotal + TaxTotal;
    }
}
