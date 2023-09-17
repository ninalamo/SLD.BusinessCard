namespace BusinessCard.Application.Application.Commands.AddSubscription;

public class AddSubscriptionCommand : IRequest<Guid>
{
    public Guid ClientId { get; }
    public Guid PlanId { get; }
    public DateTimeOffset StartDate { get; }
    public int NumberOfMonthToExpire { get; }

    public AddSubscriptionCommand(Guid clientId, Guid planId, DateTimeOffset startDate, int numberOfMonthToExpire)
    {
        ClientId = clientId;
        PlanId = planId;
        StartDate = startDate;
        NumberOfMonthToExpire = numberOfMonthToExpire;
    }
}