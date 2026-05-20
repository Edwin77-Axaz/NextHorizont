using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace NextHorizont.Infraestructura.Persistence;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new System.ArgumentNullException(nameof(configuration), "DefaultConnection string is missing");
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
