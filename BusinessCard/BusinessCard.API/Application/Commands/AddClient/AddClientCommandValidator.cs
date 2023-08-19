using FluentValidation;

namespace BusinessCard.API.Application.Commands.UpsertClient;

public class AddClientCommandValidator : AbstractValidator<AddClientCommand>
{
    public AddClientCommandValidator()
    {
        RuleFor(c => c.CompanyName).NotEmpty();
        RuleFor(c => c.MemberTierLevel).NotEmpty();
        RuleFor(c => c.IsDiscreet).NotEmpty();
    }
}