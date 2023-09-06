using System.Runtime.InteropServices.JavaScript;
using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

    public class Client : Entity, IAggregateRoot
    {
        public string CompanyName { get; set; }
        private Guid _subscriptionId;
        public MemberTier Subscription { get; private set; }
        public bool IsDiscreet { get; set; }

        private readonly List<Person> _persons;
        public IReadOnlyCollection<Person> Persons => _persons.AsReadOnly();

        #region Constructors and Factory
        
        private Client() =>_persons = new List<Person>();
        
        public Client(string name, bool isDiscreet, Guid subscriptionId) : this()
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            _subscriptionId = subscriptionId;
        }
        
        public Client(string name, bool isDiscreet, int level) : this()
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            var tier = MemberTier.GetLevels().FirstOrDefault(i => i.Level == level)?.Id;
            
            if(!tier.HasValue) throw new ArgumentNullException(nameof(level));
            if (tier.Value == Guid.Empty) throw new ArgumentNullException(nameof(level));

            _subscriptionId = tier.Value;

        }
        #endregion

        public void Amend(string name, bool isDiscreet, int subscription)
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            _subscriptionId = MemberTier.GetLevels().First(i => i.Level == subscription).Id;
        }
        

        public Person AddMemberAsync(string firstName,
            string lastName,
            string middleName,
            string nameSuffix,
            string phoneNumber,
            string email,
            string address,
            string occupation,
            string socialMedia)
        {
            var person = new Person(firstName, lastName, middleName, nameSuffix, phoneNumber, email, address,
                occupation, socialMedia)
            {
                IsActive = false
            };
            _persons.Add(person);
            return person;
        }
    }
