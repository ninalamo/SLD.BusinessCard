using FluentValidation;

namespace BusinessCard.Application.Application.Commands.EditClient;

public class EditClientCommandValidator : AbstractValidator<EditClientCommand>
{
    public EditClientCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().NotEqual(Guid.Empty).WithMessage("Id must not be null or Guid.Empty");
        RuleFor(c => c.CompanyName).NotEmpty();
        RuleFor(c => c.MemberTierLevel).NotEmpty().NotEqual(0).Must(x => x > 0)
            .WithMessage("Invalid Member Tier Level");
        RuleFor(c => c.IsDiscreet).NotEmpty();
    }
}