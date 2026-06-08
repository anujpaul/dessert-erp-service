using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public enum InboundOrderStatus
{
    Draft     = 1,
    Confirmed = 2,
    InTransit = 3,
    Receiving = 4,
    Completed = 5,
    Cancelled = 6
}

public class InboundOrder : BaseEntity
{
    public Guid               OrganizationId { get; private set; }
    public Guid               WarehouseId    { get; private set; }
    public string             OrderNumber    { get; private set; } = string.Empty;
    public Guid?              PurchaseOrderId{ get; private set; }   // links to AP PO
    public Guid?              VendorId       { get; private set; }
    public string?            VendorName     { get; private set; }
    public DateTime           ExpectedDate   { get; private set; }
    public DateTime?          ReceivedDate   { get; private set; }
    public InboundOrderStatus Status         { get; private set; } = InboundOrderStatus.Draft;
    public string?            Notes          { get; private set; }

    public Warehouse?  Warehouse { get; private set; }
    public ICollection<InboundOrderLine> Lines { get; private set; } = new List<InboundOrderLine>();

    private InboundOrder() { }

    public InboundOrder(Guid organizationId, Guid warehouseId, string orderNumber,
        DateTime expectedDate, Guid? purchaseOrderId = null,
        Guid? vendorId = null, string? vendorName = null, string? notes = null)
    {
        OrganizationId  = organizationId;
        WarehouseId     = warehouseId;
        OrderNumber     = orderNumber.Trim();
        ExpectedDate    = expectedDate;
        PurchaseOrderId = purchaseOrderId;
        VendorId        = vendorId;
        VendorName      = vendorName;
        Notes           = notes;
    }

    public void Confirm()
    {
        if (Status != InboundOrderStatus.Draft)
            throw new InvalidOperationException("Only draft orders can be confirmed.");
        Status = InboundOrderStatus.Confirmed;
        SetUpdated();
    }

    public void MarkInTransit()
    {
        if (Status != InboundOrderStatus.Confirmed)
            throw new InvalidOperationException("Order must be confirmed before marking in transit.");
        Status = InboundOrderStatus.InTransit;
        SetUpdated();
    }

    public void StartReceiving()
    {
        if (Status != InboundOrderStatus.InTransit && Status != InboundOrderStatus.Confirmed)
            throw new InvalidOperationException("Order must be confirmed or in transit to start receiving.");
        Status = InboundOrderStatus.Receiving;
        SetUpdated();
    }

    public void Complete(DateTime receivedDate)
    {
        if (Status != InboundOrderStatus.Receiving)
            throw new InvalidOperationException("Order must be in Receiving status to complete.");
        Status       = InboundOrderStatus.Completed;
        ReceivedDate = receivedDate;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == InboundOrderStatus.Completed)
            throw new InvalidOperationException("Completed orders cannot be cancelled.");
        Status = InboundOrderStatus.Cancelled;
        SetUpdated();
    }

    public void UpdateNotes(string? notes) { Notes = notes; SetUpdated(); }
}
