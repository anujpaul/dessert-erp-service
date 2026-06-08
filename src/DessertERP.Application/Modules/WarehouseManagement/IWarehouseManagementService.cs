using DessertERP.Application.Modules.WarehouseManagement.DTOs;

namespace DessertERP.Application.Modules.WarehouseManagement;

public interface IWarehouseManagementService
{
    // ── Warehouses ───────────────────────────────────────────────
    Task<List<WarehouseDto>>        GetWarehousesAsync(Guid organizationId);
    Task<WarehouseDto?>             GetWarehouseAsync(Guid id);
    Task<WarehouseDto>              CreateWarehouseAsync(CreateWarehouseDto dto);
    Task<WarehouseDto?>             UpdateWarehouseAsync(Guid id, UpdateWarehouseDto dto);
    Task<bool>                      ActivateWarehouseAsync(Guid id);
    Task<bool>                      DeactivateWarehouseAsync(Guid id);

    // ── Locations ────────────────────────────────────────────────
    Task<List<WarehouseLocationDto>> GetLocationsAsync(Guid warehouseId);
    Task<WarehouseLocationDto>       CreateLocationAsync(CreateWarehouseLocationDto dto);
    Task<bool>                       ActivateLocationAsync(Guid id);
    Task<bool>                       DeactivateLocationAsync(Guid id);

    // ── Inbound Orders ───────────────────────────────────────────
    Task<List<InboundOrderDto>>     GetInboundOrdersAsync(Guid organizationId);
    Task<InboundOrderDto?>          GetInboundOrderAsync(Guid id);
    Task<InboundOrderDto>           CreateInboundOrderAsync(CreateInboundOrderDto dto);
    Task<bool>                      ConfirmInboundOrderAsync(Guid id);
    Task<bool>                      MarkInboundInTransitAsync(Guid id);
    Task<bool>                      StartReceivingInboundAsync(Guid id);
    Task<bool>                      ReceiveInboundLinesAsync(Guid orderId, List<ReceiveInboundLineDto> lines);
    Task<bool>                      CompleteInboundOrderAsync(Guid id);
    Task<bool>                      CancelInboundOrderAsync(Guid id);

    // ── Outbound Orders ──────────────────────────────────────────
    Task<List<OutboundOrderDto>>    GetOutboundOrdersAsync(Guid organizationId);
    Task<OutboundOrderDto?>         GetOutboundOrderAsync(Guid id);
    Task<OutboundOrderDto>          CreateOutboundOrderAsync(CreateOutboundOrderDto dto);
    Task<bool>                      ConfirmOutboundOrderAsync(Guid id);
    Task<bool>                      StartPickingOutboundAsync(Guid id);
    Task<bool>                      PackOutboundOrderAsync(Guid id);
    Task<bool>                      ShipOutboundOrderAsync(Guid id, ShipOutboundOrderDto dto);
    Task<bool>                      DeliverOutboundOrderAsync(Guid id);
    Task<bool>                      CancelOutboundOrderAsync(Guid id);

    // ── Transfer Orders ──────────────────────────────────────────
    Task<List<TransferOrderDto>>    GetTransferOrdersAsync(Guid organizationId);
    Task<TransferOrderDto?>         GetTransferOrderAsync(Guid id);
    Task<TransferOrderDto>          CreateTransferOrderAsync(CreateTransferOrderDto dto);
    Task<bool>                      ConfirmTransferOrderAsync(Guid id);
    Task<bool>                      ShipTransferOrderAsync(Guid id, DateTime shippedDate);
    Task<bool>                      StartReceivingTransferAsync(Guid id);
    Task<bool>                      CompleteTransferOrderAsync(Guid id);
    Task<bool>                      CancelTransferOrderAsync(Guid id);
}
