using BusinessCard.Application.Application.Common.Helpers;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;

namespace BusinessCard.Application.Application.Commands.EditMember;

public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<EditMemberCommandHandler> _logger;

    public EditMemberCommandHandler(IClientsRepository repository, ILogger<EditMemberCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(EditMemberCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(EditMemberCommandHandler)}-{DateTimeOffset.Now}");
        
        _logger.LogInformation($"Fetching {nameof(Client)}-{DateTimeOffset.Now}");
        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        _logger.LogInformation($"Validating request... {nameof(request)}-{DateTimeOffset.Now}");
        client.AdditionalValidation(request.PhoneNumber, request.Email, request.MemberId);

        _logger.LogInformation($"Getting {nameof(Person)}-{DateTimeOffset.Now}");
        //TODO: Refactor
        // var person = client.Persons.FirstOrDefault(p => p.Id == request.MemberId);
        //
        // // person.AddKeyToCard(request.CardKey);
        // person.SetContactDetails(request.PhoneNumber, request.Email, request.Address);
        // person.SetName(request.FirstName, request.LastName, request.MiddleName, request.NameSuffix);
        // person.Occupation = request.Occupation;
        // person.SetSocialMedia(request.SocialMedia);
        // person.SetSubscription(request.SubscriptionLevel);

        _logger.LogInformation($"Update {nameof(Client)}-{DateTimeOffset.Now}");
        _repository.Update(client);

        _logger.LogInformation($"Saving changes... {DateTimeOffset.Now}");
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Guid.NewGuid();//TODO: Refactor .Id;
    }
}

public class EditMemberCommandValidator : AbstractValidator<EditMemberCommand>
{
    public EditMemberCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Occupation).NotEmpty();

        RuleFor(x => x.SocialMedia).Must(x => x.Any());
    }
}