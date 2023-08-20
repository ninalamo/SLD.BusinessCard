using FluentValidation;

namespace BusinessCard.API.Application.Commands.UpsertClient;

public class AddClientCommandValidator : AbstractValidator<AddClientCommand>
{
    public AddClientCommandValidator()
    {
        RuleFor(c => c.CompanyName).NotEmpty();
        RuleFor(c => c.MemberTierLevel)
            .NotEmpty()
            .NotEqual(0)
            .Must(x => x > 0 && x <= 7)
            .WithMessage("Invalid Member Tier Level");
        RuleFor(c => c.IsDiscreet).NotEmpty();
    }
}