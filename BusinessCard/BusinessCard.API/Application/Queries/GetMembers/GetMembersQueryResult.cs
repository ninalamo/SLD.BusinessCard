namespace BusinessCard.API.Application.Queries.GetMembers;

public record GetMembersQueryResult
{
    public GetMembersQueryResult(int pageSize, int pageNumber, int totalCount, IEnumerable<MembersResult> members)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        Members = members;
    }
    public int PageSize { get; init; } = 10;
    public int PageNumber { get; init; } = 1;
    public int TotalCount { get; init; }
    public IEnumerable<MembersResult> Members { get; init; }
}