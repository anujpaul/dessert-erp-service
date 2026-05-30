using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

public class InventoryRecord : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public decimal QuantityOnHand { get; private set; }
    public decimal QuantityReserved { get; private set; }   // held for open sales orders
    public decimal ReorderPoint { get; private set; }        // trigger reorder below this
    public decimal MinimumStock { get; private set; }
    public decimal MaximumStock { get; private set; }
    public string? Location { get; private set; }            // e.g. "A-12-3" (aisle-shelf-bin)
    public DateTime? LastCountDate { get; private set; }

    // Computed
    public decimal QuantityAvailable => QuantityOnHand - QuantityReserved;
    public bool NeedsReorder => QuantityOnHand <= ReorderPoint;

    public ProductVariant? ProductVariant { get; private set; }

    private InventoryRecord() { }

    public InventoryRecord(Guid organizationId, Guid productVariantId,
        decimal quantityOnHand = 0m, decimal reorderPoint = 5m,
        decimal minimumStock = 0m, decimal maximumStock = 100m,
        string? location = null)
    {
        OrganizationId = organizationId;
        ProductVariantId = productVariantId;
        QuantityOnHand = quantityOnHand;
        ReorderPoint = reorderPoint;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
        Location = location;
    }

    public void AdjustQuantity(decimal delta, string reason)
    {
        var newQty = QuantityOnHand + delta;
        if (newQty < 0) throw new InvalidOperationException($"Adjustment would result in negative stock ({newQty}).");
        QuantityOnHand = newQty;
        SetUpdated();
    }

    public void SetOnHand(decimal quantity, DateTime countDate)
    {
        if (quantity < 0) throw new InvalidOperationException("Quantity on hand cannot be negative.");
        QuantityOnHand = quantity;
        LastCountDate = countDate;
        SetUpdated();
    }

    public void Reserve(decimal quantity)
    {
        if (quantity > QuantityAvailable)
            throw new InvalidOperationException($"Cannot reserve {quantity} — only {QuantityAvailable} available.");
        QuantityReserved += quantity;
        SetUpdated();
    }

    public void Unreserve(decimal quantity)
    {
        QuantityReserved = Math.Max(0, QuantityReserved - quantity);
        SetUpdated();
    }

    public void UpdateThresholds(decimal reorderPoint, decimal minStock, decimal maxStock, string? location)
    {
        ReorderPoint = reorderPoint;
        MinimumStock = minStock;
        MaximumStock = maxStock;
        Location = location;
        SetUpdated();
    }
}
