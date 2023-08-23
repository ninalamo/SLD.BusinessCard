using BusinessCard.API.Application.Common.Interfaces;
using MediatR;

namespace BusinessCard.API.Application.Queries.GetMembers;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, GetMembersQueryResult>
{
    private readonly IClientQueries _queries;
    private readonly ILogger<GetMembersQueryHandler> _logger;

    public GetMembersQueryHandler(IClientQueries queries, ILogger<GetMembersQueryHandler> logger)
    {
        _queries = queries;
        _logger = logger;
    }

    public async Task<GetMembersQueryResult> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(GetMembersQueryResult)} {DateTimeOffset.Now}");
	    
        var (count, result) = await _queries.GetClientsWithPagination(request.PageSize, request.PageNumber, request.Name);

        return null;// new GetMembersQueryResult(request.PageSize, request.PageNumber, count, result);
    }
}