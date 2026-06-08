namespace DessertERP.Application.Modules.WarehouseManagement.DTOs;

public record OutboundOrderDto(
    Guid    Id,
    Guid    OrganizationId,
    Guid    WarehouseId,
    string  WarehouseName,
    string  OrderNumber,
    Guid?   SalesOrderId,
    Guid?   CustomerId,
    string? CustomerName,
    string? ShipToAddress,
    DateTime  RequestedDate,
    DateTime? ShippedDate,
    string?   TrackingNumber,
    string?   Carrier,
    string  Status,
    string? Notes,
    List<OutboundOrderLineDto> Lines
);

public record OutboundOrderLineDto(
    Guid    Id,
    int     LineNumber,
    Guid    ProductId,
    string  ProductName,
    string? ProductSku,
    Guid?   FromLocationId,
    string? FromLocationCode,
    decimal RequestedQuantity,
    decimal PickedQuantity,
    decimal ShippedQuantity,
    string  UnitOfMeasure,
    string? LotNumber,
    string? Notes
);

public record CreateOutboundOrderDto(
    Guid    OrganizationId,
    Guid    WarehouseId,
    string  OrderNumber,
    DateTime RequestedDate,
    Guid?   SalesOrderId,
    Guid?   CustomerId,
    string? CustomerName,
    string? ShipToAddress,
    string? Notes,
    List<CreateOutboundOrderLineDto> Lines
);

public record CreateOutboundOrderLineDto(
    Guid    ProductId,
    string  ProductName,
    decimal RequestedQuantity,
    string  UnitOfMeasure  = "EA",
    string? ProductSku     = null,
    Guid?   FromLocationId = null,
    string? LotNumber      = null
);

public record ShipOutboundOrderDto(
    DateTime ShippedDate,
    string?  TrackingNumber,
    string?  Carrier
);
