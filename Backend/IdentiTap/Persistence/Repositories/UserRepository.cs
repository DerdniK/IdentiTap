using System.Data;
using Dapper;
using IdentTap.Domain.Entities;
using IdentTap.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IdentTap.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("AzureSqlDatabase")
            ?? throw new InvalidOperationException("Connection string 'AzureSqlDatabase' no encontrada.");
    }

    public async Task<User?> LoginWithSpAsync(string email, string? password, string? microsoftId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email, DbType.AnsiString);
            parameters.Add("@Password", password, DbType.AnsiString);
            parameters.Add("@MicrosoftID", microsoftId, DbType.AnsiString);

            return await connection.QueryFirstOrDefaultAsync<User>(
                "sp_LoginUser",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number >= 50000)
        {
            // Errores lanzados por THROW en el procedimiento almacenado
            return null;
        }
    }
}