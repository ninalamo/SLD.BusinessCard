namespace BusinessCard.Application.Application.Queries.GetClients;

public record GetClientsQueryResult(int PageSize, int PageNumber, int TotalCount, IEnumerable<ClientsResult> Clients)
{
    public int PageSize { get; private set; } = PageSize;
    public int PageNumber { get; private set; } = PageNumber;
    public int TotalCount { get; private set; } = TotalCount;
    public IEnumerable<ClientsResult> Clients { get; private set; } = Clients;
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
