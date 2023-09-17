using BusinessCard.Application.Application.Common.Interfaces;
using BusinessCard.Application.Application.Common.SQLScripts;
using BusinessCard.Application.Application.Queries.GetClients;
using BusinessCard.Application.Application.Queries.GetClientSubscriptions;
using Dapper;

namespace BusinessCard.Application.Application.Queries;



public class SubscriptionQueries : ISubscriptionQueries
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SubscriptionQueries(IDbConnectionFactory factory)
    {
        _dbConnectionFactory = factory;
    }

    public async Task<IEnumerable<ClientSubscriptionResult>> GetClientSubscriptions(Guid clientId)
    {
        var query = SubscriptionSQL.SelectSubscriptionsByClientId;

        var parameters = new DynamicParameters();
        parameters.Add("ClientId", clientId);

        await using var connection = _dbConnectionFactory.CreateConnection(); 
        await connection.OpenAsync(CancellationToken.None);
        var result = await connection.QueryAsync<ClientSubscriptionResult>(query, parameters);

        return result;
    }
}