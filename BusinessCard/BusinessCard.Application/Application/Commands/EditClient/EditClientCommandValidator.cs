using FluentValidation;

namespace BusinessCard.Application.Application.Commands.EditClient;

public class EditClientCommandValidator : AbstractValidator<EditClientCommand>
{
    public EditClientCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Industry).NotEmpty().MaximumLength(50);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(50);
    }
}