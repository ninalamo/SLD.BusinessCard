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
    public Guid? Id { get; init; }
    public string? Name {get;  init; }
    public string? Industry { get; init; }
    public int Subscriptions { get;init; }
    public bool IsActive { get; init; }
}
