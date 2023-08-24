using System.Runtime.InteropServices.JavaScript;
using BusinessCard.API.Application.Common.Interfaces;
using BusinessCard.API.Application.Common.SQLScripts;
using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.API.Application.Queries.GetMembers;
using ClientService;
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

    public async Task<ClientsResult> GetClientById(Guid id)
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

    public async Task<(int,IEnumerable<MembersResult>)> GetMembersWithPagination(int pageSize, int pageNumber, Guid clientId)
    {
	    DynamicParameters parameters = new();
	    parameters.Add("ClientId", clientId);
	    parameters.Add("pageNumber", pageNumber);
	    parameters.Add("pageSize", pageSize);
	    parameters.Add("offSet",(pageNumber -1) * pageSize);

	    var countQuery = SqlScript.MemberCount;

	    await using var connection = _dbConnectionFactory.CreateConnection(); //new SqlConnection(_connectionString);
        
	    await connection.OpenAsync(CancellationToken.None);

	    var count = await connection.QueryAsync<int>(countQuery,new{ ClientId = clientId});

	    if (count.FirstOrDefault() == 0) return (0, Array.Empty<MembersResult>());

	    var query = SqlScript.SelectMembers + " WHERE P.[ClientId] = @ClientId ";
	    if (count.FirstOrDefault() == 1)
	    {
		    query += @" ORDER BY P.[LastName], P.[FirstName] ";
	    }
	    else
	    {
		    query += @" ORDER BY P.[LastName], P.[FirstName]  
			OFFSET @offSet 
			ROWS FETCH NEXT @pageSize ROWS ONLY";    
	    }

	    var result = await connection.QueryAsync<MembersResult>(query, parameters);

	    return (count.First(), result ?? Array.Empty<MembersResult>());
    }
}