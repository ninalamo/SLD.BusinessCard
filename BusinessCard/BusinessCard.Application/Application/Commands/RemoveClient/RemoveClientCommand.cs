using FluentValidation;

namespace BusinessCard.API.Application.Commands.RemoveClient;

public class RemoveClientCommand : IRequest
{
    public RemoveClientCommand(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}

public class RemoveClientCommandValidator : AbstractValidator<RemoveClientCommand>
{
    public RemoveClientCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}