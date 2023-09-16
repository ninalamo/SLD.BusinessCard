using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.RemoveClient;

public class RemoveClientCommandHandler : IRequestHandler<RemoveClientCommand>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<RemoveClientCommandHandler> _logger;

    public RemoveClientCommandHandler(IClientsRepository repository, ILogger<RemoveClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(RemoveClientCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(Handle)} in {nameof(RemoveClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");

        var client = await _repository.GetEntityByIdAsync(request.Id);

        _logger.LogInformation($"Checking if {nameof(request.Id)} exists. {DateTime.UtcNow}");

        if (client == null)
        {
            _logger.LogInformation($"{nameof(request.Id)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Id does not exist.") });
        }

        client.IsActive = false;
        client.Subscriptions.ToList().ForEach(c =>
        {
            c.Cancel(DateTimeOffset.Now, "Client has been removed.");
            c.Persons.ToList().ForEach(p =>
            {
                p.IsActive = false;

            });
        });
        
        _logger.LogInformation($"Updating {nameof(client)}. {DateTime.UtcNow}");
        _repository.Update(client);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Exiting {nameof(Handle)} in {nameof(RemoveClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");
        
    }
}