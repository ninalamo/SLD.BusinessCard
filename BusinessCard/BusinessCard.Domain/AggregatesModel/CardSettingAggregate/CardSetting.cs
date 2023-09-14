using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.CardSettingAggregate;

public class CardSetting : Entity, IAggregateRoot
{
    public int Level { get; private set; }
    public string Description { get; private set; }
    public int ExpiresInMonths { get; private set; }

    public CardSetting(int level, string description, int expiresInMonths)
    {
        Level = level;
        Description = description;
        ExpiresInMonths = expiresInMonths;
    }
}

public interface ICardLevelConfigRepository : IRepository<CardSetting>
{
    CardSetting Add(int level, int months, string description);
    CardSetting Update(Guid id,int level, int months, string description);
    
}