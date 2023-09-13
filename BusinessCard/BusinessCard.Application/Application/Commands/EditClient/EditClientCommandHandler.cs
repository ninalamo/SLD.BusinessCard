using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.API.Application.Commands.EditClient;

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

        var entity = await _repository.GetEntityByIdAsync(request.Id);

        _logger.LogInformation($"Checking if {nameof(request.Id)} exists. {DateTime.UtcNow}");

        if (entity == null)
        {
            _logger.LogInformation($"{nameof(request.Id)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Id does not exist.") });
        }
        _logger.LogInformation($"Updating {nameof(entity)}. {DateTime.UtcNow}");
        // entity.Amend(request.CompanyName,request.IsDiscreet,request.MemberTierLevel);
        //
        _repository.Update(entity);
        
        _logger.LogInformation($"Exiting {nameof(Handle)} in {nameof(EditClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");
        
        _logger.LogInformation($"Return {nameof(entity.Id)}. {DateTime.UtcNow}");
        return entity.Id;
    }
}