using DessertERP.Application.Modules.WarehouseManagement;
using DessertERP.Application.Common.Security;
using DessertERP.Application.Modules.WarehouseManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertERP.Api.Controllers;

[Authorize]
[Authorize(Policy = PermissionKeys.InventoryAccess)]
[ApiController]
[Route("api/warehouse")]
public class WarehouseManagementController : ControllerBase
{
    private readonly IWarehouseManagementService _svc;
    public WarehouseManagementController(IWarehouseManagementService svc) => _svc = svc;

    // ── Warehouses ───────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetWarehouses() =>
        Ok(await _svc.GetWarehousesAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWarehouse(Guid id)
    {
        var result = await _svc.GetWarehouseAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionKeys.WarehouseManage)]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto)
    {
        try { return StatusCode(201, await _svc.CreateWarehouseAsync(dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionKeys.WarehouseManage)]
    public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] UpdateWarehouseDto dto)
    {
        var result = await _svc.UpdateWarehouseAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id) =>
        await _svc.ActivateWarehouseAsync(id) ? Ok() : NotFound();

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id) =>
        await _svc.DeactivateWarehouseAsync(id) ? Ok() : NotFound();

    // ── Locations ────────────────────────────────────────────────────────────

    [HttpGet("{warehouseId:guid}/locations")]
    public async Task<IActionResult> GetLocations(Guid warehouseId) =>
        Ok(await _svc.GetLocationsAsync(warehouseId));

    [HttpPost("locations")]
    [Authorize(Policy = PermissionKeys.WarehouseManage)]
    public async Task<IActionResult> CreateLocation([FromBody] CreateWarehouseLocationDto dto) =>
        Ok(await _svc.CreateLocationAsync(dto));

    [HttpPost("locations/{id:guid}/activate")]
    public async Task<IActionResult> ActivateLocation(Guid id) =>
        await _svc.ActivateLocationAsync(id) ? Ok() : NotFound();

    [HttpPost("locations/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateLocation(Guid id) =>
        await _svc.DeactivateLocationAsync(id) ? Ok() : NotFound();

    // ── Inbound Orders ───────────────────────────────────────────────────────

    [HttpGet("inbound")]
    public async Task<IActionResult> GetInboundOrders([FromQuery] Guid organizationId) =>
        Ok(await _svc.GetInboundOrdersAsync(organizationId));

    [HttpGet("inbound/{id:guid}")]
    public async Task<IActionResult> GetInboundOrder(Guid id)
    {
        var result = await _svc.GetInboundOrderAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("inbound")]
    public async Task<IActionResult> CreateInboundOrder([FromBody] CreateInboundOrderDto dto) =>
        Ok(await _svc.CreateInboundOrderAsync(dto));

    [HttpPost("inbound/{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmInbound(Guid id) =>
        await _svc.ConfirmInboundOrderAsync(id) ? Ok() : NotFound();

    [HttpPost("inbound/{id:guid}/in-transit")]
    public async Task<IActionResult> InboundInTransit(Guid id) =>
        await _svc.MarkInboundInTransitAsync(id) ? Ok() : NotFound();

    [HttpPost("inbound/{id:guid}/start-receiving")]
    public async Task<IActionResult> StartReceivingInbound(Guid id) =>
        await _svc.StartReceivingInboundAsync(id) ? Ok() : NotFound();

    [HttpPost("inbound/{id:guid}/receive-lines")]
    [Authorize(Policy = PermissionKeys.WarehouseOperate)]
    public async Task<IActionResult> ReceiveLines(Guid id, [FromBody] List<ReceiveInboundLineDto> lines)
    {
        try { return await _svc.ReceiveInboundLinesAsync(id, lines) ? Ok() : NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("inbound/{id:guid}/complete")]
    public async Task<IActionResult> CompleteInbound(Guid id) =>
        await _svc.CompleteInboundOrderAsync(id) ? Ok() : NotFound();

    [HttpPost("inbound/{id:guid}/cancel")]
    public async Task<IActionResult> CancelInbound(Guid id) =>
        await _svc.CancelInboundOrderAsync(id) ? Ok() : NotFound();

    // ── Outbound Orders ──────────────────────────────────────────────────────

    [HttpGet("outbound")]
    public async Task<IActionResult> GetOutboundOrders([FromQuery] Guid organizationId) =>
        Ok(await _svc.GetOutboundOrdersAsync(organizationId));

    [HttpGet("outbound/{id:guid}")]
    public async Task<IActionResult> GetOutboundOrder(Guid id)
    {
        var result = await _svc.GetOutboundOrderAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("outbound")]
    public async Task<IActionResult> CreateOutboundOrder([FromBody] CreateOutboundOrderDto dto) =>
        Ok(await _svc.CreateOutboundOrderAsync(dto));

    [HttpPost("outbound/{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmOutbound(Guid id) =>
        await _svc.ConfirmOutboundOrderAsync(id) ? Ok() : NotFound();

    [HttpPost("outbound/{id:guid}/start-picking")]
    public async Task<IActionResult> StartPicking(Guid id) =>
        await _svc.StartPickingOutboundAsync(id) ? Ok() : NotFound();

    [HttpPost("outbound/{id:guid}/pack")]
    public async Task<IActionResult> PackOutbound(Guid id) =>
        await _svc.PackOutboundOrderAsync(id) ? Ok() : NotFound();

    [HttpPost("outbound/{id:guid}/ship")]
    [Authorize(Policy = PermissionKeys.WarehouseOperate)]
    public async Task<IActionResult> ShipOutbound(Guid id, [FromBody] ShipOutboundOrderDto dto)
    {
        try { return await _svc.ShipOutboundOrderAsync(id, dto) ? Ok() : NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("outbound/{id:guid}/deliver")]
    public async Task<IActionResult> DeliverOutbound(Guid id) =>
        await _svc.DeliverOutboundOrderAsync(id) ? Ok() : NotFound();

    [HttpPost("outbound/{id:guid}/cancel")]
    public async Task<IActionResult> CancelOutbound(Guid id) =>
        await _svc.CancelOutboundOrderAsync(id) ? Ok() : NotFound();

    // ── Transfer Orders ──────────────────────────────────────────────────────

    [HttpGet("transfer")]
    public async Task<IActionResult> GetTransferOrders([FromQuery] Guid organizationId) =>
        Ok(await _svc.GetTransferOrdersAsync(organizationId));

    [HttpGet("transfer/{id:guid}")]
    public async Task<IActionResult> GetTransferOrder(Guid id)
    {
        var result = await _svc.GetTransferOrderAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> CreateTransferOrder([FromBody] CreateTransferOrderDto dto) =>
        Ok(await _svc.CreateTransferOrderAsync(dto));

    [HttpPost("transfer/{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmTransfer(Guid id) =>
        await _svc.ConfirmTransferOrderAsync(id) ? Ok() : NotFound();

    [HttpPost("transfer/{id:guid}/ship")]
    [Authorize(Policy = PermissionKeys.WarehouseOperate)]
    public async Task<IActionResult> ShipTransfer(Guid id, [FromBody] DateTime shippedDate)
    {
        try { return await _svc.ShipTransferOrderAsync(id, shippedDate) ? Ok() : NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("transfer/{id:guid}/start-receiving")]
    public async Task<IActionResult> StartReceivingTransfer(Guid id) =>
        await _svc.StartReceivingTransferAsync(id) ? Ok() : NotFound();

    [HttpPost("transfer/{id:guid}/complete")]
    [Authorize(Policy = PermissionKeys.WarehouseOperate)]
    public async Task<IActionResult> CompleteTransfer(Guid id)
    {
        try { return await _svc.CompleteTransferOrderAsync(id) ? Ok() : NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("transfer/{id:guid}/cancel")]
    public async Task<IActionResult> CancelTransfer(Guid id) =>
        await _svc.CancelTransferOrderAsync(id) ? Ok() : NotFound();
}
