using BusinessCard.Infrastructure;
using Dapper;
using MediatR;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BusinessCard.API.Application.Queries.GetClients;

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, GetClientsQueryResult>
{
    private readonly IClientQueries _queries;
    private readonly ILogger<GetClientsQueryHandler> _logger;

    public GetClientsQueryHandler(IClientQueries queries, ILogger<GetClientsQueryHandler> logger)
    {
	    _queries = queries;
	    _logger = logger;
    }

    public async Task<GetClientsQueryResult> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {

	    var (count, result) = await _queries.GetClientsWithPagination(request.PageSize, request.PageNumber, request.Name);
	    
        return new GetClientsQueryResult(request.PageSize, request.PageNumber, count, result);
    }
}

public interface IClientQueries
{
	Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name);
}

public class ClientQueries : IClientQueries
{
	private readonly string _connectionString;

	public ClientQueries(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<(int,IEnumerable<ClientsResult>)> GetClientsWithPagination(int pageSize, int pageNumber, string? name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("pageNumber", pageNumber);
        parameters.Add("pageSize", pageSize);
        parameters.Add("offSet",(pageNumber -1) * pageSize);
        

        var query = @"SELECT 
	  		C.[Id] [ClientId] 
      		,C.[CompanyName] 
      		,C.[MemberTierId] [SubscriptionLevel] 
      		,C.[IsDiscreet] 
      		,C.[CreatedBy] 
      		,C.[CreatedOn] 
      		,C.[ModifiedBy] 
      		,C.[ModifiedOn] 
      		,C.[IsActive] 
	  		,M.[Name] [Subscription] 
	  		,CC.[Key] [CardKey] 
	  		,CC.Id [CarId] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 1)  [Cardholders] 
	  		,(SELECT COUNT(*) FROM kardb.kardibee.people WHERE ClientId = C.Id AND C.IsActive = 0)  [NonCardholders] 
  		FROM [kardb].[kardibee].[client] C 
  			LEFT JOIN kardb.kardibee.people P ON P.ClientId = C.Id 
  			LEFT JOIN kardb.kardibee.membertier M ON M.Id = C.MemberTierId 
  			LEFT JOIN kardb.kardibee.card CC ON CC.Id = P.CardId ";

        if (!string.IsNullOrEmpty(name))
        {
            parameters.Add("CompanyName",name);
            query += "WHERE C.[CompanyName] LIKE '@CompanyName%' ";
        }
        
        query += @"ORDER BY C.[CompanyName] 
        OFFSET @offSet 
        ROWS FETCH NEXT @pageSize ROWS ONLY";

        var countQuery = @"SELECT COUNT(*) FROM kardb.kardibee.client";
        
        await using var connection = new SqlConnection(_connectionString);
        
        await connection.OpenAsync(CancellationToken.None);

        var count = await connection.QueryAsync<int>(countQuery);

        var result = await connection.QueryAsync<ClientsResult>(query, parameters);

        return (count.First(), result);
    }
}