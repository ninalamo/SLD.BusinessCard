namespace BusinessCard.Application.Application.Commands.ExportCards;

public class GeneratePlaceholdersCommand : IRequest<GeneratePlaceholdersCommandResult>
{
    public GeneratePlaceholdersCommand(Guid clientId, int count, Guid subscriptionId)
    {
        ClientId = clientId;
        SubscriptionId = subscriptionId;
        Count = count;
    }

    public Guid ClientId { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public int Count { get; private set; }
}

public record GeneratePlaceholdersCommandResult
{
    public IEnumerable<string> Urls { get; init; }
}