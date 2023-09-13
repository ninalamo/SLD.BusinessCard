using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.API.Application.Commands.RemoveClient;

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

        var entity = await _repository.GetEntityByIdAsync(request.Id);

        _logger.LogInformation($"Checking if {nameof(request.Id)} exists. {DateTime.UtcNow}");

        if (entity == null)
        {
            _logger.LogInformation($"{nameof(request.Id)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Id does not exist.") });
        }

        entity.IsActive = false;
        entity.Persons.ToList().ForEach(p =>
        {
            p.IsActive = false;
            // p.DisableCard();
        });
        _logger.LogInformation($"Updating {nameof(entity)}. {DateTime.UtcNow}");
        _repository.Update(entity);
        
        _logger.LogInformation($"Exiting {nameof(Handle)} in {nameof(RemoveClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");

        
    }
}