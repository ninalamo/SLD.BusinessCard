namespace BusinessCard.Domain.AggregatesModel.ClientSubscriptionAggregate;

public class ClientSubscription : Entity
{
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public DateTimeOffset? CancellationDate { get; private set; }
    public DateTimeOffset? SubscriptionChangeDate { get; private set; }
    public string ReasonForChange { get; private set; }
    public int PaymentDueReminderDate { get; private set; }
    public int PaymentDueDate { get; private set; }
    
    public Guid BillingPlanId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid SubscriptionId { get; private set; }
}