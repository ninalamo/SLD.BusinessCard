using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;

namespace BusinessCard.Application.Application.Queries.GetMemberByUid;

public class GetMemberByUidQuery : IRequest<GetMemberByUidQueryResult>
{
    public GetMemberByUidQuery(string uid)
    {
        Uid = uid;
    }
    public string Uid { get; private set; }
}

public class GetMemberByUidQueryValidator : AbstractValidator<GetMemberByUidQuery>
{
    public GetMemberByUidQueryValidator()
    {
        RuleFor(i => i.Uid).NotEmpty();
    }
}