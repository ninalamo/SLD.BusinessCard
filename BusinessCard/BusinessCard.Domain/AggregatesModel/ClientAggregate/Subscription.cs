namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class Subscription : Entity
{
    public Subscription(Guid billingPlanId, Guid clientId, Guid configId, DateTimeOffset startDate, int numberOfMonthsToExpire)
    {
        _billingPlanId = billingPlanId;
        _clientId = clientId;
        _configId = configId;
        StartDate = startDate;
        EndDate = startDate.AddMonths(numberOfMonthsToExpire);
    }
    
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public DateTimeOffset? CancellationDate { get; private set; }
    public DateTimeOffset? RenewalDate { get; private set; }
    public DateTimeOffset? SubscriptionChangeDate { get; private set; }
    public string? ReasonForChange { get; private set; }
    public int PaymentDueReminderDate { get; private set; } = 10;
    public int PaymentDueDate { get; private set; } = 15;

    private Guid _billingPlanId;
    private Guid _clientId;
    private Guid _configId;
    
    // public BillingPlan BillingPlan { get; private set; }
    // public Person Person { get; private set; }
    // public CardLevelConfig Config { get; private set; }
}