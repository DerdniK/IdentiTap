using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentTap.Application.DTOs;
using IdentTap.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentTap.Infrastructure.Security;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _config;

    public JwtProvider(IConfiguration config) => _config = config;

    public string GenerateToken(UserSessionDto user)
    {
        var secret = _config["Jwt:Secret"] ?? "CLAVE_SECRETA_SUPER_SEGURA_DE_AL_MENOS_32_BYTES_123456";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleName)
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = "IdentTapApi",
            Audience = "IdentTapClients",
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }
}