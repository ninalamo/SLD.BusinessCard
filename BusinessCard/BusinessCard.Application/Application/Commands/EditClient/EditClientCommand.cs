namespace BusinessCard.Application.Application.Commands.EditClient;

public class EditClientCommand : IRequest<Guid>
{
    public EditClientCommand(Guid id, string name, string industry)
    {
        Id = id;
        Name = name;
        Industry = industry;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Industry { get; private set; }
}