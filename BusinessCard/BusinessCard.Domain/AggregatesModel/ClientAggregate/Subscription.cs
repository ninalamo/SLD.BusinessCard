namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class Subscription : Entity
{
    public const int DefaultReminderInterval = 10;
    public const int DefaultScheduleInterval = 15;
    
    public enum Status
    {
        New = 0,
        Cancelled = 1,
        EndOfContract = 2,
        PreTerminated = 3,
        Renewed = 4,
        Deleted = 5,
    }
   
    
    public Subscription(Guid billingPlanId, DateTimeOffset startDate, DateTimeOffset endDate, Status state = Status.New)
    {
        _billingPlanId = billingPlanId;
        
        StartDate = startDate;
        EndDate = endDate;
        Reason = Enum.GetName(typeof(Status), Status.New);
        State = state;
    }
    
   
    public CardSetting Setting { get; private set; }


    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public DateTimeOffset? ActualEndDate { get; private set; }
    public Status State { get; private set; }
    public string? Reason { get; private set; }
    public int PaymentScheduleReminderInterval { get; private set; } = DefaultReminderInterval;
    public int PaymentScheduleInterval { get; private set; } = DefaultScheduleInterval;
    public string GetName() => Id.ToString().Replace("-", "").ToUpper();

    private Guid _billingPlanId;
  
    public Guid GetBillingPlanId() => _billingPlanId;

    public Subscription CreateRenewal(DateTimeOffset renewDate, int numberOfMonthsToExpire) =>
        new(_billingPlanId, renewDate, renewDate.AddMonths(numberOfMonthsToExpire), Status.Renewed);
    
    public void PreTerminate(DateTimeOffset lastDay, string reason)
    {
        State = Status.PreTerminated;
        Reason = reason;
        ActualEndDate = lastDay;
        IsActive = false;
    }
    
    public void EndContract(DateTimeOffset lastDay, string reason)
    {
        State = Status.EndOfContract;
        Reason = reason;
        ActualEndDate = lastDay;
        IsActive = false;
    }

    public void Cancel(DateTimeOffset lastDay, string reason)
    {
        State = Status.Cancelled;
        Reason = reason;
        ActualEndDate = lastDay;
        IsActive = false;
    }

    public void MarkAsDeleted(string reason)
    {
        State = Status.Deleted;
        Reason = reason;
        IsActive = false;
    }

    public void ChangeBillingPlan(Guid newId)
    {
        _billingPlanId = newId;
    }

    public void ChangeCardSetting(int level, int expiresInMonths, string description)
    {
        Setting = new CardSetting(level, description, expiresInMonths);
    }

    public void UpdateReminderInterval(int dayOfMonth) => PaymentScheduleReminderInterval = dayOfMonth;
    public void UpdatePaymentScheduleInterval(int dayOfMonth) => PaymentScheduleInterval = dayOfMonth;

}