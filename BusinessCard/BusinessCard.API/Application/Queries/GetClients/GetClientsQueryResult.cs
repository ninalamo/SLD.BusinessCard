using System.Text.Json.Serialization;

namespace BusinessCard.API.Application.Queries.GetClients;

public record GetClientsQueryResult
{
    public GetClientsQueryResult(int pageSize, int pageNumber, int totalCount, IEnumerable<ClientsResult> clients)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        Clients = clients;
    }
    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }
    public int TotalCount { get; private set; }
    public IEnumerable<ClientsResult> Clients { get; private set; }
}

public record ClientsResult
{
    public Guid ClientId { get; init; }
    public string CompanyName {get;  init; }
    public bool IsDiscreet { get; init; }
    public int? SubscriptionLevel { get;init; }
    public string? Subscription { get; init; }
    public int CardHolders { get; init; }
    public int NonCardHolders { get; init; }
    public string? CreatedBy { get; init; }
    public string? ModifiedBy { get; init; }
    public DateTimeOffset? CreatedOn { get; init; }
    public DateTimeOffset? ModifiedOn { get; init; }
    public bool IsActive { get; init; }
}
