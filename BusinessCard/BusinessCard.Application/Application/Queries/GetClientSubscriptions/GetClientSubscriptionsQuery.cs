namespace BusinessCard.Application.Application.Queries.GetClientSubscriptions;

public class GetClientSubscriptionsQuery : IRequest<GetClientSubscriptionQueryResult>
{
    public GetClientSubscriptionsQuery(Guid clientId)
    {
        ClientId = clientId;
    }
    public Guid ClientId { get; }
}