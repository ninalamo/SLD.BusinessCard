using BusinessCard.Application.Application.Common.Helpers;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.AddMemberWithIdentityKey;

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
        
        _logger.LogInformation($"Fetching {nameof(Subscription)}-{DateTimeOffset.Now}");
        var subscription = client.Subscriptions.FirstOrDefault(i => i.Id == request.SubscriptionId);

        if (subscription == null)
            throw new ValidationException("Subscription not found.", new[]
            {
                new ValidationFailure("SubscriptionId", "SubscriptionId not found.")
            });

        _logger.LogInformation($"Validating request... {nameof(request)}-{DateTimeOffset.Now}");
        subscription.AdditionalValidation(request.PhoneNumber, request.Email);

        _logger.LogInformation($"Adding {nameof(Person)}-{DateTimeOffset.Now}");
        var person = new Person(
            request.FirstName,
            request.LastName,
            request.MiddleName, 
            request.NameSuffix,
            request.PhoneNumber,
            request.Email,
            request.Address,
            request.Occupation);
        
        person.SetSocialMedia(request.SocialMedia.Facebook,
            request.SocialMedia.Instagram,
            request.SocialMedia.Twitter,
            request.SocialMedia.Pinterest,
            request.SocialMedia.LinkedIn);
        
        _logger.LogInformation($"Setting industry level for {nameof(Person)}-{DateTimeOffset.Now}");
        person.Occupation = request.Occupation;
        
        _logger.LogInformation($"Adding IdentityId to {nameof(Person)}-{DateTimeOffset.Now}");
        person.SetIdentity(request.IdentityId);

        _logger.LogInformation($"Adding card to {nameof(Person)}-{DateTimeOffset.Now}");
        person.AddCard(request.CardKey, subscription.CardExpiryInMonths);
        
        _logger.LogInformation($"Update {nameof(Client)}-{DateTimeOffset.Now}");
        subscription.AddMember(person);
        _repository.Update(client);

        _logger.LogInformation($"Saving changes... {DateTimeOffset.Now}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return person.Id;
    }
}