namespace BusinessCard.API.Application.Queries.GetMembers;

public record GetMembersQueryResult
{
    public int PageSize { get; init; } = 10;
    public int PageNumber { get; init; } = 1;
    public int TotalCount { get; init; }
    public IEnumerable<MembersResult> Members { get; init; }
}