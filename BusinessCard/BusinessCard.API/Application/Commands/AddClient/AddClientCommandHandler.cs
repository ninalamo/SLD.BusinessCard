using System.Text.Json;
using System.Text.Json.Serialization;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using MediatR;


namespace BusinessCard.API.Application.Commands.UpsertClient;

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
        _logger.LogInformation($"Fetching tier id from database...");
        var tier = MemberTier.GetLevels().First(i => i.Level == request.MemberTierLevel).Id;
        
        _logger.LogInformation($"Creating {nameof(Client)}. Request:{JsonSerializer.Serialize(request)}");
        var id = (await _repository.CreateAsync(request.CompanyName, request.IsDiscreet,tier)).Id;
        
        return CommandResult.Success(id);
    }
}