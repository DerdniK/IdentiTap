using IdentTap.Application.DTOs;
using IdentTap.Application.Interfaces;

namespace IdentTap.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", async (StandardLoginRequest req, IAuthService auth) =>
        {
            var result = await auth.LoginStandardAsync(req);
            return result is not null ? Results.Ok(result) : Results.Unauthorized();
        });

        group.MapPost("/login-microsoft", async (MicrosoftLoginRequest req, IAuthService auth) =>
        {
            if (string.IsNullOrWhiteSpace(req.IdToken))
                return Results.BadRequest(new { message = "IdToken es requerido." });

            var result = await auth.LoginWithMicrosoftAsync(req);
            return result is not null ? Results.Ok(result) : Results.Unauthorized();
        });
    }
}