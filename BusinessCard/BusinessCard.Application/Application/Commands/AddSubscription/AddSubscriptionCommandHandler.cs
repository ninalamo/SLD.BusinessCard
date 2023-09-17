using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.AddSubscription;

public class AddSubscriptionCommandHandler : IRequestHandler<AddSubscriptionCommand, Guid>
{
    private readonly ILogger<AddSubscriptionCommandHandler> _logger;
    private readonly IClientsRepository _repository;

    public AddSubscriptionCommandHandler(ILogger<AddSubscriptionCommandHandler> logger, IClientsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<Guid> Handle(AddSubscriptionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(Handle)} in {nameof(AddSubscriptionCommandHandler)} with request: {request}. {DateTime.UtcNow}");

        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        _logger.LogInformation($"Checking if {nameof(request.ClientId)} exists. {DateTime.UtcNow}");

        if (client == null)
        {
            _logger.LogInformation($"{nameof(request.ClientId)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("ClientId", "ClientId does not exist.") });
        }
        
        _logger.LogInformation($"Adding subscriptions to {nameof(client)}. {DateTime.UtcNow}");
        client.AddSubscription(
            request.PlanId,
            request.StartDate,
            request.EndDate,
            request.CardLevel,
            request.CardExpiryInMonth);
        
        _repository.Update(client);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Exiting {nameof(Handle)} in {nameof(AddSubscriptionCommandHandler)} with request: {request}. {DateTime.UtcNow}");
        
        _logger.LogInformation($"Return {nameof(client.Id)}. {DateTime.UtcNow}");
        return client.Id;
    }
}