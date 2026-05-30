using DessertERP.Domain.Modules.SystemAdmin;

namespace DessertERP.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(AppUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    DateTime AccessTokenExpiry { get; }
    DateTime RefreshTokenExpiry { get; }
}
