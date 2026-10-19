using IdentTap.Domain.Entities;

namespace IdentTap.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> LoginWithSpAsync(string email, string? password, string? microsoftId);
}