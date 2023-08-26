using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Application.Common.Interfaces.Helpers;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BusinessCard.API.Application.Commands.AddMember;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<AddClientCommandHandler> _logger;

    public AddMemberCommandHandler(IClientsRepository repository, ILogger<AddClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(AddMemberCommandHandler)}-{DateTimeOffset.Now}");
        
        _logger.LogInformation($"Fetching {nameof(Client)}-{DateTimeOffset.Now}");
        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        _logger.LogInformation($"Validating request... {nameof(request)}-{DateTimeOffset.Now}");
        client.AdditionalValidation(request.PhoneNumber, request.Email);

        _logger.LogInformation($"Adding {nameof(Person)}-{DateTimeOffset.Now}");
        var person = await client.AddMemberAsync(request.FirstName, request.LastName, request.MiddleName, request.NameSuffix, request.PhoneNumber,request.Email,request.Address,request.Occupation,request.SocialMedia);

        _logger.LogInformation($"Update {nameof(Client)}-{DateTimeOffset.Now}");
        _repository.Update(client);

        _logger.LogInformation($"Saving changes... {DateTimeOffset.Now}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return person.Id;
    }

}