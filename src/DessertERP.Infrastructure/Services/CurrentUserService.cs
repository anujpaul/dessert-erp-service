using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DessertERP.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DessertERP.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string Username =>
        User?.FindFirstValue("username")
        ?? User?.Identity?.Name
        ?? User?.FindFirstValue(ClaimTypes.Email)
        ?? "System";

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
}
