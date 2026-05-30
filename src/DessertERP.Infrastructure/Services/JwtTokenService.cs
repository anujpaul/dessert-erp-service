using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DessertERP.Application.Common.Interfaces;
using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DessertERP.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenMinutes;
    private readonly int _refreshTokenDays;

    public JwtTokenService(IConfiguration configuration)
    {
        var jwt = configuration.GetSection("JwtSettings");
        _secret              = jwt["Secret"]             ?? throw new InvalidOperationException("JwtSettings:Secret missing.");
        _issuer              = jwt["Issuer"]             ?? "DessertERP";
        _audience            = jwt["Audience"]           ?? "DessertERP";
        _accessTokenMinutes  = int.TryParse(jwt["AccessTokenMinutes"],  out var a) ? a : 60;
        _refreshTokenDays    = int.TryParse(jwt["RefreshTokenDays"],    out var r) ? r : 7;
    }

    public DateTime AccessTokenExpiry  => DateTime.UtcNow.AddMinutes(_accessTokenMinutes);
    public DateTime RefreshTokenExpiry => DateTime.UtcNow.AddDays(_refreshTokenDays);

    public string GenerateAccessToken(AppUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("username",                    user.Username),
            new("orgId",                       user.OrganizationId.ToString()),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        };

        claims.AddRange(roles.Select(r       => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission",    p)));

        var token = new JwtSecurityToken(
            issuer:             _issuer,
            audience:           _audience,
            claims:             claims,
            notBefore:          DateTime.UtcNow,
            expires:            AccessTokenExpiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
