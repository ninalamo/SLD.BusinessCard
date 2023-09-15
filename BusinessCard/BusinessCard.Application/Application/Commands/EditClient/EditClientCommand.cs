namespace BusinessCard.Application.Application.Commands.EditClient;

public class EditClientCommand : IRequest<Guid>
{
    public EditClientCommand(Guid id, string companyName, int memberTierLevel, bool isDiscreet)
    {
        Id = id;
        CompanyName = companyName;
        MemberTierLevel = memberTierLevel;
        IsDiscreet = isDiscreet;
    }
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    public int MemberTierLevel { get; private set; }
    public bool IsDiscreet { get; private set; }
}