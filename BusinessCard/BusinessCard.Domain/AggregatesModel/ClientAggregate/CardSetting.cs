using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class CardSetting : Enumeration
{
    public int Level { get; private set; }
    public CardSetting(Guid id, string name, int level) : base(id, name)
    {
        Level = level;
    }
}
