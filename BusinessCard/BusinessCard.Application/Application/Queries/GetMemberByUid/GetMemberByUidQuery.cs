using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;

namespace BusinessCard.Application.Application.Queries.GetMemberByUid;

public class GetMemberByUidQuery : IRequest<GetMemberByUidQueryResult>
{
    public GetMemberByUidQuery(Guid clientId, string uid)
    {
        ClientId = clientId;
        Uid = uid;
    }

    public Guid ClientId { get; private set; }
    public string Uid { get; private set; }
}

public class GetMemberByUidQueryValidator : AbstractValidator<GetMemberByUidQuery>
{
    public GetMemberByUidQueryValidator()
    {
        RuleFor(i => i.Uid).NotEmpty();
        RuleFor(i => i.ClientId).NotEmpty();
    }
}