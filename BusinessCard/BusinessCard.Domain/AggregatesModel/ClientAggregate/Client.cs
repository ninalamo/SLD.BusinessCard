using System.Runtime.InteropServices.JavaScript;
using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public bool IsDiscreet { get; set; }
        public string Industry { get; set; }


        private readonly List<Subscription> _subscriptions;
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();
        

        private readonly List<Person> _persons;
        public IReadOnlyCollection<Person> Persons => _persons.AsReadOnly();

        #region Constructors and Factory
        
        private Client()
        {
            _persons = new List<Person>();
            _subscriptions = new List<Subscription>();
        }
        
        public Client(string name, bool isDiscreet, string industry) : this()
        {
            
            
            Name = name;
            IsDiscreet = isDiscreet;
            Industry = industry;
        }
        #endregion


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
