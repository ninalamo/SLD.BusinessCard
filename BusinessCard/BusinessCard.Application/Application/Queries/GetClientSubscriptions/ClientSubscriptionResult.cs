using BusinessCard.Domain.AggregatesModel.ClientAggregate;

namespace BusinessCard.Application.Application.Queries.GetClientSubscriptions;

public record ClientSubscriptionResult
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public Guid BillingPlanId { get; init; }
    public int Level { get; init; }
    public string Description { get;init;}
    public int CardExpiryInMonths { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public DateTimeOffset? ActualEndDate { get; init; }
    public Subscription.Status State { get;init; }
    public string StateDescription => Enum.GetName(typeof(Subscription.Status), State);
    public string? Reason { get; init; }
    public int PaymentScheduleReminderInterval { get;init; }
    public int PaymentScheduleInterval { get;init; } 
}