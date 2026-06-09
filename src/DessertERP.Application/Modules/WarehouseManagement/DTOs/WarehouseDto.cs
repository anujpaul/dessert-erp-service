namespace DessertERP.Application.Modules.WarehouseManagement.DTOs;

public record WarehouseDto(
    Guid    Id,
    Guid    OrganizationId,
    string  Code,
    string  Name,
    string? Address,
    string? City,
    string? Country,
    bool    IsActive,
    bool    IsDefault,
    int     LocationCount
);

public record CreateWarehouseDto(
    string  Code,
    string  Name,
    string? Address,
    string? City,
    string? Country,
    bool    IsDefault = false
);

public record UpdateWarehouseDto(
    string  Name,
    string? Address,
    string? City,
    string? Country
);

// ───── Location ─────

public record WarehouseLocationDto(
    Guid    Id,
    Guid    WarehouseId,
    string  Code,
    string? Zone,
    string? Aisle,
    string? Bay,
    string? Level,
    string? Bin,
    bool    IsActive,
    bool    IsPickable,
    bool    IsReceivable
);

public record CreateWarehouseLocationDto(
    Guid    WarehouseId,
    string  Code,
    string? Zone,
    string? Aisle,
    string? Bay,
    string? Level,
    string? Bin,
    bool    IsPickable   = true,
    bool    IsReceivable = true
);
