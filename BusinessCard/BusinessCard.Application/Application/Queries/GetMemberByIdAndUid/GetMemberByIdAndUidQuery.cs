using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;

namespace BusinessCard.Application.Application.Queries.GetMemberByIdAndUid;

public class GetMemberByIdAndUidQuery : IRequest<GetMemberByIdAndUidQueryResult>
{
    public GetMemberByIdAndUidQuery(Guid clientId, Guid memberId, string uid)
    {
        ClientId = clientId;
        MemberId = memberId;
        Uid = uid;
    }

    public Guid ClientId { get; private set; }
    public Guid MemberId { get; private set; }
    public string Uid { get; private set; }
}

public class GetMemberByIdAndUidQueryValidator : AbstractValidator<GetMemberByIdAndUidQuery>
{
    public GetMemberByIdAndUidQueryValidator()
    {
        RuleFor(i => i.Uid).NotEmpty();
        RuleFor(i => i.ClientId).NotEmpty();
        RuleFor(i => i.MemberId).NotEmpty();
    }
}