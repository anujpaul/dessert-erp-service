using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.ProductManagement;

public class InventoryRecord : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public Guid ProductVariantId { get; private set; }

    // ── Quantity buckets ────────────────────────────────────────────────────
    public decimal QuantityOnHand { get; private set; }
    public decimal QuantityReserved { get; private set; }
    public decimal QuantityOnOrder { get; private set; }

    // ── Thresholds ──────────────────────────────────────────────────────────
    public decimal ReorderPoint { get; private set; }
    public decimal MinimumStock { get; private set; }
    public decimal MaximumStock { get; private set; }

    // ── Costing ─────────────────────────────────────────────────────────────
    public decimal AverageCost { get; private set; }

    // ── Metadata ────────────────────────────────────────────────────────────
    public string? Location { get; private set; }
    public DateTime? LastCountDate { get; private set; }
    public DateTime? LastReceivedDate { get; private set; }

    // ── Computed ────────────────────────────────────────────────────────────
    public decimal QuantityAvailable => QuantityOnHand - QuantityReserved;
    public decimal QuantityProjected => QuantityOnHand + QuantityOnOrder - QuantityReserved;
    public bool NeedsReorder => QuantityOnHand <= ReorderPoint;
    public bool IsOutOfStock => QuantityOnHand <= 0;
    public decimal StockValue => QuantityOnHand * AverageCost;

    public ProductVariant? ProductVariant { get; private set; }

    private InventoryRecord() { }

    // Positional: (orgId, variantId, onHand, reorderPoint, minStock, maxStock, location)
    public InventoryRecord(Guid organizationId, Guid productVariantId,
        decimal quantityOnHand = 0m, decimal reorderPoint = 5m,
        decimal minimumStock = 0m, decimal maximumStock = 9999m,
        string? location = null)
    {
        OrganizationId   = organizationId;
        ProductVariantId = productVariantId;
        QuantityOnHand   = quantityOnHand;
        ReorderPoint     = reorderPoint;
        MinimumStock     = minimumStock;
        MaximumStock     = maximumStock;
        Location         = location;
    }

    // ── Mutation methods ────────────────────────────────────────────────────

    /// <summary>Signed delta adjust (positive = in, negative = out). Used by POS, returns, manual.</summary>
    public void AdjustQuantity(decimal delta, string? reason = null)
    {
        QuantityOnHand = Math.Max(0, QuantityOnHand + delta);
        SetUpdated();
    }

    /// <summary>Receive stock from a PO, updating weighted-average cost.</summary>
    public void ReceiveStock(decimal qty, decimal unitCost)
    {
        if (qty <= 0) throw new InvalidOperationException("Received quantity must be positive.");
        var totalValue   = (QuantityOnHand * AverageCost) + (qty * unitCost);
        var totalQty     = QuantityOnHand + qty;
        AverageCost      = totalQty > 0 ? totalValue / totalQty : unitCost;
        QuantityOnHand  += qty;
        LastReceivedDate = DateTime.UtcNow;
        SetUpdated();
    }

    /// <summary>Issue stock outbound (sale shipment, vendor return).</summary>
    public void IssueStock(decimal qty)
    {
        if (qty <= 0) throw new InvalidOperationException("Issued quantity must be positive.");
        if (qty > QuantityOnHand)
            throw new InvalidOperationException($"Cannot issue {qty} — only {QuantityOnHand} on hand.");
        QuantityOnHand -= qty;
        SetUpdated();
    }

    /// <summary>Cycle count — set absolute on-hand.</summary>
    public void SetOnHand(decimal quantity, DateTime? countDate = null)
    {
        if (quantity < 0) throw new InvalidOperationException("Quantity on hand cannot be negative.");
        QuantityOnHand = quantity;
        LastCountDate  = countDate ?? DateTime.UtcNow;
        SetUpdated();
    }

    public void Reserve(decimal quantity, decimal backorderLimit = 0m)
    {
        if (quantity <= 0) throw new InvalidOperationException("Reserved quantity must be positive.");
        if (backorderLimit < 0) throw new InvalidOperationException("Backorder limit cannot be negative.");
        if (quantity > QuantityAvailable + backorderLimit)
            throw new InvalidOperationException(
                $"Cannot reserve {quantity} - only {QuantityAvailable} available and {backorderLimit} allowed for backorder.");
        QuantityReserved += quantity;
        SetUpdated();
    }

    public void Unreserve(decimal quantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Unreserved quantity must be positive.");
        QuantityReserved = Math.Max(0, QuantityReserved - quantity);
        SetUpdated();
    }

    public void SetOnOrder(decimal qty)
    {
        QuantityOnOrder = Math.Max(0, qty);
        SetUpdated();
    }

    public void SetThresholds(decimal reorderPoint, decimal minimumStock, decimal maximumStock,
        string? location = null)
    {
        ReorderPoint = reorderPoint;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
        if (location is not null) Location = location;
        SetUpdated();
    }

    public void SetLocation(string? location)
    {
        Location = location;
        SetUpdated();
    }
}
