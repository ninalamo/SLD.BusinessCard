using System.Text.Json;
using BusinessCard.API.Application.Common.Models;
using BusinessCard.API.Application.Queries.GetMemberId;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;

namespace BusinessCard.Application.Application.Queries.GetMemberByIdAndUid;

public class GetMemberIdAndUidQueryHandler : IRequestHandler<GetMemberByIdAndUidQuery, GetMemberByIdAndUidQueryResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<GetMemberIdQueryHandler> _logger;

    public GetMemberIdAndUidQueryHandler(IClientsRepository repository, ILogger<GetMemberIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<GetMemberByIdAndUidQueryResult> Handle(GetMemberByIdAndUidQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {GetMemberIdQueryHandlerName} {Now}", nameof(GetMemberIdQueryHandler), DateTimeOffset.Now);

        var result = new GetMemberByIdAndUidQueryResult(); 
        result.SetMessage("");
        result.IsValid = true;
        
        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);
        if (client == null)
        {
            result.SetMember(null);
            result.SetMessage("Client id not found.");
            result.IsValid = false;

            return result;
        }

        var person = client.Persons.FirstOrDefault(c =>  c.Id == request.MemberId);
        if (person == null)
        {
            result.SetMember(null);
            result.SetMessage("Member id not found.");
            result.IsValid = false;

            return result;
        }

        // if (person.HasKeylessCard())
        // {
        //     result.IsValid = true;
        //     result.SetMember(null);
        //     result.SetMessage("");
        //     return result;
        // }
        // else
        // {
        //     if (person.Card.Key != request.Uid)
        //     {
        //         result.IsValid = false;
        //         result.SetMessage("Member already assigned.");
        //         return result;
        //     }
        // }

        var member = new MemberIdAndUidResult()
        {
                ClientId = request.ClientId,
                Subscription = "To remove",//person.Subscription.Name,
                SubscriptionLevel = 1, //person.Subscription.Level,
                Address = person.Address,
                // CardKey = person.Card.Key,
                CreatedBy = person.CreatedBy,
                CreatedOn = person.CreatedOn,
                Email = person.Email,
                Facebook = ToSocialMediaObject(person.SocialMedia).Facebook,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NameSuffix = person.NameSuffix,
                MiddleName = person.MiddleName,
                Id = person.Id,
                Instagram = ToSocialMediaObject(person.SocialMedia).Instagram,
                Pinterest = ToSocialMediaObject(person.SocialMedia).Pinterest,
                Occupation = person.Occupation,
                Twitter = ToSocialMediaObject(person.SocialMedia).Twitter,
                IsActive = person.IsActive,
                PhoneNumber = person.PhoneNumber,
                ModifiedBy = person.ModifiedBy,
                ModifiedOn = person.ModifiedOn,
                LinkedIn = ToSocialMediaObject(person.SocialMedia).LinkedIn,
                IdentityUserId = person.IdentityUserId,
                Company = client.Name
        };

        result.SetMember(member);

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