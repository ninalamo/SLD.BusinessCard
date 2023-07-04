using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public class Card : Entity
    {

        public string Key { get; private set; }
        public Card() { }

        public void LinkToPhysicalCard(string key)
        {
            if(key != default)
            {
                throw BusinessCardDomainException.Create(new Exception("Business validation error. NFC Key is immutable"));
            }
            Key = key;
        }

    }
}
