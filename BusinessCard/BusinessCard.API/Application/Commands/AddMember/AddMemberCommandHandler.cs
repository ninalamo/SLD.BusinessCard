using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Logging;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BusinessCard.API.Application.Commands.AddMember;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILoggerAdapter<AddClientCommandHandler> _logger;

    public AddMemberCommandHandler(IClientsRepository repository, ILoggerAdapter<AddClientCommandHandler> logger)
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
        BusinessValidate(request, client);

        _logger.LogInformation($"Adding {nameof(Person)}-{DateTimeOffset.Now}");
        var person = await client.AddMember(request.FirstName, request.LastName, request.MiddleName, request.NameSuffix, request.PhoneNumber,request.Email,request.Address,request.Occupation,request.SocialMedia);

        _logger.LogInformation($"Update {nameof(Client)}-{DateTimeOffset.Now}");
        _repository.Update(client);

        _logger.LogInformation($"Saving changes... {DateTimeOffset.Now}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return person.Id;
    }

    private static void BusinessValidate(AddMemberCommand request, Client client)
    {
        var validationFailures = new List<ValidationFailure>();
        if (client.Persons.Any(i => i.PhoneNumber == request.PhoneNumber))
            validationFailures.Add(new ValidationFailure("PhoneNumber",
                $"PhoneNumber: ({request.PhoneNumber}) Already exists."));

        if (client.Persons.Any(i =>  i.Email.ToLower() == request.Email.ToLower()))
            validationFailures.Add(new ValidationFailure("Email", $"Email: ({request.Email}) Already exists."));

        if (validationFailures.Any())
            throw new ValidationException("Business validation error.", validationFailures);
    }
}