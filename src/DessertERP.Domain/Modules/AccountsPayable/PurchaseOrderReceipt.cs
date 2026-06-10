using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.WarehouseManagement;

/// <summary>
/// Records a single goods-receipt event against a PO.
/// A PO may have many receipts over its lifetime (partial deliveries).
/// </summary>
public class PurchaseOrderReceipt : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid PurchaseOrderId { get; private set; }
    public string ReceiptNumber { get; private set; } = string.Empty;
    public DateTime ReceivedDate { get; private set; }
    public Guid? WarehouseId { get; private set; }
    public Guid? WarehouseLocationId { get; private set; }
    public string? Notes { get; private set; }

    public PurchaseOrder? PurchaseOrder { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public WarehouseLocation? WarehouseLocation { get; private set; }

    private readonly List<PurchaseOrderReceiptLine> _lines = new();
    public IReadOnlyCollection<PurchaseOrderReceiptLine> Lines => _lines.AsReadOnly();

    private PurchaseOrderReceipt() { }

    public PurchaseOrderReceipt(Guid organizationId, Guid purchaseOrderId,
        string receiptNumber, DateTime receivedDate, Guid warehouseId,
        Guid warehouseLocationId, string? notes = null)
    {
        OrganizationId  = organizationId;
        PurchaseOrderId = purchaseOrderId;
        ReceiptNumber   = receiptNumber;
        ReceivedDate    = receivedDate;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        Notes           = notes;
    }

    public PurchaseOrderReceiptLine AddLine(Guid purchaseOrderLineId, decimal qty)
    {
        if (qty <= 0) throw new InvalidOperationException("Receipt quantity must be positive.");
        var line = new PurchaseOrderReceiptLine(Id, purchaseOrderLineId, qty);
        _lines.Add(line);
        return line;
    }
}

/// <summary>One line inside a receipt — maps to a PO line and records how many units arrived.</summary>
public class PurchaseOrderReceiptLine : BaseEntity
{
    public Guid ReceiptId { get; private set; }
    public Guid PurchaseOrderLineId { get; private set; }
    public decimal Qty { get; private set; }

    public PurchaseOrderReceipt? Receipt { get; private set; }
    public PurchaseOrderLine? PurchaseOrderLine { get; private set; }

    private PurchaseOrderReceiptLine() { }

    public PurchaseOrderReceiptLine(Guid receiptId, Guid purchaseOrderLineId, decimal qty)
    {
        ReceiptId            = receiptId;
        PurchaseOrderLineId  = purchaseOrderLineId;
        Qty                  = qty;
    }
}
