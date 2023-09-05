using BusinessCard.API.Application.Common.Interfaces.Helpers;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using MediatR;

namespace BusinessCard.API.Application.Commands.AddMemberWithIdentityKey;

public class AddMemberWithIdentityKeyCommandHandler : IRequestHandler<AddMemberWithIdentityKeyCommand, Guid>
{
    private readonly ILogger<AddMemberWithIdentityKeyCommandHandler> _logger;
    private readonly IClientsRepository _repository;

    public AddMemberWithIdentityKeyCommandHandler(ILogger<AddMemberWithIdentityKeyCommandHandler> logger, IClientsRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Guid> Handle(AddMemberWithIdentityKeyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(AddMemberWithIdentityKeyCommandHandler)}-{DateTimeOffset.Now}");
        
        _logger.LogInformation($"Fetching {nameof(Client)}-{DateTimeOffset.Now}");
        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        _logger.LogInformation($"Validating request... {nameof(request)}-{DateTimeOffset.Now}");
        client.AdditionalValidation(request.PhoneNumber, request.Email);

        _logger.LogInformation($"Adding {nameof(Person)}-{DateTimeOffset.Now}");
        var person = client.AddMemberAsync(request.FirstName, request.LastName, request.MiddleName, request.NameSuffix, request.PhoneNumber,request.Email,request.Address,request.Occupation,request.SocialMedia);

        _logger.LogInformation($"Adding IdentityId to {nameof(Person)}-{DateTimeOffset.Now}");
        person.SetIdentity(request.IdentityId);
        
        _logger.LogInformation($"Update {nameof(Client)}-{DateTimeOffset.Now}");
        _repository.Update(client);

        _logger.LogInformation($"Saving changes... {DateTimeOffset.Now}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return person.Id;
    }
}