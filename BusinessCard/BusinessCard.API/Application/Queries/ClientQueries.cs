using BusinessCard.API.Application.Common.Interfaces;
using BusinessCard.API.Application.Common.SQLScripts;
using BusinessCard.API.Application.Queries.GetClients;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BusinessCard.API.Application.Queries;

public class ClientQueries : IClientQueries
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ClientQueries(IDbConnectionFactory factory)
    {
	    _dbConnectionFactory = factory;
    }
    
    public async Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("pageNumber", pageNumber);
        parameters.Add("pageSize", pageSize);
        parameters.Add("offSet",(pageNumber -1) * pageSize);


        var query = SqlScript.SelectClients;

        if (!string.IsNullOrEmpty(name))
        {
            parameters.Add("CompanyName",name);
            query += " WHERE C.[CompanyName] LIKE '@CompanyName%' ";
        }
        
        query += @" ORDER BY C.[CompanyName] 
        OFFSET @offSet 
        ROWS FETCH NEXT @pageSize ROWS ONLY";

        var countQuery = SqlScript.ClientCount;

        await using var connection = _dbConnectionFactory.CreateConnection(); //new SqlConnection(_connectionString);
        
        await connection.OpenAsync(CancellationToken.None);

        var count = await connection.QueryAsync<int>(countQuery);

        var result = await connection.QueryAsync<ClientsResult>(query, parameters);

        return (count.First(), result);
    }

    public async Task<ClientsResult> GetClientbyId(Guid id)
    {
	    DynamicParameters parameters = new();
	    parameters.Add("Id", id);

	    string query = SqlScript.SelectClientById;

	    await using SqlConnection connection =_dbConnectionFactory.CreateConnection(); // new SqlConnection(_connectionString);
        
	    await connection.OpenAsync(CancellationToken.None);

	    var result = await connection.QueryAsync<ClientsResult>(query, parameters);

	    var clientsResults = result as ClientsResult[] ?? result.ToArray();
	    
	    if (!clientsResults.Any()) throw new KeyNotFoundException("Id not found.");

	    return clientsResults.FirstOrDefault();
    }
}