using System.Text.Json;
using BusinessCard.Application.Application.Common.Models;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;

namespace BusinessCard.Application.Application.Queries.GetMemberId;

public class GetMemberIdQuery : IRequest<GetMemberByIdQueryResult>
{
    public GetMemberIdQuery(Guid clientId, Guid memberId)
    {
        ClientId = clientId;
        MemberId = memberId;
    }
    public Guid ClientId { get; init; }
    public Guid MemberId { get; init; }
}

public class GetMemberIdQueryHandler : IRequestHandler<GetMemberIdQuery, GetMemberByIdQueryResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<GetMemberIdQueryHandler> _logger;

    public GetMemberIdQueryHandler(IClientsRepository repository, ILogger<GetMemberIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<GetMemberByIdQueryResult> Handle(GetMemberIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {GetMemberIdQueryHandlerName} {Now}", nameof(GetMemberIdQueryHandler), DateTimeOffset.Now);

        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);
        
        if (client == null) throw new KeyNotFoundException("Client not found.");

        var member = client.Persons.FirstOrDefault(c => c.Id == request.MemberId);

        if (member == null) throw new KeyNotFoundException("Member not found.");

        return new GetMemberByIdQueryResult()
        {
            ClientId = request.ClientId,
            Subscription = "To remove",//member.MemberTier.Name,
            SubscriptionLevel = 1,//member.MemberTier.Level,
            Address = member.Address,
            // CardKey = member.Card?.Key ?? "",
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


public record GetMemberByIdQueryResult
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public string NameSuffix { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string Address { get; init; }
    public string Occupation { get; init; }
    public string Facebook { get; init; }
    public string LinkedIn { get; init; }
    public string Instagram { get; init; }
    public string Pinterest { get; init; }
    public string Twitter { get; init; }
    public string CardKey { get; init; }
    public string? Subscription { get; init; }
    public int SubscriptionLevel { get; init; }
    public string? CreatedBy { get; init; }
    public string? ModifiedBy { get; init; }
    public DateTimeOffset? CreatedOn { get; init; }
    public DateTimeOffset? ModifiedOn { get; init; }
    public bool IsActive { get; init; }
    public string SocialMedia { get; init; }
}