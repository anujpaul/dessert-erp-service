using DessertERP.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DessertERP.Infrastructure.Services;

/// <summary>
/// Reads the active Organization from the X-Organization-Id HTTP request header.
/// Registered as Scoped so it picks up the current HttpContext per request.
/// </summary>
public class CurrentOrganizationService : ICurrentOrganizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentOrganizationService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public Guid OrganizationId
    {
        get
        {
            var header = _httpContextAccessor.HttpContext?.Request.Headers["X-Organization-Id"]
                .FirstOrDefault();

            return Guid.TryParse(header, out var id) ? id : Guid.Empty;
        }
    }
}
