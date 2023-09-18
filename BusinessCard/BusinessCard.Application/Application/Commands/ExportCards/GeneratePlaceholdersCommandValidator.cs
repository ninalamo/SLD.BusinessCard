using FluentValidation;

namespace BusinessCard.Application.Application.Commands.ExportCards;

public class GeneratePlaceholdersCommandValidator : AbstractValidator<GeneratePlaceholdersCommand>
{
    public GeneratePlaceholdersCommandValidator()
    {
        RuleFor(i => i.ClientId).NotEmpty();
        RuleFor(i => i.SubscriptionId).NotEmpty();

        RuleFor(i => i.Count).Must(x => x <= 1000 && x > 0).WithMessage("Must be between 0 and 1000 only.");
    }
}