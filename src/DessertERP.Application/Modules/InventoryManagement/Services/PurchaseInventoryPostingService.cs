using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Application.Modules.InventoryManagement.Services;

public record PurchaseInventoryLine(
    Guid ProductVariantId,
    decimal Quantity,
    decimal UnitCost);

public interface IPurchaseInventoryPostingService
{
    Task PostPurchaseOrderAsync(
        Guid organizationId,
        Guid purchaseOrderId,
        string purchaseOrderNumber,
        IEnumerable<PurchaseInventoryLine> lines,
        CancellationToken ct = default);

    Task PostReceiptAsync(
        Guid organizationId,
        Guid purchaseOrderId,
        string purchaseOrderNumber,
        string receiptNumber,
        Guid warehouseId,
        Guid warehouseLocationId,
        IEnumerable<PurchaseInventoryLine> lines,
        string? notes = null,
        CancellationToken ct = default);

    Task ReleaseOutstandingAsync(
        Guid organizationId,
        Guid purchaseOrderId,
        string purchaseOrderNumber,
        IEnumerable<PurchaseInventoryLine> lines,
        CancellationToken ct = default);
}

public class PurchaseInventoryPostingService : IPurchaseInventoryPostingService
{
    private readonly IAppDbContext _db;

    public PurchaseInventoryPostingService(IAppDbContext db) => _db = db;

    public async Task PostPurchaseOrderAsync(
        Guid organizationId,
        Guid purchaseOrderId,
        string purchaseOrderNumber,
        IEnumerable<PurchaseInventoryLine> lines,
        CancellationToken ct = default)
    {
        foreach (var line in lines.Where(line => line.Quantity > 0))
        {
            var inventory = await LoadInventoryRecordAsync(line.ProductVariantId, ct);
            inventory.AdjustOnOrder(line.Quantity);
            AddTransaction(
                organizationId, purchaseOrderId, purchaseOrderNumber, inventory,
                InventoryTransactionType.PurchaseOrderPlaced, line.Quantity, line.UnitCost,
                "Purchase order sent to vendor");
        }
    }

    public async Task PostReceiptAsync(
        Guid organizationId,
        Guid purchaseOrderId,
        string purchaseOrderNumber,
        string receiptNumber,
        Guid warehouseId,
        Guid warehouseLocationId,
        IEnumerable<PurchaseInventoryLine> lines,
        string? notes = null,
        CancellationToken ct = default)
    {
        var warehouse = await _db.Warehouses.FirstOrDefaultAsync(
            item => item.Id == warehouseId && item.OrganizationId == organizationId, ct)
            ?? throw new InvalidOperationException("Receiving warehouse not found.");
        if (!warehouse.IsActive)
            throw new InvalidOperationException("The receiving warehouse is inactive.");

        var location = await _db.WarehouseLocations.FirstOrDefaultAsync(
            item => item.Id == warehouseLocationId && item.WarehouseId == warehouseId, ct)
            ?? throw new InvalidOperationException("The receiving location does not belong to the selected warehouse.");
        if (!location.IsActive || !location.IsReceivable)
            throw new InvalidOperationException("The selected location is not active and receivable.");

        foreach (var line in lines.Where(line => line.Quantity > 0))
        {
            var inventory = await LoadInventoryRecordAsync(line.ProductVariantId, ct);
            inventory.ReceiveStock(line.Quantity, line.UnitCost);
            inventory.AdjustOnOrder(-line.Quantity);
            var balance = await _db.WarehouseInventoryBalances.FirstOrDefaultAsync(item =>
                item.ProductVariantId == line.ProductVariantId
                && item.WarehouseId == warehouseId
                && item.WarehouseLocationId == warehouseLocationId, ct);
            if (balance is null)
            {
                balance = new Domain.Modules.WarehouseManagement.WarehouseInventoryBalance(
                    organizationId, line.ProductVariantId, warehouseId, warehouseLocationId);
                _db.WarehouseInventoryBalances.Add(balance);
            }
            balance.Receive(line.Quantity);
            AddTransaction(
                organizationId, purchaseOrderId, receiptNumber, inventory,
                InventoryTransactionType.PurchaseReceipt, line.Quantity, line.UnitCost,
                notes ?? $"Goods received against {purchaseOrderNumber}");
        }
    }

    public async Task ReleaseOutstandingAsync(
        Guid organizationId,
        Guid purchaseOrderId,
        string purchaseOrderNumber,
        IEnumerable<PurchaseInventoryLine> lines,
        CancellationToken ct = default)
    {
        foreach (var line in lines.Where(line => line.Quantity > 0))
        {
            var inventory = await LoadInventoryRecordAsync(line.ProductVariantId, ct);
            inventory.AdjustOnOrder(-line.Quantity);
            AddTransaction(
                organizationId, purchaseOrderId, purchaseOrderNumber, inventory,
                InventoryTransactionType.PurchaseOrderClosed, -line.Quantity, line.UnitCost,
                "Outstanding purchase order quantity removed");
        }
    }

    private async Task<InventoryRecord> LoadInventoryRecordAsync(
        Guid productVariantId, CancellationToken ct) =>
        await _db.InventoryRecords.FirstOrDefaultAsync(
            record => record.ProductVariantId == productVariantId, ct)
        ?? throw new InvalidOperationException("Inventory record not found for a purchase order line.");

    private void AddTransaction(
        Guid organizationId,
        Guid purchaseOrderId,
        string referenceNumber,
        InventoryRecord inventory,
        InventoryTransactionType type,
        decimal quantity,
        decimal unitCost,
        string notes)
    {
        _db.InventoryTransactions.Add(new InventoryTransaction(
            organizationId,
            inventory.ProductVariantId,
            type,
            quantity,
            unitCost,
            inventory.QuantityOnHand,
            referenceNumber,
            purchaseOrderId,
            notes));
    }
}
