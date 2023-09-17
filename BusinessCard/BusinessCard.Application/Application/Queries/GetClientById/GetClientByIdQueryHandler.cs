using BusinessCard.Application.Application.Common.Interfaces;
using BusinessCard.Application.Application.Queries.GetClients;

namespace BusinessCard.Application.Application.Queries.GetClientById;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, IEnumerable<ClientsResult>>
{
    private readonly IClientQueries _queries;
    private readonly ILogger<GetClientByIdQueryHandler> _logger;

    public GetClientByIdQueryHandler(IClientQueries queries, ILogger<GetClientByIdQueryHandler> logger)
    {
        _queries = queries;
        _logger = logger;
    }


    public async Task<IEnumerable<ClientsResult>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {ClientByIdQueryHandlerName} {Now}", nameof(GetClientByIdQueryHandler), DateTimeOffset.Now);
	    
        var result = await _queries.GetClientById(request.Id);

        return result;
    }
}