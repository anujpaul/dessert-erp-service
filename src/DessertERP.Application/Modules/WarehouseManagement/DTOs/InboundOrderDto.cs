namespace DessertERP.Application.Modules.WarehouseManagement.DTOs;

public record InboundOrderDto(
    Guid    Id,
    Guid    OrganizationId,
    Guid    WarehouseId,
    string  WarehouseName,
    string  OrderNumber,
    Guid?   PurchaseOrderId,
    Guid?   VendorId,
    string? VendorName,
    DateTime  ExpectedDate,
    DateTime? ReceivedDate,
    string  Status,
    string? Notes,
    List<InboundOrderLineDto> Lines
);

public record InboundOrderLineDto(
    Guid     Id,
    int      LineNumber,
    Guid     ProductId,
    string   ProductName,
    string?  ProductSku,
    Guid?    LocationId,
    string?  LocationCode,
    decimal  OrderedQuantity,
    decimal  ReceivedQuantity,
    string   UnitOfMeasure,
    string?  LotNumber,
    DateTime? ExpiryDate,
    string?  Notes
);

public record CreateInboundOrderDto(
    Guid    OrganizationId,
    Guid    WarehouseId,
    string  OrderNumber,
    DateTime ExpectedDate,
    Guid?   PurchaseOrderId,
    Guid?   VendorId,
    string? VendorName,
    string? Notes,
    List<CreateInboundOrderLineDto> Lines
);

public record CreateInboundOrderLineDto(
    Guid     ProductId,
    string   ProductName,
    decimal  OrderedQuantity,
    string   UnitOfMeasure = "EA",
    string?  ProductSku    = null,
    Guid?    LocationId    = null,
    string?  LotNumber     = null,
    DateTime? ExpiryDate   = null
);

public record ReceiveInboundLineDto(
    Guid    LineId,
    decimal Quantity,
    Guid?   LocationId = null
);
