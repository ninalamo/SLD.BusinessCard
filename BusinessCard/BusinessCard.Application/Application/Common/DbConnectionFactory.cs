using BusinessCard.Application.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;

namespace BusinessCard.Application.Application.Common;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}