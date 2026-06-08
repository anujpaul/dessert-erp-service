using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public enum OutboundOrderStatus
{
    Draft     = 1,
    Confirmed = 2,
    Picking   = 3,
    Packed    = 4,
    Shipped   = 5,
    Delivered = 6,
    Cancelled = 7
}

public class OutboundOrder : BaseEntity
{
    public Guid               OrganizationId  { get; private set; }
    public Guid               WarehouseId     { get; private set; }
    public string             OrderNumber     { get; private set; } = string.Empty;
    public Guid?              SalesOrderId    { get; private set; }  // links to AR Sales Order
    public Guid?              CustomerId      { get; private set; }
    public string?            CustomerName    { get; private set; }
    public string?            ShipToAddress   { get; private set; }
    public DateTime           RequestedDate   { get; private set; }
    public DateTime?          ShippedDate     { get; private set; }
    public string?            TrackingNumber  { get; private set; }
    public string?            Carrier         { get; private set; }
    public OutboundOrderStatus Status         { get; private set; } = OutboundOrderStatus.Draft;
    public string?            Notes           { get; private set; }

    public Warehouse? Warehouse { get; private set; }
    public ICollection<OutboundOrderLine> Lines { get; private set; } = new List<OutboundOrderLine>();

    private OutboundOrder() { }

    public OutboundOrder(Guid organizationId, Guid warehouseId, string orderNumber,
        DateTime requestedDate, Guid? salesOrderId = null,
        Guid? customerId = null, string? customerName = null,
        string? shipToAddress = null, string? notes = null)
    {
        OrganizationId = organizationId;
        WarehouseId    = warehouseId;
        OrderNumber    = orderNumber.Trim();
        RequestedDate  = requestedDate;
        SalesOrderId   = salesOrderId;
        CustomerId     = customerId;
        CustomerName   = customerName;
        ShipToAddress  = shipToAddress;
        Notes          = notes;
    }

    public void Confirm()
    {
        if (Status != OutboundOrderStatus.Draft)
            throw new InvalidOperationException("Only draft orders can be confirmed.");
        Status = OutboundOrderStatus.Confirmed;
        SetUpdated();
    }

    public void StartPicking()
    {
        if (Status != OutboundOrderStatus.Confirmed)
            throw new InvalidOperationException("Order must be confirmed before picking.");
        Status = OutboundOrderStatus.Picking;
        SetUpdated();
    }

    public void Pack()
    {
        if (Status != OutboundOrderStatus.Picking)
            throw new InvalidOperationException("Order must be in picking before packing.");
        Status = OutboundOrderStatus.Packed;
        SetUpdated();
    }

    public void Ship(DateTime shippedDate, string? trackingNumber = null, string? carrier = null)
    {
        if (Status != OutboundOrderStatus.Packed)
            throw new InvalidOperationException("Order must be packed before shipping.");
        Status         = OutboundOrderStatus.Shipped;
        ShippedDate    = shippedDate;
        TrackingNumber = trackingNumber;
        Carrier        = carrier;
        SetUpdated();
    }

    public void MarkDelivered()
    {
        if (Status != OutboundOrderStatus.Shipped)
            throw new InvalidOperationException("Order must be shipped before marking delivered.");
        Status = OutboundOrderStatus.Delivered;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == OutboundOrderStatus.Shipped || Status == OutboundOrderStatus.Delivered)
            throw new InvalidOperationException("Shipped or delivered orders cannot be cancelled.");
        Status = OutboundOrderStatus.Cancelled;
        SetUpdated();
    }

    public void UpdateNotes(string? notes) { Notes = notes; SetUpdated(); }
}
