namespace BusinessCard.Domain.AggregatesModel.ClientSubscriptionAggregate;

public class SubscriptionPayment : Entity
{
    public Guid PlanId { get; private set; }
    public decimal Amount { get; private set; }
    public string ModeOfPayment { get; private set; }
    public DateTimeOffset PaymentDate { get; private set; }
    public int NumberOfDaysLate { get; private set; }
}