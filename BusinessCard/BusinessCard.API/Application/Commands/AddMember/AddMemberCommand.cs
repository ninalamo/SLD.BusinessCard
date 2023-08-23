using MediatR;

namespace BusinessCard.API.Application.Commands.AddMember;

public class AddMemberCommand : IRequest<Guid>
{
    public AddMemberCommand(Guid clientId, string firstName, string lastName, string middleName, string nameSuffix, string phoneNumber, string email, string address, string occupation, 
        string facebook, string linkedIn, string instagram, string pinterest, string twitter)
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

        var list = new List<string>();
        
        if(!string.IsNullOrEmpty(facebook)) list.Add(facebook);
        if(!string.IsNullOrEmpty(facebook)) list.Add(linkedIn);
        if(!string.IsNullOrEmpty(facebook)) list.Add(instagram);
        if(!string.IsNullOrEmpty(facebook)) list.Add(linkedIn);
        if(!string.IsNullOrEmpty(facebook)) list.Add(pinterest);
        if(!string.IsNullOrEmpty(facebook)) list.Add(twitter);

        SocialMedia = list.Any() ? list.ToArray() : Array.Empty<string>();

    }
    public Guid ClientId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public string NameSuffix { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string Address { get; init; }
    public string Occupation { get; init; }
    public string[] SocialMedia { get; private set; }
}