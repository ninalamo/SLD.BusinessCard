using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{

    public class Client : Entity, IAggregateRoot
    {
        public string CompanyName { get; set; }

        private Guid _memberTierId;
        public bool IsDiscreet { get; set; }


        private readonly List<Person> _persons;
        public IReadOnlyCollection<Person> Persons => _persons.AsReadOnly();


        #region Constructors and Factory
        private Client() { 
            _persons = new List<Person>();
            IsActive = false;
        }
        public Client(string name, bool isDiscreet, Guid memberTierId) : this()
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            _memberTierId = memberTierId;
        }
        
        public Client(string name, bool isDiscreet, int level) : this()
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            var tier = MemberTier.GetLevels().FirstOrDefault(i => i.Level == level)?.Id;
            
            if(!tier.HasValue) throw BusinessCardDomainException.Create(new ArgumentException(nameof(level)));
            if (tier.Value == Guid.Empty) throw BusinessCardDomainException.Create(new ArgumentException(nameof(level)));

            _memberTierId = tier.Value;

        }
        #endregion

        public void UpdateSelf(string name, bool isDiscreet, int subscription)
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            _memberTierId = MemberTier.GetLevels().First(i => i.Level == subscription).Id;
        }

        public async Task GenerateContactsAsync(int count)
        {
            if (count > 1000)
            {
                throw BusinessCardDomainException.Create(new InvalidOperationException("Count limit exceeded 1000"));
            }

            await Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    _persons.Add(new Person());
                }
            });
        }

        private Person AddMember(Person person)
        {
            _persons.Add(person);
            return person;
        }

        public async Task<Person> AddMember(string firstName,
            string lastName,
            string middleName,
            string nameSuffix,
            string phoneNumber,
            string email,
            string address,
            string occupation,
            string[] socialMedia)
        {
            var person = new Person(firstName, lastName, middleName, nameSuffix, phoneNumber, email, address,
                occupation, socialMedia);

            return AddMember(person);
        }
    

    }
}
