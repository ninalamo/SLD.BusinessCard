using System.Text.Json;
using BusinessCard.API.Application.Common.Models;

namespace BusinessCard.Application.Application.Commands.AddMemberWithIdentityKey;


public class AddMemberWithIdentityKeyCommand : IRequest<Guid>
{
    public AddMemberWithIdentityKeyCommand(Guid clientId, string firstName, string lastName, string middleName, string nameSuffix, string phoneNumber, string email, string address, string occupation, 
        string facebook, string linkedIn, string instagram, string pinterest, string twitter, string identityId, string cardKey)
    {
        ClientId = clientId;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        NameSuffix = nameSuffix;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        Occupation = occupation;
        IdentityId = identityId;
        CardKey = cardKey;

        SocialMedia = JsonSerializer.Serialize(new SocialMediaObject()
        {
            Facebook = string.IsNullOrEmpty(facebook) ?  "N/A" : facebook,
            Instagram =string.IsNullOrEmpty(instagram) ?  "N/A" : instagram,
            LinkedIn = string.IsNullOrEmpty(linkedIn) ?  "N/A" : linkedIn,
            Pinterest = string.IsNullOrEmpty(pinterest) ?  "N/A" : pinterest,
            Twitter =string.IsNullOrEmpty(twitter) ?  "N/A" : twitter, 
        });

    }
    public Guid ClientId { get; }
    public string IdentityId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string MiddleName { get; }
    public string NameSuffix { get; }
    public string PhoneNumber { get; }
    public string Email { get; }
    public string Address { get; }
    public string Occupation { get; }
    public string SocialMedia { get; private set; }
    public string CardKey { get; }
}