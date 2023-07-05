using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{

    public class Client : Entity, IAggregateRoot
    {
        public string CompanyName { get; set; }
        public Tier Subscription { get; private set; }
        public bool IsDiscreet { get; set; }


        private List<Person> _contacts;
        public IReadOnlyCollection<Person> Contacts => _contacts.AsReadOnly();


        #region Constructors and Factory
        public Client() { 
            _contacts = new List<Person>();
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
                    _contacts.Add(new Person());
                }
            });
        }
    

    }
}
