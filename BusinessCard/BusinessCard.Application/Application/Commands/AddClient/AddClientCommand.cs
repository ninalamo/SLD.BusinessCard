namespace BusinessCard.Application.Application.Commands.AddClient;

public class AddClientCommand : IRequest<Guid>
{
    public AddClientCommand(string name, string industry)
    {
        Name = name;
        Industry = industry;
    }
    public string Name { get; private set; }
    public string Industry { get; private set; }
   
}