using FluentValidation;

namespace BusinessCard.Application.Application.Commands.AddClient;

public class AddClientCommandValidator : AbstractValidator<AddClientCommand>
{
    public AddClientCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Industry).NotEmpty();
    }
}