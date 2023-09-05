using System.Text.Json;
using BusinessCard.API.Application.Common.Models;

namespace BusinessCard.API.Application.Commands.EditMember;

public class EditMemberCommand : IRequest<Guid>
{
    public EditMemberCommand(Guid clientId, Guid memberId, string firstName, string lastName, string middleName, string nameSuffix, string phoneNumber, string email, string address, string occupation, string facebook, string linkedIn, string instagram, string pinterest, string twitter, string cardKey, int subscriptionLevel)
    {
        ClientId = clientId;
        MemberId = memberId;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        NameSuffix = nameSuffix;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        Occupation = occupation;
        CardKey = cardKey;
        SubscriptionLevel = subscriptionLevel;
        
        SocialMedia = JsonSerializer.Serialize(new SocialMediaObject()
        {
            Facebook = string.IsNullOrEmpty(facebook) ?  "N/A" : facebook,
            Instagram =string.IsNullOrEmpty(instagram) ?  "N/A" : instagram,
            LinkedIn = string.IsNullOrEmpty(linkedIn) ?  "N/A" : linkedIn,
            Pinterest = string.IsNullOrEmpty(pinterest) ?  "N/A" : pinterest,
            Twitter =string.IsNullOrEmpty(twitter) ?  "N/A" : twitter, 
        });
    }
    public Guid ClientId { get; init; }
    public Guid MemberId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public string NameSuffix { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string Address { get; init; }
    public string Occupation { get; init; }
    public string SocialMedia { get; init; }
    public string CardKey { get; init; }
    public int SubscriptionLevel { get; init; }
}