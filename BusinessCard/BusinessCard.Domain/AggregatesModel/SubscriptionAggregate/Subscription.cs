using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.SubscriptionAggregate;

public class Subscription : Entity
{
    public int Level { get; private set; }
    public string Description { get; private set; }
    public int ExpiresInMonths { get; private set; }

    public Subscription(int level, string description, int expiresInMonths)
    {
        Level = level;
        Description = description;
        ExpiresInMonths = expiresInMonths;
    }
}

public class Plan : Enumeration
{
    public static IEnumerable<Plan> Plans = new Plan[]
    {
        new (Guid.Parse("00000000-0000-0000-0000-000000000001"), "Free Trial"),
        new (Guid.Parse("00000000-0000-0000-0000-000000000002"), "Monthly"),
        new (Guid.Parse("00000000-0000-0000-0000-000000000003"), "Yearly"),
    };
    
    public Plan(Guid id, string name) : base(id, name)
    {
    }
}

public class ClientSubscription : Entity
{
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public DateTimeOffset? CancellationDate { get; private set; }
    public DateTimeOffset? SubscriptionChangeDate { get; private set; }
    public string ReasonForChange { get; private set; }
    public int PaymentDueReminderDate { get; private set; }
    public int PaymentDueDate { get; private set; }
    
    
    public Guid ClientId { get; private set; }
    public Guid SubscriptionId { get; private set; }
}

public class SubscriptionPayment : Entity
{
    public Guid PlanId { get; private set; }
    public decimal Amount { get; private set; }
    public string ModeOfPayment { get; private set; }
    public DateTimeOffset PaymentDate { get; private set; }
    public int NumberOfDaysLate { get; private set; }
}