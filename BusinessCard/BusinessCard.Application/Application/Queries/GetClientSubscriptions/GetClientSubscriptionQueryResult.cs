namespace BusinessCard.Application.Application.Queries.GetClientSubscriptions;

public record GetClientSubscriptionQueryResult
{
    public GetClientSubscriptionQueryResult(IEnumerable<ClientSubscriptionResult> result)
    {
        Result = result ?? Array.Empty<ClientSubscriptionResult>();
    }
    
    public IEnumerable<ClientSubscriptionResult> Result { get; }
}