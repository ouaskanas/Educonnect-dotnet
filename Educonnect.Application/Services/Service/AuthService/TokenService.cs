using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Educonnect.Application.Services.IService.IAuthService;
using Educonnect.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Educonnect.Application.Services.Service.AuthService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Spn, user.SecurityStamp.ToString()),
            new Claim(ClaimTypes.Name,           user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email,          user.Email    ?? string.Empty),
            new Claim(ClaimTypes.GivenName,      user.Name),
            new Claim(ClaimTypes.Role,           user.Role.ToString()),
        };

        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiryMinutes = _config.GetValue<int>("Jwt:AccessTokenExpiryMinutes", 15);

        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    // Opaque cryptographic token — stored in DB, never decoded
    public string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
