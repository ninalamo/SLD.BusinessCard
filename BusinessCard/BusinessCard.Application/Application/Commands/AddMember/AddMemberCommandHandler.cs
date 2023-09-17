using BusinessCard.Application.Application.Commands.AddClient;
using BusinessCard.Application.Application.Common.Helpers;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.AddMember;

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

        if (!client.Subscriptions.Any())
            throw new ValidationException("Client must have a subscription",
                new[] { new ValidationFailure("Subscription", "Cannot add member for client has no subscription.") });


        Subscription defaultSubscription = client.Subscriptions.FirstOrDefault(i => i.Id == request.SubscriptionId);

        if (defaultSubscription == null)
            throw new ValidationException("Subscription not found.",
                new[] { new ValidationFailure("Subscription", "SubscriptionId not found.") });
        
        _logger.LogInformation($"Adding {nameof(Person)}-{DateTimeOffset.Now}");
        var person = new Person(request.FirstName, request.LastName, request.MiddleName, request.NameSuffix,
            request.PhoneNumber, request.Email, request.Address, request.Occupation, "");

        person.SetSocialMedia(request.Facebook, request.Instagram, request.Twitter, request.Pinterest,
            request.LinkedIn);

        _logger.LogInformation($"Adding {nameof(Person)}-{DateTimeOffset.Now}");
        defaultSubscription.AddMember(person);

        _logger.LogInformation($"Update {nameof(Client)}-{DateTimeOffset.Now}");
        _repository.Update(client);

        _logger.LogInformation($"Saving changes... {DateTimeOffset.Now}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return person.Id;
    }

}