using BusinessCard.API.Application.Common.Interfaces;
using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.Application.Application.Common.Interfaces;

namespace BusinessCard.API.Application.Queries.GetClientById;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientsResult>
{
    private readonly IClientQueries _queries;
    private readonly ILogger<GetClientByIdQueryHandler> _logger;

    public GetClientByIdQueryHandler(IClientQueries queries, ILogger<GetClientByIdQueryHandler> logger)
    {
        _queries = queries;
        _logger = logger;
    }


    public async Task<ClientsResult> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {ClientByIdQueryHandlerName} {Now}", nameof(GetClientByIdQueryHandler), DateTimeOffset.Now);
	    
        var result = await _queries.GetClientById(request.Id);

        return result;
    }
}