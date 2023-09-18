                          


namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{
    public class Person : Entity
    {
        private readonly List<Card> _cards;
        public IReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

        public Person(
            string firstName,
            string lastName,
            string middleName,
            string nameSuffix,
            string phoneNumber,
            string email,
            string address,
            string occupation) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            NameSuffix = nameSuffix;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            Occupation = occupation;
            
            SocialMediaAccounts = new SocialMedia("N/A", "N/A", "N/A", "N/A", "N/A");
        }

        public Person()
        {
            IsSubscriptionOverride = false;
            _cards = new List<Card>();
            IsActive = false;
        }

        public SocialMedia SocialMediaAccounts { get; private set; }
        public bool IsSubscriptionOverride { get; private set; }
        public string IdentityUserId { get; private set; } = string.Empty;

        public void SetIdentity(string key) => IdentityUserId = Guid.Parse(key).ToString();
        public bool HasIdentity() => !string.IsNullOrEmpty(IdentityUserId);

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleName { get; private set; }
        public string NameSuffix { get; private set; }

        public string PhoneNumber { get; set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string Occupation { get; set; }

        public void SetSubscription(int level)
        {
            throw new NotImplementedException();
        }

        public void AddCard(string uid, int cardExpiryInMonths)
        {
          
            var newCard = new Card();
            newCard.Activate(uid,cardExpiryInMonths);
            _cards.Add(newCard);
            
        }

        public void SetName(string firstname, string lastname, string middlename, string nameSuffix)
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

        public void SetSocialMedia(string facebook, string instagram, string twitter, string pinterest, string linkedId)
        {
            SocialMediaAccounts = new SocialMedia(facebook, instagram, linkedId, pinterest, twitter);
        }

    }
}
