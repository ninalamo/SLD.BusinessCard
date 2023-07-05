﻿
using BusinessCard.Domain.Exceptions;
using Newtonsoft.Json;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public sealed class Person : Entity
    {
        public Person(
            string firstName, 
            string lastName, 
            string middleName, 
            string nameSuffix,
            string phoneNumber,
            string email, 
            string address, 
            string occupation,
            string[] socialMedia) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            NameSuffix = nameSuffix;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            Occupation = occupation;
            
            SetSocialMedia(socialMedia);
        }

        public Person()
        {
            IsSubsriptionOverride = false;
            Card = new();
            
#if DEBUG
            Id = Guid.NewGuid();
#endif
        }

        public bool IsSubsriptionOverride { get; private set; }
        public Tier? OverrideSubscription { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleName { get; private set; }
        public string NameSuffix { get; private set; }

        public string PhoneNumber { get; set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string SocialMedia { get; private set; }
        public string Occupation { get; private set; }
        public Card Card { get; private set; }


        public void SetOverrideSubscription(Tier overrideSubscription)
        {
            IsSubsriptionOverride = true;
            OverrideSubscription = overrideSubscription;
        }

        public void SetName(string firstname,string lastname, string middlename, string nameSuffix)
        {
            FirstName = firstname;
            LastName = lastname;
            MiddleName = middlename; 
            NameSuffix = nameSuffix;
        }

        public void SetContactDetails(string phoneNumber, string email, string address)
        {
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }

        public void SetSocialMedia(string[] links)
        {
            var json = string.Empty;
            if (links.Any()) {
                json = JsonConvert.SerializeObject(links.Select(x => new { link = x }).ToArray());
            }
            SocialMedia = json;
        }
        
        public void SetCard(string key)
        {
            if (Card == default) Card = new();
            Card.SetKey(key);
        }

        public void RemoveCard()
        {
            Card = null;
        }

        public bool HasCard() => Card != default;

        public void EnableCard()
        {
            if (Card == default) throw BusinessCardDomainException.CreateArgumentNullException(nameof(Card));
            Card.IsActive = true;
        }

        public void DisableCard()
        {
            if (Card == default) throw BusinessCardDomainException.CreateArgumentNullException(nameof(Card));
            Card.Deactivate();
        }

    }
}
