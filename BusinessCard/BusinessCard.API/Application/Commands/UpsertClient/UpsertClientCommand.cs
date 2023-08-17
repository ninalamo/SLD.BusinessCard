using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BusinessCard.API.Application.Commands.UpsertClient;

public class UpsertClientCommand : IRequest<CommandResult>
{
    public UpsertClientCommand(Guid? id, string companyName, bool isDiscreet, int memberTierLevel)
    {
        Id = id;
        CompanyName = companyName;
        IsDiscreet = isDiscreet;
        MemberTierLevel = memberTierLevel;
    }
    public Guid? Id { get; private set; }
    public string CompanyName { get; private set; }
    public int MemberTierLevel { get; private set; }
    public bool IsDiscreet { get; private set; }
}

public class UpsertClientCommandValidation : AbstractValidator<UpsertClientCommand>
{
    public UpsertClientCommandValidation()
    {
        RuleFor(c => c.CompanyName).NotEmpty();
        RuleFor(c => c.MemberTierLevel).NotEmpty();
        RuleFor(c => c.IsDiscreet).NotEmpty();
    }
}

public class UpsertClientCommandHandler : IRequestHandler<UpsertClientCommand, CommandResult>
{
    private readonly IClientsRepository _repository;

    public UpsertClientCommandHandler(IClientsRepository repository)
    {
        _repository = repository;
    }
    public async Task<CommandResult> Handle(UpsertClientCommand request, CancellationToken cancellationToken)
    {
        Guid id = Guid.Empty;
        var tier = MemberTier.GetLevels().First(i => i.Level == request.MemberTierLevel).Id;
        if (request.Id.HasValue && request.Id.Value != Guid.Empty)
        {
            //update
            var updated = await _repository.GetEntityByIdAsync(request.Id.Value);

            if (updated == null) 
                return CommandResult.Failed(request.Id, $"Client with id:{request.Id} does not exist.");
           
            updated.UpdateSelf(request.CompanyName,request.IsDiscreet,request.MemberTierLevel);
            id = _repository.Update(updated).Id;
        }
        else
        {
            //create
            id = _repository.Create(request.CompanyName, request.IsDiscreet,tier).Id;
        }
        return CommandResult.Success(id);
    }
}