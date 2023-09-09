using System.Text.Json;
using BusinessCard.API.Application.Common.Models;
using BusinessCard.API.Application.Queries.GetMemberId;
using BusinessCard.Application.Application.Common.Interfaces;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Application.Application.Queries.GetMemberByUid;

public class GetMemberUidQueryHandler : IRequestHandler<GetMemberByUidQuery, GetMemberByUidQueryResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<GetMemberIdQueryHandler> _logger;

    public GetMemberUidQueryHandler(IClientsRepository repository, ILogger<GetMemberIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<GetMemberByUidQueryResult> Handle(GetMemberByUidQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {GetMemberIdQueryHandlerName} {Now}", nameof(GetMemberIdQueryHandler), DateTimeOffset.Now);

        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);
        
        if (client == null) throw new KeyNotFoundException("Client not found.");

        var member = client.Persons.FirstOrDefault(c => c.Card.Key == request.Uid && c.Id == request.MemberId);

        if (member == null)
            throw new BusinessCardDomainException("Member not found.", new KeyNotFoundException("Card Key not found."));


        var result = new GetMemberByUidQueryResult
        {
                ClientId = request.ClientId,
                Subscription = member.Subscription.Name,
                SubscriptionLevel = member.Subscription.Level,
                Address = member.Address,
                CardKey = member.Card.Key,
                CreatedBy = member.CreatedBy,
                CreatedOn = member.CreatedOn,
                Email = member.Email,
                Facebook = ToSocialMediaObject(member.SocialMedia).Facebook,
                FirstName = member.FirstName,
                LastName = member.LastName,
                NameSuffix = member.NameSuffix,
                MiddleName = member.MiddleName,
                Id = member.Id,
                Instagram = ToSocialMediaObject(member.SocialMedia).Instagram,
                Pinterest = ToSocialMediaObject(member.SocialMedia).Pinterest,
                Occupation = member.Occupation,
                Twitter = ToSocialMediaObject(member.SocialMedia).Twitter,
                IsActive = member.IsActive,
                PhoneNumber = member.PhoneNumber,
                ModifiedBy = member.ModifiedBy,
                ModifiedOn = member.ModifiedOn,
                LinkedIn = ToSocialMediaObject(member.SocialMedia).LinkedIn,
                IdentityUserId = member.IdentityUserId,
                Company = client.CompanyName
        };

        return result;


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