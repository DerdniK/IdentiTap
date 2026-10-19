using IdentTap.Application.DTOs;

namespace IdentTap.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(UserSessionDto user);
}
