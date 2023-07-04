using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate
{

    public class Client : Entity, IAggregateRoot
    {
        public string CompanyName { get; set; }
        public Tier Subscription { get; private set; }
        public bool IsDiscreet { get; private set; }


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

       
    

    }

    public interface ICompanyRepository : IRepository<Client>
    {
        Client Create(string name);
        Client Update(Client company);
        
    }
}
