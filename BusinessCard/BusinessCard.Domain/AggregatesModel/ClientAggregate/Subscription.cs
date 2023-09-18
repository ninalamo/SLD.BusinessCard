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
    
    public Subscription(Guid billingPlanId, DateTimeOffset startDate, DateTimeOffset endDate, int level = 1, int cardExpiryInMonths = 12, Status state = Status.New)
    {
        _billingPlanId = billingPlanId;
        
        StartDate = startDate;
        EndDate = endDate;
        Reason = Enum.GetName(typeof(Status), Status.New);
        State = state;

        Level = level;
        CardExpiryInMonths = cardExpiryInMonths;

        _persons = new List<Person>();
    }
    private readonly List<Person> _persons;
    public IReadOnlyCollection<Person> Persons => _persons.AsReadOnly();
   
  
    public int Level { get; private set; }
    public string Description { get; set; }
    public int CardExpiryInMonths { get; private set; }
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
        new(_billingPlanId, renewDate, renewDate.AddMonths(numberOfMonthsToExpire), state: Status.Renewed);

    public Subscription CreateRenewal(DateTimeOffset renewData, DateTimeOffset endDate)
    {
        return new(_billingPlanId, renewData, endDate, state: Status.Renewed);
    }
    
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
        Level = level;
        CardExpiryInMonths = expiresInMonths;
        Description = description;
    }

    public void UpdateReminderInterval(int dayOfMonth) => PaymentScheduleReminderInterval = dayOfMonth;
    public void UpdatePaymentScheduleInterval(int dayOfMonth) => PaymentScheduleInterval = dayOfMonth;

    public Person AddMember(Person person)
    {
        person.IsActive = person.HasIdentity();
        _persons.Add(person);
        
        return person;
    }
}