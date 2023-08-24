﻿
using BusinessCard.Domain.Exceptions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public class Person : Entity
    {
        private Guid _memberTierId;
        private Guid _cardId;

        public Person(
            string firstName, 
            string lastName, 
            string middleName, 
            string nameSuffix,
            string phoneNumber,
            string email, 
            string address, 
            string occupation,
            string socialMedia) : this()
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
            _memberTierId = MemberTier.GetLevels().First(i => i.Level == 1).Id;
            Card = new Card();
            IsActive = false;
        }

        public bool IsSubsriptionOverride { get; private set; }


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

        public void SetSocialMedia(string links)
        {
            SocialMedia = links;
        }

        private string ToJson(Dictionary<string, string> dict)
        {
            var entries = dict.Select(d => string.Format("\"{0}\": [{1}]", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
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

        public bool HasCard() => Card != default(Card);

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
