using MediatR;

namespace BusinessCard.API.Application.Queries.GetMembers;

public class GetMembersQuery : IRequest<GetMembersQueryResult>
{
    public GetMembersQuery(int pageSize, int pageNumber, Guid clientId)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        ClientId = clientId;
    }
    
    public int PageSize { get; private set; } = 10;
    public int PageNumber { get; private set; } = 1;
    public Guid ClientId { get; private set; }
}