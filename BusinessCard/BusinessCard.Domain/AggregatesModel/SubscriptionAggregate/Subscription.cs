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
