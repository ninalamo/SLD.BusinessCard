using BusinessCard.Application.Application.Queries.GetClientSubscriptions;

namespace BusinessCard.Application.Application.Common.Interfaces;

public interface ISubscriptionQueries
{
    Task<IEnumerable<ClientSubscriptionResult>> GetClientSubscriptions(Guid clientId);
}