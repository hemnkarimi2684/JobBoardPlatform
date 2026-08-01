using Microsoft.Data.SqlClient;
using System.Data;

namespace JobBoardPlatform.Infrastructure.Dapper.Connection;

public class DbConnectionFactory :  IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection() => new SqlConnection(_connectionString);
}
