namespace BusinessCard.Application.Application.Commands.AddClient;

public class AddClientCommand : IRequest<CommandResult>
{
    public AddClientCommand(string name, string industry, bool isDiscreet)
    {
        Name = name;
        Industry = industry;
        IsDiscreet = isDiscreet;
    }
    public string Name { get; private set; }
    public string Industry { get; private set; }
    public bool IsDiscreet { get; private set; }
}