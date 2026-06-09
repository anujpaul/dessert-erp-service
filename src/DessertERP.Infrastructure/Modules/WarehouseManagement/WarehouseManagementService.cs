using DessertERP.Application.Modules.WarehouseManagement;
using DessertERP.Application.Modules.WarehouseManagement.DTOs;
using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.WarehouseManagement;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DessertERP.Infrastructure.Modules.WarehouseManagement;

public class WarehouseManagementService : IWarehouseManagementService
{
    private readonly AppDbContext _db;
    private readonly ICurrentOrganizationService _org;

    public WarehouseManagementService(AppDbContext db, ICurrentOrganizationService org)
    {
        _db = db;
        _org = org;
    }

    // ── Warehouses ──────────────────────────────────────────────────────────

    public async Task<List<WarehouseDto>> GetWarehousesAsync() =>
        await _db.Warehouses
            .Where(w => w.OrganizationId == _org.OrganizationId)
            .OrderBy(w => w.Code)
            .Select(w => ToWarehouseDto(w))
            .ToListAsync();

    public async Task<WarehouseDto?> GetWarehouseAsync(Guid id)
    {
        var w = await _db.Warehouses.Include(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id == id);
        return w is null ? null : ToWarehouseDto(w);
    }

    public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto)
    {
        var warehouse = new Warehouse(_org.OrganizationId, dto.Code, dto.Name,
            dto.Address, dto.City, dto.Country, dto.IsDefault);
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();
        return ToWarehouseDto(warehouse);
    }

    public async Task<WarehouseDto?> UpdateWarehouseAsync(Guid id, UpdateWarehouseDto dto)
    {
        var warehouse = await _db.Warehouses.FindAsync(id);
        if (warehouse is null) return null;
        warehouse.Update(dto.Name, dto.Address, dto.City, dto.Country);
        await _db.SaveChangesAsync();
        return ToWarehouseDto(warehouse);
    }

    public async Task<bool> ActivateWarehouseAsync(Guid id)
    {
        var w = await _db.Warehouses.FindAsync(id);
        if (w is null) return false;
        w.Activate();
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateWarehouseAsync(Guid id)
    {
        var w = await _db.Warehouses.FindAsync(id);
        if (w is null) return false;
        w.Deactivate();
        await _db.SaveChangesAsync();
        return true;
    }

    // ── Locations ───────────────────────────────────────────────────────────

    public async Task<List<WarehouseLocationDto>> GetLocationsAsync(Guid warehouseId) =>
        await _db.WarehouseLocations
            .Where(l => l.WarehouseId == warehouseId)
            .OrderBy(l => l.Code)
            .Select(l => ToLocationDto(l))
            .ToListAsync();

    public async Task<WarehouseLocationDto> CreateLocationAsync(CreateWarehouseLocationDto dto)
    {
        var loc = new WarehouseLocation(dto.WarehouseId, dto.Code,
            dto.Zone, dto.Aisle, dto.Bay, dto.Level, dto.Bin,
            dto.IsPickable, dto.IsReceivable);
        _db.WarehouseLocations.Add(loc);
        await _db.SaveChangesAsync();
        return ToLocationDto(loc);
    }

    public async Task<bool> ActivateLocationAsync(Guid id)
    {
        var loc = await _db.WarehouseLocations.FindAsync(id);
        if (loc is null) return false;
        loc.Activate();
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateLocationAsync(Guid id)
    {
        var loc = await _db.WarehouseLocations.FindAsync(id);
        if (loc is null) return false;
        loc.Deactivate();
        await _db.SaveChangesAsync();
        return true;
    }

    // ── Inbound Orders ──────────────────────────────────────────────────────

    public async Task<List<InboundOrderDto>> GetInboundOrdersAsync(Guid organizationId) =>
        await _db.InboundOrders
            .Include(o => o.Warehouse)
            .Include(o => o.Lines)
            .Where(o => o.OrganizationId == organizationId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => ToInboundDto(o))
            .ToListAsync();

    public async Task<InboundOrderDto?> GetInboundOrderAsync(Guid id)
    {
        var o = await _db.InboundOrders
            .Include(x => x.Warehouse)
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id);
        return o is null ? null : ToInboundDto(o);
    }

    public async Task<InboundOrderDto> CreateInboundOrderAsync(CreateInboundOrderDto dto)
    {
        var order = new InboundOrder(dto.OrganizationId, dto.WarehouseId, dto.OrderNumber,
            dto.ExpectedDate, dto.PurchaseOrderId, dto.VendorId, dto.VendorName, dto.Notes);

        int line = 1;
        foreach (var l in dto.Lines)
            order.Lines.Add(new InboundOrderLine(order.Id, line++, l.ProductId, l.ProductName,
                l.OrderedQuantity, l.UnitOfMeasure, l.ProductSku, l.LocationId, l.LotNumber, l.ExpiryDate));

        _db.InboundOrders.Add(order);
        await _db.SaveChangesAsync();

        await _db.Entry(order).Reference(x => x.Warehouse).LoadAsync();
        return ToInboundDto(order);
    }

    public async Task<bool> ConfirmInboundOrderAsync(Guid id)          => await AdvanceInbound(id, o => o.Confirm());
    public async Task<bool> MarkInboundInTransitAsync(Guid id)         => await AdvanceInbound(id, o => o.MarkInTransit());
    public async Task<bool> StartReceivingInboundAsync(Guid id)        => await AdvanceInbound(id, o => o.StartReceiving());
    public async Task<bool> CompleteInboundOrderAsync(Guid id)         => await AdvanceInbound(id, o => o.Complete(DateTime.UtcNow));
    public async Task<bool> CancelInboundOrderAsync(Guid id)           => await AdvanceInbound(id, o => o.Cancel());

    public async Task<bool> ReceiveInboundLinesAsync(Guid orderId, List<ReceiveInboundLineDto> lines)
    {
        var order = await _db.InboundOrders.Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null) return false;

        foreach (var r in lines)
        {
            var line = order.Lines.FirstOrDefault(l => l.Id == r.LineId);
            if (line is null) continue;

            var locationId = r.LocationId ?? line.LocationId
                ?? throw new InvalidOperationException($"A receiving location is required for line {line.LineNumber}.");
            await ValidateLocation(locationId, order.WarehouseId, requireReceivable: true);

            var inventory = await GetInventoryRecord(line.ProductId);
            var balance = await GetOrCreateBalance(
                order.OrganizationId, line.ProductId, order.WarehouseId, locationId);

            line.Receive(r.Quantity, locationId);
            balance.Receive(r.Quantity);
            inventory.ReceiveStock(r.Quantity, inventory.AverageCost);
        }
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AdvanceInbound(Guid id, Action<InboundOrder> action)
    {
        var o = await _db.InboundOrders.FindAsync(id);
        if (o is null) return false;
        action(o);
        await _db.SaveChangesAsync();
        return true;
    }

    // ── Outbound Orders ─────────────────────────────────────────────────────

    public async Task<List<OutboundOrderDto>> GetOutboundOrdersAsync(Guid organizationId) =>
        await _db.OutboundOrders
            .Include(o => o.Warehouse)
            .Include(o => o.Lines)
            .Where(o => o.OrganizationId == organizationId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => ToOutboundDto(o))
            .ToListAsync();

    public async Task<OutboundOrderDto?> GetOutboundOrderAsync(Guid id)
    {
        var o = await _db.OutboundOrders
            .Include(x => x.Warehouse)
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id);
        return o is null ? null : ToOutboundDto(o);
    }

    public async Task<OutboundOrderDto> CreateOutboundOrderAsync(CreateOutboundOrderDto dto)
    {
        var order = new OutboundOrder(dto.OrganizationId, dto.WarehouseId, dto.OrderNumber,
            dto.RequestedDate, dto.SalesOrderId, dto.CustomerId, dto.CustomerName,
            dto.ShipToAddress, dto.Notes);

        int line = 1;
        foreach (var l in dto.Lines)
            order.Lines.Add(new OutboundOrderLine(order.Id, line++, l.ProductId, l.ProductName,
                l.RequestedQuantity, l.UnitOfMeasure, l.ProductSku, l.FromLocationId, l.LotNumber));

        _db.OutboundOrders.Add(order);
        await _db.SaveChangesAsync();
        await _db.Entry(order).Reference(x => x.Warehouse).LoadAsync();
        return ToOutboundDto(order);
    }

    public async Task<bool> ConfirmOutboundOrderAsync(Guid id)  => await AdvanceOutbound(id, o => o.Confirm());
    public async Task<bool> StartPickingOutboundAsync(Guid id)  => await AdvanceOutbound(id, o => o.StartPicking());
    public async Task<bool> PackOutboundOrderAsync(Guid id)     => await AdvanceOutbound(id, o => o.Pack());
    public async Task<bool> DeliverOutboundOrderAsync(Guid id)  => await AdvanceOutbound(id, o => o.MarkDelivered());
    public async Task<bool> CancelOutboundOrderAsync(Guid id)   => await AdvanceOutbound(id, o => o.Cancel());

    public async Task<bool> ShipOutboundOrderAsync(Guid id, ShipOutboundOrderDto dto)
    {
        var o = await _db.OutboundOrders.Include(x => x.Lines).FirstOrDefaultAsync(x => x.Id == id);
        if (o is null) return false;

        foreach (var line in o.Lines)
        {
            var quantity = line.RequestedQuantity - line.ShippedQuantity;
            if (quantity <= 0) continue;
            var locationId = line.FromLocationId
                ?? throw new InvalidOperationException($"A pick location is required for line {line.LineNumber}.");
            await ValidateLocation(locationId, o.WarehouseId, requirePickable: true);

            var inventory = await GetInventoryRecord(line.ProductId);
            var balance = await GetExistingBalance(line.ProductId, o.WarehouseId, locationId);
            balance.Issue(quantity);
            inventory.IssueStock(quantity);
            line.Pick(quantity);
            line.Ship(quantity);
        }

        o.Ship(dto.ShippedDate, dto.TrackingNumber, dto.Carrier);
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AdvanceOutbound(Guid id, Action<OutboundOrder> action)
    {
        var o = await _db.OutboundOrders.FindAsync(id);
        if (o is null) return false;
        action(o);
        await _db.SaveChangesAsync();
        return true;
    }

    // ── Transfer Orders ─────────────────────────────────────────────────────

    public async Task<List<TransferOrderDto>> GetTransferOrdersAsync(Guid organizationId) =>
        await _db.TransferOrders
            .Include(o => o.FromWarehouse)
            .Include(o => o.ToWarehouse)
            .Include(o => o.Lines)
            .Where(o => o.OrganizationId == organizationId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => ToTransferDto(o))
            .ToListAsync();

    public async Task<TransferOrderDto?> GetTransferOrderAsync(Guid id)
    {
        var o = await _db.TransferOrders
            .Include(x => x.FromWarehouse)
            .Include(x => x.ToWarehouse)
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id);
        return o is null ? null : ToTransferDto(o);
    }

    public async Task<TransferOrderDto> CreateTransferOrderAsync(CreateTransferOrderDto dto)
    {
        var order = new TransferOrder(dto.OrganizationId, dto.OrderNumber,
            dto.FromWarehouseId, dto.ToWarehouseId, dto.RequestedDate, dto.Notes);

        int line = 1;
        foreach (var l in dto.Lines)
            order.Lines.Add(new TransferOrderLine(order.Id, line++, l.ProductId, l.ProductName,
                l.RequestedQuantity, l.UnitOfMeasure, l.ProductSku,
                l.FromLocationId, l.ToLocationId, l.LotNumber));

        _db.TransferOrders.Add(order);
        await _db.SaveChangesAsync();
        await _db.Entry(order).Reference(x => x.FromWarehouse).LoadAsync();
        await _db.Entry(order).Reference(x => x.ToWarehouse).LoadAsync();
        return ToTransferDto(order);
    }

    public async Task<bool> ConfirmTransferOrderAsync(Guid id)        => await AdvanceTransfer(id, o => o.Confirm());
    public async Task<bool> StartReceivingTransferAsync(Guid id)      => await AdvanceTransfer(id, o => o.StartReceiving());
    public async Task<bool> CompleteTransferOrderAsync(Guid id)
    {
        var o = await _db.TransferOrders.Include(x => x.Lines).FirstOrDefaultAsync(x => x.Id == id);
        if (o is null) return false;

        foreach (var line in o.Lines)
        {
            var quantity = line.ShippedQuantity - line.ReceivedQuantity;
            if (quantity <= 0) continue;
            var locationId = line.ToLocationId
                ?? throw new InvalidOperationException($"A destination location is required for line {line.LineNumber}.");
            await ValidateLocation(locationId, o.ToWarehouseId, requireReceivable: true);
            var balance = await GetOrCreateBalance(
                o.OrganizationId, line.ProductId, o.ToWarehouseId, locationId);
            balance.Receive(quantity);
            line.Receive(quantity, locationId);
        }

        o.Complete(DateTime.UtcNow);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> CancelTransferOrderAsync(Guid id)         => await AdvanceTransfer(id, o => o.Cancel());

    public async Task<bool> ShipTransferOrderAsync(Guid id, DateTime shippedDate)
    {
        var o = await _db.TransferOrders.Include(x => x.Lines).FirstOrDefaultAsync(x => x.Id == id);
        if (o is null) return false;

        foreach (var line in o.Lines)
        {
            var quantity = line.RequestedQuantity - line.ShippedQuantity;
            if (quantity <= 0) continue;
            var locationId = line.FromLocationId
                ?? throw new InvalidOperationException($"A source location is required for line {line.LineNumber}.");
            await ValidateLocation(locationId, o.FromWarehouseId, requirePickable: true);
            var balance = await GetExistingBalance(line.ProductId, o.FromWarehouseId, locationId);
            balance.Issue(quantity);
            line.Ship(quantity);
        }

        o.Ship(shippedDate);
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AdvanceTransfer(Guid id, Action<TransferOrder> action)
    {
        var o = await _db.TransferOrders.FindAsync(id);
        if (o is null) return false;
        action(o);
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<InventoryRecord> GetInventoryRecord(Guid productVariantId) =>
        await _db.InventoryRecords.FirstOrDefaultAsync(r => r.ProductVariantId == productVariantId)
        ?? throw new InvalidOperationException("Inventory record was not found for the warehouse order line.");

    private async Task<WarehouseInventoryBalance> GetExistingBalance(
        Guid productVariantId, Guid warehouseId, Guid locationId) =>
        await _db.WarehouseInventoryBalances.FirstOrDefaultAsync(b =>
            b.ProductVariantId == productVariantId
            && b.WarehouseId == warehouseId
            && b.WarehouseLocationId == locationId)
        ?? throw new InvalidOperationException("No inventory is allocated to the selected warehouse location.");

    private async Task<WarehouseInventoryBalance> GetOrCreateBalance(
        Guid organizationId, Guid productVariantId, Guid warehouseId, Guid locationId)
    {
        var balance = await _db.WarehouseInventoryBalances.FirstOrDefaultAsync(b =>
            b.ProductVariantId == productVariantId
            && b.WarehouseId == warehouseId
            && b.WarehouseLocationId == locationId);
        if (balance is not null) return balance;

        balance = new WarehouseInventoryBalance(
            organizationId, productVariantId, warehouseId, locationId);
        _db.WarehouseInventoryBalances.Add(balance);
        return balance;
    }

    private async Task ValidateLocation(
        Guid locationId, Guid warehouseId, bool requirePickable = false, bool requireReceivable = false)
    {
        var location = await _db.WarehouseLocations.FirstOrDefaultAsync(l =>
            l.Id == locationId && l.WarehouseId == warehouseId);
        if (location is null)
            throw new InvalidOperationException("The selected location does not belong to the order warehouse.");
        if (!location.IsActive)
            throw new InvalidOperationException("The selected warehouse location is inactive.");
        if (requirePickable && !location.IsPickable)
            throw new InvalidOperationException("The selected warehouse location is not pickable.");
        if (requireReceivable && !location.IsReceivable)
            throw new InvalidOperationException("The selected warehouse location is not receivable.");
    }

    // ── Mapping helpers ─────────────────────────────────────────────────────

    private static WarehouseDto ToWarehouseDto(Warehouse w) => new(
        w.Id, w.OrganizationId, w.Code, w.Name, w.Address, w.City, w.Country,
        w.IsActive, w.IsDefault, w.Locations.Count);

    private static WarehouseLocationDto ToLocationDto(WarehouseLocation l) => new(
        l.Id, l.WarehouseId, l.Code, l.Zone, l.Aisle, l.Bay, l.Level, l.Bin,
        l.IsActive, l.IsPickable, l.IsReceivable);

    private static InboundOrderDto ToInboundDto(InboundOrder o) => new(
        o.Id, o.OrganizationId, o.WarehouseId, o.Warehouse?.Name ?? string.Empty,
        o.OrderNumber, o.PurchaseOrderId, o.VendorId, o.VendorName,
        o.ExpectedDate, o.ReceivedDate, o.Status.ToString(), o.Notes,
        o.Lines.Select(l => new InboundOrderLineDto(
            l.Id, l.LineNumber, l.ProductId, l.ProductName, l.ProductSku,
            l.LocationId, l.Location?.Code, l.OrderedQuantity, l.ReceivedQuantity,
            l.UnitOfMeasure, l.LotNumber, l.ExpiryDate, l.Notes)).ToList());

    private static OutboundOrderDto ToOutboundDto(OutboundOrder o) => new(
        o.Id, o.OrganizationId, o.WarehouseId, o.Warehouse?.Name ?? string.Empty,
        o.OrderNumber, o.SalesOrderId, o.CustomerId, o.CustomerName,
        o.ShipToAddress, o.RequestedDate, o.ShippedDate, o.TrackingNumber, o.Carrier,
        o.Status.ToString(), o.Notes,
        o.Lines.Select(l => new OutboundOrderLineDto(
            l.Id, l.LineNumber, l.ProductId, l.ProductName, l.ProductSku,
            l.FromLocationId, l.FromLocation?.Code, l.RequestedQuantity,
            l.PickedQuantity, l.ShippedQuantity, l.UnitOfMeasure, l.LotNumber, l.Notes)).ToList());

    private static TransferOrderDto ToTransferDto(TransferOrder o) => new(
        o.Id, o.OrganizationId, o.OrderNumber,
        o.FromWarehouseId, o.FromWarehouse?.Name ?? string.Empty,
        o.ToWarehouseId,   o.ToWarehouse?.Name   ?? string.Empty,
        o.RequestedDate, o.ShippedDate, o.ReceivedDate, o.Status.ToString(), o.Notes,
        o.Lines.Select(l => new TransferOrderLineDto(
            l.Id, l.LineNumber, l.ProductId, l.ProductName, l.ProductSku,
            l.FromLocationId, l.FromLocation?.Code,
            l.ToLocationId,   l.ToLocation?.Code,
            l.RequestedQuantity, l.ShippedQuantity, l.ReceivedQuantity,
            l.UnitOfMeasure, l.LotNumber, l.Notes)).ToList());
}
