using System.Text.Json;
using BusinessCard.Application.Application.Common.Interfaces;
using BusinessCard.Application.Application.Common.Models;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Application.Application.Queries.GetMemberByUid;


public class GetMemberByUidQueryHandler : IRequestHandler<GetMemberByUidQuery, GetMemberByUidQueryResult>
{
    private readonly IClientsRepository _repository;
    private readonly IClientQueries _queries;
    private readonly ILogger<GetMemberByUidQueryHandler> _logger;

    public GetMemberByUidQueryHandler(IClientsRepository repository, IClientQueries queries, ILogger<GetMemberByUidQueryHandler> logger)
    {
        _repository = repository;
        _queries = queries;
        _logger = logger;
    }

    public async Task<GetMemberByUidQueryResult> Handle(GetMemberByUidQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {GetMemberIdQueryHandlerName} {Now}", nameof(GetMemberByUidQueryResult), DateTimeOffset.Now);

        

        string message;
        
        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);
        message = client != null ? "Client id not found." : string.Empty;

        var cardExists = await _queries.IsCardExists(request.Uid);
        message += cardExists ? string.Empty : $"{Environment.NewLine}Card key not found.";

       // var person = client?.Persons.FirstOrDefault();//c =>  c.Card?.Key == request.Uid);
        // message +=  person == null ? string.Empty : $"{Environment.NewLine}Key no longer available.";
        
      
        
        // var member = new MemberUidResult()
        // {
        //     ClientId = request.ClientId,
        //     Subscription = "To remove",//person?.Subscription?.Name,
        //     SubscriptionLevel = 1, //person.Subscription.Level,
        //     Address = person.Address,
        //     // CardKey = person.Card.Key,
        //     CreatedBy = person.CreatedBy,
        //     CreatedOn = person.CreatedOn,
        //     Email = person.Email,
        //     Facebook = ToSocialMediaObject(person.SocialMedia).Facebook,
        //     FirstName = person.FirstName,
        //     LastName = person.LastName,
        //     NameSuffix = person.NameSuffix,
        //     MiddleName = person.MiddleName,
        //     Id = person.Id,
        //     Instagram = ToSocialMediaObject(person.SocialMedia).Instagram,
        //     Pinterest = ToSocialMediaObject(person.SocialMedia).Pinterest,
        //     Occupation = person.Occupation,
        //     Twitter = ToSocialMediaObject(person.SocialMedia).Twitter,
        //     IsActive = person.IsActive,
        //     PhoneNumber = person.PhoneNumber,
        //     ModifiedBy = person.ModifiedBy,
        //     ModifiedOn = person.ModifiedOn,
        //     LinkedIn = ToSocialMediaObject(person.SocialMedia).LinkedIn,
        //     IdentityUserId = person.IdentityUserId,
        //     Company = client.Name
        // };
        //
        // result.SetMember(member);

        return default;
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