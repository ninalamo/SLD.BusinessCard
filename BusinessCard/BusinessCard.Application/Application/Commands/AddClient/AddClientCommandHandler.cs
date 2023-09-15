using System.Text.Json;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;

namespace BusinessCard.Application.Application.Commands.AddClient;

public class AddClientCommandHandler : IRequestHandler<AddClientCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<AddClientCommandHandler> _logger;

    public AddClientCommandHandler(IClientsRepository repository, ILogger<AddClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddClientCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(AddClientCommandHandler)}.");
        _logger.LogInformation($"Creating {nameof(Client)}. Request:{JsonSerializer.Serialize(request)}");
        var created = await _repository.CreateAsync(request.Name, request.Industry);
        
        _logger.LogInformation($"Saving {nameof(Client)}. Request:{JsonSerializer.Serialize(request)}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return created.Id;
    }
}