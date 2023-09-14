using BusinessCard.Domain.AggregatesModel.CardSettingAggregate;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class Subscription : Entity
{
    public enum Status
    {
        New = 0,
        Cancelled = 1,
        EndOfContract = 2,
        PreTerminated = 3,
        Renewed = 4
    }
    public Subscription(Guid billingPlanId, Guid cardSettingId, DateTimeOffset startDate, int numberOfMonthsToExpire)
    {
        _billingPlanId = billingPlanId;
        _cardSettingId = cardSettingId;
        StartDate = startDate;
        EndDate = startDate.AddMonths(numberOfMonthsToExpire);
        Reason = Enum.GetName(typeof(Status), Status.New);
        State = Status.New;
    }
    
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public DateTimeOffset? ActualEndDate { get; private set; }
    public Status State { get; private set; }
    public string? Reason { get; private set; }
    public int PaymentDueReminderDate { get; private set; } = 10;
    public int PaymentDueDate { get; private set; } = 15;
    public string GetName() => Id.ToString().Replace("-", "").ToUpper();

    private Guid _billingPlanId;
    private Guid _cardSettingId;
    
    public BillingPlan BillingPlan { get; private set; }
    public CardSetting CardSetting { get; private set; }

    public void Cancel(DateTimeOffset lastDay, string reason)
    {
        State = Status.Cancelled;
        Reason = reason;
        ActualEndDate = lastDay;
    }

}