using FluentValidation;

namespace BusinessCard.Application.Application.Queries.GetClientSubscriptions;

public class GetClientSubscriptionsQueryValidator : AbstractValidator<GetClientSubscriptionsQuery>
{
    public GetClientSubscriptionsQueryValidator()
    {
        RuleFor(i => i.ClientId).NotEmpty();
    }
}