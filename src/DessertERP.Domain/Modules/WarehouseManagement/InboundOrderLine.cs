using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class InboundOrderLine : BaseEntity
{
    public Guid    InboundOrderId     { get; private set; }
    public int     LineNumber         { get; private set; }
    public Guid    ProductId          { get; private set; }
    public string  ProductName        { get; private set; } = string.Empty;
    public string? ProductSku         { get; private set; }
    public Guid?   LocationId         { get; private set; }   // destination bin
    public decimal OrderedQuantity    { get; private set; }
    public decimal ReceivedQuantity   { get; private set; }
    public string  UnitOfMeasure      { get; private set; } = "EA";
    public string? LotNumber          { get; private set; }
    public DateTime? ExpiryDate       { get; private set; }
    public string?  Notes             { get; private set; }

    public InboundOrder?    InboundOrder { get; private set; }
    public WarehouseLocation? Location  { get; private set; }

    private InboundOrderLine() { }

    public InboundOrderLine(Guid inboundOrderId, int lineNumber, Guid productId,
        string productName, decimal orderedQuantity, string unitOfMeasure = "EA",
        string? productSku = null, Guid? locationId = null,
        string? lotNumber = null, DateTime? expiryDate = null, string? notes = null)
    {
        InboundOrderId   = inboundOrderId;
        LineNumber        = lineNumber;
        ProductId         = productId;
        ProductName       = productName.Trim();
        OrderedQuantity   = orderedQuantity;
        UnitOfMeasure     = unitOfMeasure;
        ProductSku        = productSku;
        LocationId        = locationId;
        LotNumber         = lotNumber;
        ExpiryDate        = expiryDate;
        Notes             = notes;
    }

    public void Receive(decimal quantity, Guid? locationId = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("Received quantity must be positive.");
        if (ReceivedQuantity + quantity > OrderedQuantity)
            throw new InvalidOperationException("Received quantity cannot exceed the ordered quantity.");
        ReceivedQuantity += quantity;
        if (locationId.HasValue) LocationId = locationId;
        SetUpdated();
    }

    public void AssignLocation(Guid locationId) { LocationId = locationId; SetUpdated(); }
    public void SetLot(string lotNumber, DateTime? expiryDate)
    {
        LotNumber  = lotNumber;
        ExpiryDate = expiryDate;
        SetUpdated();
    }
}
