using BusinessCard.API.Logging;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BusinessCard.API.Application.Commands.EditClientCommandHandler;

public class EditClientCommand : IRequest<Guid>
{
    public EditClientCommand(Guid id, string companyName, int memberTierLevel, bool isDiscreet)
    {
        Id = id;
        CompanyName = companyName;
        MemberTierLevel = memberTierLevel;
        IsDiscreet = isDiscreet;
    }
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    public int MemberTierLevel { get; private set; }
    public bool IsDiscreet { get; private set; }
}

public class EditClientCommandHandler : IRequestHandler<EditClientCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILoggerAdapter<EditClientCommandHandler> _logger;

    public EditClientCommandHandler(IClientsRepository repository, ILoggerAdapter<EditClientCommandHandler> logger)
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
            throw BusinessCardDomainException.Create(new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Id does not exist.") }));
        }
        _logger.LogInformation($"Updating {nameof(entity)}. {DateTime.UtcNow}");
        entity.UpdateSelf(request.CompanyName,request.IsDiscreet,request.MemberTierLevel);
        
        _repository.Update(entity);
        
        _logger.LogInformation($"Return {nameof(entity.Id)}. {DateTime.UtcNow}");
        return entity.Id;
    }
}