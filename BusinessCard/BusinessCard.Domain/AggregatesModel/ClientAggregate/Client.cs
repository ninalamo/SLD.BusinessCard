using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{

    public class Client : Entity, IAggregateRoot
    {
        public string CompanyName { get; set; }
        public Tier Subscription { get; private set; }
        public bool IsDiscreet { get; set; }


        private readonly List<Person> _persons;
        public IReadOnlyCollection<Person> Persons => _persons.AsReadOnly();


        #region Constructors and Factory
        private Client() { 
            _persons = new List<Person>();
            IsActive = false;
        }
        public Client(string name, bool isDiscreet, Tier subscription) : this()
        {
            CompanyName = name;
            IsDiscreet = isDiscreet;
            Subscription = subscription;
        }
        #endregion

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
    

    }
}
