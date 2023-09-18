using BusinessCard.Application.Application.Common.Interfaces;

namespace BusinessCard.Application.Application.Queries.GetClientSubscriptions;

public class GetClientSubscriptionsQueryHandler : IRequestHandler<GetClientSubscriptionsQuery,GetClientSubscriptionQueryResult>
{
    private readonly ISubscriptionQueries _subscriptionQueries;
    private readonly ILogger<GetClientSubscriptionsQueryHandler> _logger;

    public GetClientSubscriptionsQueryHandler(ISubscriptionQueries subscriptionQueries, ILogger<GetClientSubscriptionsQueryHandler> logger)
    {
        _subscriptionQueries = subscriptionQueries;
        _logger = logger;
    }
    
    
    public async Task<GetClientSubscriptionQueryResult> Handle(GetClientSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(GetClientSubscriptionsQueryHandler)} {DateTimeOffset.Now}");

        var result = await _subscriptionQueries.GetClientSubscriptions(request.ClientId);

        return new GetClientSubscriptionQueryResult(result);
    }
}