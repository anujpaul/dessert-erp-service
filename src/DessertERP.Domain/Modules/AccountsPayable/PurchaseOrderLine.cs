using DessertERP.Domain.Common;

namespace DessertERP.Domain.Modules.AccountsPayable;

public class PurchaseOrderLine : BaseEntity
{
    public Guid PurchaseOrderId { get; private set; }
    public Guid ProductVariantId { get; private set; }   // FK → product_variants
    public string ProductCode { get; private set; } = string.Empty;  // denormalized SKU for display
    public string Description { get; private set; } = string.Empty;
    public string UnitOfMeasure { get; private set; } = "Each";
    public decimal OrderedQty { get; private set; }
    public decimal ReceivedQty { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TaxRate { get; private set; }

    // Computed
    public decimal LineTotal      => Math.Round(OrderedQty  * UnitCost * (1 + TaxRate / 100), 4);
    public decimal ReceivedTotal  => Math.Round(ReceivedQty * UnitCost * (1 + TaxRate / 100), 4);
    public decimal OutstandingQty => OrderedQty - ReceivedQty;
    public bool    IsFullyReceived => ReceivedQty >= OrderedQty;

    public PurchaseOrder? PurchaseOrder { get; private set; }

    private PurchaseOrderLine() { }

    public PurchaseOrderLine(Guid purchaseOrderId, Guid productVariantId, string productCode,
        string description, string unitOfMeasure, decimal orderedQty, decimal unitCost, decimal taxRate = 0)
    {
        PurchaseOrderId = purchaseOrderId;
        ProductVariantId = productVariantId;
        ProductCode = productCode;
        Description = description;
        UnitOfMeasure = unitOfMeasure;
        OrderedQty = orderedQty;
        UnitCost = unitCost;
        TaxRate = taxRate;
    }

    public void Receive(decimal qty)
    {
        if (qty <= 0) throw new InvalidOperationException("Received quantity must be positive.");
        if (ReceivedQty + qty > OrderedQty)
            throw new InvalidOperationException("Cannot receive more than ordered quantity.");
        ReceivedQty += qty;
        SetUpdated();
    }
}
