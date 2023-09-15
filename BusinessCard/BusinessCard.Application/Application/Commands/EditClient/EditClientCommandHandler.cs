using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.EditClient;

public class EditClientCommandHandler : IRequestHandler<EditClientCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<EditClientCommandHandler> _logger;

    public EditClientCommandHandler(IClientsRepository repository, ILogger<EditClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Guid> Handle(EditClientCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(Handle)} in {nameof(EditClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");

        var client = await _repository.GetEntityByIdAsync(request.Id);

        _logger.LogInformation($"Checking if {nameof(request.Id)} exists. {DateTime.UtcNow}");

        if (client == null)
        {
            _logger.LogInformation($"{nameof(request.Id)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Id does not exist.") });
        }
        
        _logger.LogInformation($"Updating {nameof(client)}. {DateTime.UtcNow}");
        client.Name = request.Name;
        client.Industry = request.Industry;
        
        _repository.Update(client);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Exiting {nameof(Handle)} in {nameof(EditClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");
        
        _logger.LogInformation($"Return {nameof(client.Id)}. {DateTime.UtcNow}");
        return client.Id;
    }
}