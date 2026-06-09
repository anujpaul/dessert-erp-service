using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class OutboundOrderLine : BaseEntity
{
    public Guid    OutboundOrderId   { get; private set; }
    public int     LineNumber        { get; private set; }
    public Guid    ProductId         { get; private set; }
    public string  ProductName       { get; private set; } = string.Empty;
    public string? ProductSku        { get; private set; }
    public Guid?   FromLocationId    { get; private set; }  // pick from this bin
    public decimal RequestedQuantity { get; private set; }
    public decimal PickedQuantity    { get; private set; }
    public decimal ShippedQuantity   { get; private set; }
    public string  UnitOfMeasure     { get; private set; } = "EA";
    public string? LotNumber         { get; private set; }
    public string? Notes             { get; private set; }

    public OutboundOrder?    OutboundOrder { get; private set; }
    public WarehouseLocation? FromLocation { get; private set; }

    private OutboundOrderLine() { }

    public OutboundOrderLine(Guid outboundOrderId, int lineNumber, Guid productId,
        string productName, decimal requestedQuantity, string unitOfMeasure = "EA",
        string? productSku = null, Guid? fromLocationId = null,
        string? lotNumber = null, string? notes = null)
    {
        OutboundOrderId   = outboundOrderId;
        LineNumber         = lineNumber;
        ProductId          = productId;
        ProductName        = productName.Trim();
        RequestedQuantity  = requestedQuantity;
        UnitOfMeasure      = unitOfMeasure;
        ProductSku         = productSku;
        FromLocationId     = fromLocationId;
        LotNumber          = lotNumber;
        Notes              = notes;
    }

    public void Pick(decimal quantity, Guid? fromLocationId = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("Picked quantity must be positive.");
        if (PickedQuantity + quantity > RequestedQuantity)
            throw new InvalidOperationException("Picked quantity cannot exceed the requested quantity.");
        PickedQuantity += quantity;
        if (fromLocationId.HasValue) FromLocationId = fromLocationId;
        SetUpdated();
    }

    public void Ship(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Shipped quantity must be positive.");
        if (ShippedQuantity + quantity > PickedQuantity)
            throw new InvalidOperationException("Shipped quantity cannot exceed the picked quantity.");
        ShippedQuantity += quantity;
        SetUpdated();
    }

    public void AssignLocation(Guid fromLocationId) { FromLocationId = fromLocationId; SetUpdated(); }
}
