
using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public class Person : Entity
    {
        private Guid _subscriptionId;
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
            _subscriptionId = MemberTier.GetLevels().First(i => i.Level == 1).Id;
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
        public string Occupation { get; set; }
        public Card? Card { get; private set; }
        public MemberTier? Subscription { get; private set; }

        public void SetSubscription(int level)
        {
            _subscriptionId = MemberTier.GetTier(level).Id;
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

        public void SetSocialMedia(string links)
        {
            SocialMedia = links;
        }

        public void AddKeyToCard(string key)
        {
            if (Card == default) Card = new();
            Card.SetKey(key);
            IsActive = true;
        }

        public void RemoveCard() => Card = default;

        public bool HasCard() => Card != default(Card);

        public bool HasKeylessCard() => Card == default || !Card.HasKey(); 

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
