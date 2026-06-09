using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class TransferOrderLine : BaseEntity
{
    public Guid    TransferOrderId   { get; private set; }
    public int     LineNumber        { get; private set; }
    public Guid    ProductId         { get; private set; }
    public string  ProductName       { get; private set; } = string.Empty;
    public string? ProductSku        { get; private set; }
    public Guid?   FromLocationId    { get; private set; }
    public Guid?   ToLocationId      { get; private set; }
    public decimal RequestedQuantity { get; private set; }
    public decimal ShippedQuantity   { get; private set; }
    public decimal ReceivedQuantity  { get; private set; }
    public string  UnitOfMeasure     { get; private set; } = "EA";
    public string? LotNumber         { get; private set; }
    public string? Notes             { get; private set; }

    public TransferOrder?    TransferOrder { get; private set; }
    public WarehouseLocation? FromLocation { get; private set; }
    public WarehouseLocation? ToLocation   { get; private set; }

    private TransferOrderLine() { }

    public TransferOrderLine(Guid transferOrderId, int lineNumber, Guid productId,
        string productName, decimal requestedQuantity, string unitOfMeasure = "EA",
        string? productSku = null, Guid? fromLocationId = null,
        Guid? toLocationId = null, string? lotNumber = null, string? notes = null)
    {
        TransferOrderId   = transferOrderId;
        LineNumber         = lineNumber;
        ProductId          = productId;
        ProductName        = productName.Trim();
        RequestedQuantity  = requestedQuantity;
        UnitOfMeasure      = unitOfMeasure;
        ProductSku         = productSku;
        FromLocationId     = fromLocationId;
        ToLocationId       = toLocationId;
        LotNumber          = lotNumber;
        Notes              = notes;
    }

    public void Ship(decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Shipped quantity must be positive.");
        if (ShippedQuantity + quantity > RequestedQuantity)
            throw new InvalidOperationException("Shipped quantity cannot exceed the requested quantity.");
        ShippedQuantity += quantity;
        SetUpdated();
    }

    public void Receive(decimal quantity, Guid? toLocationId = null)
    {
        if (quantity <= 0) throw new ArgumentException("Received quantity must be positive.");
        if (ReceivedQuantity + quantity > ShippedQuantity)
            throw new InvalidOperationException("Received quantity cannot exceed the shipped quantity.");
        ReceivedQuantity += quantity;
        if (toLocationId.HasValue) ToLocationId = toLocationId;
        SetUpdated();
    }

    public void AssignLocations(Guid? fromLocationId, Guid? toLocationId)
    {
        if (fromLocationId.HasValue) FromLocationId = fromLocationId;
        if (toLocationId.HasValue)   ToLocationId   = toLocationId;
        SetUpdated();
    }
}
