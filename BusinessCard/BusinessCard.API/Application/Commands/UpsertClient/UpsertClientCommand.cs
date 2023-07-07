using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BusinessCard.API.Application.Commands.UpsertClient;

public class UpsertClientCommand : IRequest<CommandResult>
{
    public UpsertClientCommand(Guid? id, string companyName, bool isDiscreet, Tier subscription)
    {
        Id = id;
        CompanyName = companyName;
        IsDiscreet = isDiscreet;
        Subscription = subscription;
    }
    public Guid? Id { get; private set; }
    public string CompanyName { get; private set; }
    public Tier Subscription { get; private set; }
    public bool IsDiscreet { get; private set; }
}

public class UpsertClientCommandValidation : AbstractValidator<UpsertClientCommand>
{
    public UpsertClientCommandValidation()
    {
        RuleFor(c => c.CompanyName).NotEmpty();
        RuleFor(c => c.Subscription).NotEmpty();
        RuleFor(c => c.IsDiscreet).NotEmpty();
    }
}

public class UpsertClientCommandHandler : IRequestHandler<UpsertClientCommand, CommandResult>
{
    private readonly IClientsRepository _repository;
    public async Task<CommandResult> Handle(UpsertClientCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Empty;
        if (request.Id.HasValue)
        {
            //update
            var updated = await _repository.GetEntityByIdAsync(request.Id.Value);
            updated.UpdateSelf(request.CompanyName,request.IsDiscreet,request.Subscription);
            id = _repository.Update(updated).Id;
        }
        else
        {
            //create
            id = _repository.Create(request.CompanyName, request.IsDiscreet, request.Subscription).Id;
        }

        _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return CommandResult.Success(id);
    }
}