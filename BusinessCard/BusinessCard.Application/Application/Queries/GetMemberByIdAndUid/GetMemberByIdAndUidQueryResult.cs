namespace BusinessCard.Application.Application.Queries.GetMemberByIdAndUid;

public record GetMemberByIdAndUidQueryResult
{
    public IEnumerable<MemberIdAndUidResult> Member { get; init; }

}

public record MemberIdAndUidResult
{
    public Guid MemberId { get; init; }
    public Guid ClientId { get; init; }
    public Guid SubscriptionId { get; init;}
    public int CardLevel { get; init;}
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public string NameSuffix { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string Address { get; init; }
    public string Occupation { get; init; }
    public string Facebook { get; init; }
    public string LinkedIn { get; init; }
    public string Instagram { get; init; }
    public string Pinterest { get; init; }
    public string Twitter { get; init; }
    public string CardKey { get; init; }
    public bool IsActive { get; init; }
    public string IdentityUserId { get; init; }
    public string Company { get; init; }
}