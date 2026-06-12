namespace DessertERP.Application.Modules.WarehouseManagement.DTOs;

public record WarehouseDto(
    Guid    Id,
    Guid    OrganizationId,
    string  Code,
    string  Name,
    string? Address,
    string? City,
    string? Country,
    Guid?   WarehouseTypeId,
    string? WarehouseTypeName,
    Guid?   SiteId,
    string? SiteName,
    string? SiteCapabilities,
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
    bool    IsDefault = false,
    Guid?   WarehouseTypeId = null,
    Guid?   SiteId = null
);

public record UpdateWarehouseDto(
    string  Name,
    string? Address,
    string? City,
    string? Country,
    Guid?   WarehouseTypeId = null,
    Guid?   SiteId = null
);

public record WarehouseTypeDto(Guid Id, string Name, string? Description, bool IsActive);
public record CreateWarehouseTypeDto(string Name, string? Description = null);

public record OperationalSiteDto(
    Guid Id, string Code, string Name, string? Address, string? City, string? Country,
    bool IsRetailStore, bool IsFulfillmentCenter, bool IsReturnCenter,
    bool IsWarehouse, bool IsActive, string Capabilities);

public record CreateOperationalSiteDto(
    string Code, string Name, string? Address, string? City, string? Country,
    bool IsRetailStore = false, bool IsFulfillmentCenter = false,
    bool IsReturnCenter = false, bool IsWarehouse = true);

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
