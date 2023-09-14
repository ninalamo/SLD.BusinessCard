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
    public Subscription(Guid billingPlanId, Guid cardSettingId, DateTimeOffset startDate, int numberOfMonthsToExpire)
    {
        _billingPlanId = billingPlanId;
        _cardSettingId = cardSettingId;
        StartDate = startDate;
        EndDate = startDate.AddMonths(numberOfMonthsToExpire);
        Reason = Enum.GetName(typeof(Status), Status.New);
        State = Status.New;
       
    }

    private Subscription(Guid billingPlanId, Guid cardSettingId, DateTimeOffset startDate, int numberOfMonthsToExpire,
        Status state)
    {
        _billingPlanId = billingPlanId;
        _cardSettingId = cardSettingId;
        StartDate = startDate;
        EndDate = startDate.AddMonths(numberOfMonthsToExpire);
        Reason = Enum.GetName(typeof(Status), state);
        State = state;
    }
    
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public DateTimeOffset? ActualEndDate { get; private set; }
    public Status State { get; private set; }
    public string? Reason { get; private set; }
    public int PaymentScheduleReminderInterval { get; private set; } = DefaultReminderInterval;
    public int PaymentScheduleInterval { get; private set; } = DefaultScheduleInterval;
    public string GetName() => Id.ToString().Replace("-", "").ToUpper();

    private Guid _billingPlanId;
    private Guid _cardSettingId;

    public Guid GetBillingPlanId() => _billingPlanId;
    public Guid GetCardSettingId() => _cardSettingId;

    public Subscription CreateRenewal(DateTimeOffset renewDate, int numberOfMonthsToExpire) =>
        new(_billingPlanId, _cardSettingId, renewDate, numberOfMonthsToExpire, state: Status.Renewed);
    
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

    public void ChangeCardSetting(Guid newId)
    {
        _cardSettingId = newId;
    }

    public void UpdateReminderInterval(int dayOfMonth) => PaymentScheduleReminderInterval = dayOfMonth;
    public void UpdatePaymentScheduleInterval(int dayOfMonth) => PaymentScheduleInterval = dayOfMonth;

}