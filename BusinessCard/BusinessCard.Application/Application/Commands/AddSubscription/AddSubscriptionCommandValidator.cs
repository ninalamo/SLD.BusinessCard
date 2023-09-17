using FluentValidation;

namespace BusinessCard.Application.Application.Commands.AddSubscription;

public class AddSubscriptionCommandValidator : AbstractValidator<AddSubscriptionCommand>
{
    public AddSubscriptionCommandValidator()
    {
        RuleFor(i => i.ClientId).NotEmpty();
        RuleFor(i => i.PlanId).NotEmpty();
        RuleFor(i => i.StartDate).NotEmpty().GreaterThanOrEqualTo(DateTimeOffset.Now);
        RuleFor(i => i.NumberOfMonthToExpire).NotEmpty().GreaterThanOrEqualTo(1);
    }
}