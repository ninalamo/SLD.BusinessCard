namespace BusinessCard.Application.Application.Commands.ChangeSubscription;

public class ChangeSubscriptionCommand
{
    public ChangeSubscriptionCommand(Guid clientId, Guid memberId, Guid oldSubscriptionId, Guid newSubscriptionId)
    {
        ClientId = clientId;
        MemberId = memberId;
        OldSubscriptionId = oldSubscriptionId;
        NewSubscriptionId = newSubscriptionId;
    }
    public Guid ClientId { get; }
    public Guid MemberId { get; }
    public Guid OldSubscriptionId { get; }
    public Guid NewSubscriptionId { get; }
}

