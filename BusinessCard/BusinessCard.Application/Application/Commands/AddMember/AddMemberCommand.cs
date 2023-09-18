using System.Text.Json;
using BusinessCard.Application.Application.Common.Models;

namespace BusinessCard.Application.Application.Commands.AddMember;

public class AddMemberCommand : IRequest<Guid>
{
    public AddMemberCommand(Guid clientId, string firstName, string lastName, string middleName, string nameSuffix, string phoneNumber, string email, string address, string occupation, 
        string facebook, string linkedIn, string instagram, string pinterest, string twitter, Guid subscriptionId)
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


        SubscriptionId = subscriptionId;
        Facebook = string.IsNullOrEmpty(facebook) ? "N/A" : facebook;
        Instagram = string.IsNullOrEmpty(instagram) ? "N/A" : instagram;
        LinkedIn = string.IsNullOrEmpty(linkedIn) ? "N/A" : linkedIn;
        Pinterest = string.IsNullOrEmpty(pinterest) ? "N/A" : pinterest;
        Twitter = string.IsNullOrEmpty(twitter) ? "N/A" : twitter;


    }
    public Guid ClientId { get; init; }
    public Guid SubscriptionId { get; init; }
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
  

}