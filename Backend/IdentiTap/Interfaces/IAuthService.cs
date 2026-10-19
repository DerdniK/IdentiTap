using IdentTap.Application.DTOs;

namespace IdentTap.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto?> LoginWithMicrosoftAsync(MicrosoftLoginRequest request);
    Task<AuthResultDto?> LoginStandardAsync(StandardLoginRequest request);
}