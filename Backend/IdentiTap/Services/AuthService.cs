using IdentTap.Application.DTOs;
using IdentTap.Application.Interfaces;
using IdentTap.Domain.Interfaces;

namespace IdentTap.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMicrosoftTokenValidator _microsoftValidator;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(
        IUserRepository userRepository, 
        IMicrosoftTokenValidator microsoftValidator,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _microsoftValidator = microsoftValidator;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResultDto?> LoginWithMicrosoftAsync(MicrosoftLoginRequest request)
    {
        var claims = await _microsoftValidator.ValidateAsync(request.IdToken);
        if (claims is null) return null;

        var user = await _userRepository.LoginWithSpAsync(claims.Email, password: null, claims.MicrosoftId);
        if (user is null) return null;

        var dto = new UserSessionDto
        {
            UserID = user.UserID,
            Email = user.Email,
            RoleID = user.RoleID,
            RoleName = user.RoleName,
            CreatedAt = user.CreatedAt
        };

        var token = _jwtProvider.GenerateToken(dto);
        return new AuthResultDto(token, dto);
    }

    public async Task<AuthResultDto?> LoginStandardAsync(StandardLoginRequest request)
    {
        var user = await _userRepository.LoginWithSpAsync(request.Email, request.Password, microsoftId: null);
        if (user is null) return null;

        var dto = new UserSessionDto
        {
            UserID = user.UserID,
            Email = user.Email,
            RoleID = user.RoleID,
            RoleName = user.RoleName,
            CreatedAt = user.CreatedAt
        };

        var token = _jwtProvider.GenerateToken(dto);
        return new AuthResultDto(token, dto);
    }
}