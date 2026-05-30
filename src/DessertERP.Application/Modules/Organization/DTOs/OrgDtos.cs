namespace DessertERP.Application.Modules.Organization.DTOs;

public record OrganizationDto(
    Guid Id,
    string Code,
    string Name,
    string BaseCurrency,
    int FiscalYearStartMonth,
    string? Address,
    string? Phone,
    string? Email,
    string? TaxId,
    string? LogoUrl,
    string Status,
    DateTime CreatedAt
);

public record CreateOrganizationRequest(
    string Code,
    string Name,
    string BaseCurrency = "USD",
    int FiscalYearStartMonth = 1,
    string? Address = null,
    string? Phone = null,
    string? Email = null,
    string? TaxId = null
);

public record UpdateOrganizationRequest(
    string Name,
    string? Address,
    string? Phone,
    string? Email,
    string? TaxId,
    string? LogoUrl
);
