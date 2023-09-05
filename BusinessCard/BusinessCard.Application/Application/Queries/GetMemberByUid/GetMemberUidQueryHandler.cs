using System.Text.Json;
using BusinessCard.API.Application.Common.Models;
using BusinessCard.API.Application.Queries.GetMemberId;
using BusinessCard.Application.Application.Common.Interfaces;

namespace BusinessCard.Application.Application.Queries.GetMemberByUid;

public class GetMemberUidQueryHandler : IRequestHandler<GetMemberByUidQuery, GetMemberByUidQueryResult>
{
    private readonly IClientQueries _clientQueries;
    private readonly ILogger<GetMemberUidQueryHandler> _logger;

    public GetMemberUidQueryHandler(IClientQueries clientQueries, ILogger<GetMemberUidQueryHandler> logger)
    {
        _clientQueries = clientQueries;
        _logger = logger;
    }

    public async Task<GetMemberByUidQueryResult> Handle(GetMemberByUidQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {GetMemberIdQueryHandlerName} {Now}", nameof(GetMemberIdQueryHandler),
            DateTimeOffset.Now);

        var member = await _clientQueries.GetClientByUid(request.Uid);

        if (member == null) return null;

        return new GetMemberByUidQueryResult
        {
            ClientId = member.ClientId,
            Subscription = member.Subscription,
            SubscriptionLevel = member.SubscriptionLevel,
            Address = member.Address,
            CardKey = member.CardKey,
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