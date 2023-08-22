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
	    _logger.LogInformation($"Starting {nameof(GetClientsQueryHandler)} {DateTimeOffset.Now}");
	    
	    var (count, result) = await _queries.GetClientsWithPagination(request.PageSize, request.PageNumber, request.Name);
	    
        return new GetClientsQueryResult(request.PageSize, request.PageNumber, count, result);
    }
}
