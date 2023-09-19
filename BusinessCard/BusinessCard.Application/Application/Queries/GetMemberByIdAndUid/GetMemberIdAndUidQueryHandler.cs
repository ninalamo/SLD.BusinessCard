using System.Text.Json;
using BusinessCard.Application.Application.Common.Interfaces;
using BusinessCard.Application.Application.Common.Models;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Queries.GetMemberByIdAndUid;

public class GetMemberIdAndUidQueryHandler : IRequestHandler<GetMemberByIdAndUidQuery, GetMemberByIdAndUidQueryResult>
{
    private readonly IClientsRepository _repository;
    private readonly IClientQueries _queries;
    private readonly ILogger<GetMemberIdAndUidQueryHandler> _logger;

    public GetMemberIdAndUidQueryHandler(IClientsRepository repository, IClientQueries queries, ILogger<GetMemberIdAndUidQueryHandler> logger)
    {
        _repository = repository;
        _queries = queries;
        _logger = logger;
    }

    public async Task<GetMemberByIdAndUidQueryResult> Handle(GetMemberByIdAndUidQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {GetMemberIdQueryHandlerName} {Now}", nameof(GetMemberIdAndUidQueryHandler), DateTimeOffset.Now);

        var cardResults = await _queries.GetCardByUidAndClientId(request.Uid, request.ClientId);

        var emptyResult = new GetMemberByIdAndUidQueryResult()
        {
            Member = Array.Empty<MemberIdAndUidResult>()
        };
        ;

        if (!cardResults.Any()) return emptyResult;

        var card = cardResults.SingleOrDefault();

        var client = await _repository.GetWithPropertiesByIdAsync(card.ClientId);

        var subscription = client.Subscriptions.FirstOrDefault(i => i.Id == card.SubscriptionId);

        if (subscription == null) return emptyResult;

        var person = subscription.Persons.FirstOrDefault(i => i.Id == card.MemberId);

        if (person == null) return emptyResult;
            
        

        var m = new MemberIdAndUidResult()
        {
                ClientId = request.ClientId,
                Address = person.Address,
                CardKey = card.Uid,
                Email = person.Email,
                Facebook = person.SocialMediaAccounts?.Facebook,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NameSuffix = person.NameSuffix,
                MiddleName = person.MiddleName,
                MemberId = person.Id,
                Instagram = person.SocialMediaAccounts?.Instagram,
                Pinterest = person.SocialMediaAccounts?.Pinterest,
                Occupation = person.Occupation,
                Twitter = person.SocialMediaAccounts?.Twitter,
                IsActive = person.IsActive,
                PhoneNumber = person.PhoneNumber,
                LinkedIn = person.SocialMediaAccounts?.LinkedIn,
                IdentityUserId = person.IdentityUserId,
                Company = client.Name,
                SubscriptionId = subscription.Id,
                CardLevel = subscription.Level,
        };

        return new GetMemberByIdAndUidQueryResult()
        {
            Member = new[] { m }
        };
    }

    private static SocialMediaObject ToSocialMediaObject(string json)
    {
        try
        {
            var smo = JsonSerializer.Deserialize<SocialMediaObject>(json);
            return smo;
        }
        catch (Exception ex)
        {
            return new SocialMediaObject();
        }

    }
}