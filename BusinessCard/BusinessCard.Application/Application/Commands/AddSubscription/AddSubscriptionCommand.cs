namespace BusinessCard.Application.Application.Commands.AddSubscription;

public class AddSubscriptionCommand : IRequest<Guid>
{
    public Guid ClientId { get; }
    public Guid PlanId { get; }
    public DateTimeOffset StartDate { get; }
    public DateTimeOffset EndDate { get; }
    public int CardExpiryInMonth { get; }
    public int CardLevel { get; }

    public AddSubscriptionCommand(Guid clientId, Guid planId, DateTimeOffset startDate, DateTimeOffset endDate, int cardExpiryInMonth, int level )
    {
        ClientId = clientId;
        PlanId = planId;
        StartDate = startDate;
        CardExpiryInMonth = cardExpiryInMonth;
        EndDate = endDate;
        CardLevel = level;
    }
}