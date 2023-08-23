using BusinessCard.API.Application.Queries.GetClients;
using FluentValidation;
using MediatR;

namespace BusinessCard.API.Application.Queries.GetClientById;

public class GetClientByIdQuery : IRequest<ClientsResult>
{
    public GetClientByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}

public class GetClientByIdQueryValidator : AbstractValidator<GetClientByIdQuery>
{
    public GetClientByIdQueryValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}

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
	    
        var result = await _queries.GetClientbyId(request.Id);

        return result;
    }
}