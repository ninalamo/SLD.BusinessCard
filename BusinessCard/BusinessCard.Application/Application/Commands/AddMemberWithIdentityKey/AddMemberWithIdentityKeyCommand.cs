using System.Text.Json;
using BusinessCard.Application.Application.Common.Models;

namespace BusinessCard.Application.Application.Commands.AddMemberWithIdentityKey;


public class AddMemberWithIdentityKeyCommand : IRequest<Guid>
{
    public AddMemberWithIdentityKeyCommand(Guid clientId, string firstName, string lastName, string middleName, string nameSuffix, string phoneNumber, string email, string address, string occupation, 
        string identityId, string cardKey, Guid subscriptionId, SocialMediaObject socialMedia)
    {
        ClientId = clientId;
        SubscriptionId = subscriptionId;
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

        SocialMedia = socialMedia;

    }
    public Guid SubscriptionId { get; }
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
    public SocialMediaObject SocialMedia { get; private set; }
    public string CardKey { get; }
}