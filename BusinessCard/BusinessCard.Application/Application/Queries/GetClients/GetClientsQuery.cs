namespace BusinessCard.Application.Application.Queries.GetClients;

public class GetClientsQuery : IRequest<GetClientsQueryResult>
{
    public GetClientsQuery(int pageSize, int pageNumber, string name)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Name = name;
    }

    public int PageSize { get; private set; } = 10;
    public int PageNumber { get; private set; } = 1;
    public string? Name { get; private set; }
}

