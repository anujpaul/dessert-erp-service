using DessertERP.Domain.Common;
using DessertERP.Domain.Modules.ProductManagement;

namespace DessertERP.Domain.Modules.WarehouseManagement;

public class WarehouseInventoryBalance : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Guid WarehouseLocationId { get; private set; }
    public decimal QuantityOnHand { get; private set; }
    public decimal QuantityReserved { get; private set; }

    public ProductVariant? ProductVariant { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public WarehouseLocation? WarehouseLocation { get; private set; }

    public decimal QuantityAvailable => QuantityOnHand - QuantityReserved;

    private WarehouseInventoryBalance() { }

    public WarehouseInventoryBalance(
        Guid organizationId,
        Guid productVariantId,
        Guid warehouseId,
        Guid warehouseLocationId,
        decimal quantityOnHand = 0m)
    {
        if (quantityOnHand < 0)
            throw new InvalidOperationException("Warehouse quantity cannot be negative.");

        OrganizationId = organizationId;
        ProductVariantId = productVariantId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        QuantityOnHand = quantityOnHand;
    }

    public void SetOnHand(decimal quantity)
    {
        if (quantity < QuantityReserved)
            throw new InvalidOperationException("Warehouse quantity cannot be less than its reserved quantity.");
        QuantityOnHand = quantity;
        SetUpdated();
    }

    public void Receive(decimal quantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Received quantity must be positive.");
        QuantityOnHand += quantity;
        SetUpdated();
    }

    public void Issue(decimal quantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Issued quantity must be positive.");
        if (quantity > QuantityAvailable)
            throw new InvalidOperationException(
                $"Cannot issue {quantity} from this location - only {QuantityAvailable} available.");
        QuantityOnHand -= quantity;
        SetUpdated();
    }
}
