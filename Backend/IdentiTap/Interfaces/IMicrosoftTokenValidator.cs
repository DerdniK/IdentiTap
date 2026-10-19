namespace IdentTap.Domain.Interfaces;

public record MicrosoftIdentityClaims(string MicrosoftId, string Email);

public interface IMicrosoftTokenValidator
{
    Task<MicrosoftIdentityClaims?> ValidateAsync(string idToken);
}