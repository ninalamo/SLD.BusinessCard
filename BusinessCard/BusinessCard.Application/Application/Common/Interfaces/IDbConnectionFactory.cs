using Microsoft.Data.SqlClient;

namespace BusinessCard.API.Application.Common.Interfaces;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}