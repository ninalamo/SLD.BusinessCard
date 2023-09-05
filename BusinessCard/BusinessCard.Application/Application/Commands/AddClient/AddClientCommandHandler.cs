using System.Text.Json;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using MediatR;

namespace BusinessCard.API.Application.Commands.AddClient;

public class AddClientCommandHandler : IRequestHandler<AddClientCommand, CommandResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<AddClientCommandHandler> _logger;

    public AddClientCommandHandler(IClientsRepository repository, ILogger<AddClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(AddClientCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(AddClientCommandHandler)}.");
        _logger.LogInformation($"Creating {nameof(Client)}. Request:{JsonSerializer.Serialize(request)}");
        var id = (await _repository.CreateAsync(request.CompanyName, request.IsDiscreet,request.MemberTierLevel)).Id;
        
        return CommandResult.Success(id);
    }
}