using BusinessCard.Domain.Exceptions;
using FluentValidation;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public sealed class Card : Entity
    {
        public string Key { get; private set; }

        public Card()
        {
            Key = string.Empty;
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
