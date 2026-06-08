namespace DessertERP.Application.Modules.WarehouseManagement.DTOs;

public record TransferOrderDto(
    Guid    Id,
    Guid    OrganizationId,
    string  OrderNumber,
    Guid    FromWarehouseId,
    string  FromWarehouseName,
    Guid    ToWarehouseId,
    string  ToWarehouseName,
    DateTime  RequestedDate,
    DateTime? ShippedDate,
    DateTime? ReceivedDate,
    string  Status,
    string? Notes,
    List<TransferOrderLineDto> Lines
);

public record TransferOrderLineDto(
    Guid    Id,
    int     LineNumber,
    Guid    ProductId,
    string  ProductName,
    string? ProductSku,
    Guid?   FromLocationId,
    string? FromLocationCode,
    Guid?   ToLocationId,
    string? ToLocationCode,
    decimal RequestedQuantity,
    decimal ShippedQuantity,
    decimal ReceivedQuantity,
    string  UnitOfMeasure,
    string? LotNumber,
    string? Notes
);

public record CreateTransferOrderDto(
    Guid    OrganizationId,
    string  OrderNumber,
    Guid    FromWarehouseId,
    Guid    ToWarehouseId,
    DateTime RequestedDate,
    string? Notes,
    List<CreateTransferOrderLineDto> Lines
);

public record CreateTransferOrderLineDto(
    Guid    ProductId,
    string  ProductName,
    decimal RequestedQuantity,
    string  UnitOfMeasure  = "EA",
    string? ProductSku     = null,
    Guid?   FromLocationId = null,
    Guid?   ToLocationId   = null,
    string? LotNumber      = null
);
