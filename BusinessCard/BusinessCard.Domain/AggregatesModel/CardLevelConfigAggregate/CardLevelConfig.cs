using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.SubscriptionAggregate;

public class CardLevelConfig : Entity, IAggregateRoot
{
    public int Level { get; private set; }
    public string Description { get; private set; }
    public int ExpiresInMonths { get; private set; }

    public CardLevelConfig(int level, string description, int expiresInMonths)
    {
        Level = level;
        Description = description;
        ExpiresInMonths = expiresInMonths;
    }
}

public interface ICardLevelConfigRepository : IRepository<CardLevelConfig>
{
    CardLevelConfig Add(int level, int months, string description);
    CardLevelConfig Update(Guid id,int level, int months, string description);
    
}