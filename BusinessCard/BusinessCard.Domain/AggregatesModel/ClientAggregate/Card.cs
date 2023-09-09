using BusinessCard.Domain.Exceptions;
using FluentValidation;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public sealed class Card : Entity
    {
        public enum CardType
        {
            LevelOne = 1,
            LevelTwo = 2,
            LevelThree = 3,
            LevelFour = 4,
            LevelFive = 5,
            LevelSix = 6,
        }
        
        public string Key { get; private set; }
        public CardType Type { get; private set; }

        public Card()
        {
            Key = string.Empty;
            Type = CardType.LevelOne;
        }

        public void SetKey(string key)
        {
            if(Key != string.Empty)
            {
                throw  new ValidationException("Business validation error. NFC Key is immutable");
            }
            Key = key;

        }

        public bool HasKey() => !string.IsNullOrEmpty(Key);

    }
}
