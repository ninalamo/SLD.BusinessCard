using Microsoft.Data.SqlClient;

namespace BusinessCard.Application.Application.Common.Interfaces;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}