using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public enum TransferOrderStatus
{
    Draft     = 1,
    Confirmed = 2,
    InTransit = 3,
    Receiving = 4,
    Completed = 5,
    Cancelled = 6
}

public class TransferOrder : BaseEntity
{
    public Guid                OrganizationId     { get; private set; }
    public string              OrderNumber        { get; private set; } = string.Empty;
    public Guid                FromWarehouseId    { get; private set; }
    public Guid                ToWarehouseId      { get; private set; }
    public DateTime            RequestedDate      { get; private set; }
    public DateTime?           ShippedDate        { get; private set; }
    public DateTime?           ReceivedDate       { get; private set; }
    public TransferOrderStatus Status             { get; private set; } = TransferOrderStatus.Draft;
    public string?             Notes              { get; private set; }

    public Warehouse? FromWarehouse { get; private set; }
    public Warehouse? ToWarehouse   { get; private set; }
    public ICollection<TransferOrderLine> Lines { get; private set; } = new List<TransferOrderLine>();

    private TransferOrder() { }

    public TransferOrder(Guid organizationId, string orderNumber,
        Guid fromWarehouseId, Guid toWarehouseId,
        DateTime requestedDate, string? notes = null)
    {
        if (fromWarehouseId == toWarehouseId)
            throw new ArgumentException("From and To warehouses must be different.");

        OrganizationId  = organizationId;
        OrderNumber     = orderNumber.Trim();
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId   = toWarehouseId;
        RequestedDate   = requestedDate;
        Notes           = notes;
    }

    public void Confirm()
    {
        if (Status != TransferOrderStatus.Draft)
            throw new InvalidOperationException("Only draft transfers can be confirmed.");
        Status = TransferOrderStatus.Confirmed;
        SetUpdated();
    }

    public void Ship(DateTime shippedDate)
    {
        if (Status != TransferOrderStatus.Confirmed)
            throw new InvalidOperationException("Transfer must be confirmed before shipping.");
        Status      = TransferOrderStatus.InTransit;
        ShippedDate = shippedDate;
        SetUpdated();
    }

    public void StartReceiving()
    {
        if (Status != TransferOrderStatus.InTransit)
            throw new InvalidOperationException("Transfer must be in transit before receiving.");
        Status = TransferOrderStatus.Receiving;
        SetUpdated();
    }

    public void Complete(DateTime receivedDate)
    {
        if (Status != TransferOrderStatus.Receiving)
            throw new InvalidOperationException("Transfer must be in Receiving status to complete.");
        Status       = TransferOrderStatus.Completed;
        ReceivedDate = receivedDate;
        SetUpdated();
    }

    public void Cancel()
    {
        if (Status == TransferOrderStatus.Completed)
            throw new InvalidOperationException("Completed transfers cannot be cancelled.");
        Status = TransferOrderStatus.Cancelled;
        SetUpdated();
    }

    public void UpdateNotes(string? notes) { Notes = notes; SetUpdated(); }
}
