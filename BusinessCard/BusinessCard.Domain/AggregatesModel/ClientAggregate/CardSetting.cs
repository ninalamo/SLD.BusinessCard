using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class CardSetting : ValueObject
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
    

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Level;
        yield return Description;
        yield return ExpiresInMonths;
    }
}
