using IdentTap.Application.Interfaces;
using IdentTap.Domain.Interfaces;
using IdentTap.Infrastructure.Persistence.Repositories;
using IdentTap.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace IdentTap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMicrosoftTokenValidator, MicrosoftTokenValidator>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
}