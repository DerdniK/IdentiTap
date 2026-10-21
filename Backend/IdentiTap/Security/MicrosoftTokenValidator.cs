using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentTap.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace IdentTap.Infrastructure.Security;

public class MicrosoftTokenValidator : IMicrosoftTokenValidator
{
    private readonly IConfiguration _config;

    public MicrosoftTokenValidator(IConfiguration config)
    {
        _config = config;
    }

    public async Task<MicrosoftIdentityClaims?> ValidateAsync(string idToken)
    {
        var tenantId = _config["AzureAd:TenantId"];
        var clientId = _config["AzureAd:ClientId"];
        var authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";

        var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{authority}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());

        var openIdConfig = await configManager.GetConfigurationAsync();

        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Evita que .NET sobreescriba los nombres nativos de los claims
        tokenHandler.InboundClaimTypeMap.Clear();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateAudience = true,
            ValidAudience = clientId,
            ValidateLifetime = true,
            IssuerSigningKeys = openIdConfig.SigningKeys
        };

        try
        {
            var principal = tokenHandler.ValidateToken(idToken, validationParameters, out _);
            
            // Al limpiar el mapeo, "oid" y "preferred_username" se leen de forma directa
            var microsoftId = principal.FindFirst("oid")?.Value;
            var email = principal.FindFirst("preferred_username")?.Value 
                        ?? principal.FindFirst("email")?.Value;

            Console.WriteLine($"---> [DEBUG ENTRA] MicrosoftID leído: '{microsoftId}'");
            Console.WriteLine($"---> [DEBUG ENTRA] Email leído: '{email}'");

            if (string.IsNullOrEmpty(microsoftId) || string.IsNullOrEmpty(email))
                return null;

            return new MicrosoftIdentityClaims(microsoftId, email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> [DEBUG ENTRA ERROR]: {ex.Message}");
            return null;
        }
    }
}