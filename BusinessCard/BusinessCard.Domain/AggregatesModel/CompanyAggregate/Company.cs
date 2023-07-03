using BusinessCard.Domain.AggregatesModel.NFCAggregate;
using BusinessCard.Domain.Exceptions;

namespace BusinessCard.Domain.AggregatesModel.CompanyAggregate
{
    public class Company : Entity, IAggregateRoot
    {
        public string Name { get; set; }

        private List<Employee> _employees;
        public IEnumerable<Employee> Employees => _employees.AsReadOnly();

        private List<NfcCard> _cards;
        public IEnumerable<NfcCard> Cards => _cards.AsReadOnly();


        #region Constructors and Factory
        public Company() { 
            _employees = new List<Employee>();
            _cards = new List<NfcCard>();
        }
        public Company(string name) : this()
        {
            Name = name;

        }
        public static Company Create(string name) => new(name);
        #endregion

        #region Employee CRUD
        public void AddEmployee(Employee employee) => _employees.Add(employee);

        public void UpdateEmployee(Employee employee)
        {
            if (employee.IsTransient()) throw BusinessCardDomainException.Create(new ArgumentNullException(nameof(employee)));

            var toUpdate = _employees.SingleOrDefault(x => x.Id == employee.Id) ?? throw BusinessCardDomainException.Create(new KeyNotFoundException(nameof(employee.Id)));

            toUpdate.FirstName = employee.FirstName;
            toUpdate.LastName = employee.LastName;
            toUpdate.MiddleName = employee.MiddleName;
            toUpdate.UpdateContactDetails(employee.PhoneNumber, employee.Email, employee.Address);

        }

        public void RemoveEmployee(Guid id)
        {
            var employee = _employees.SingleOrDefault(x => x.Id == id) ?? throw BusinessCardDomainException.Create(new KeyNotFoundException(nameof(id)));
            _employees.Remove(employee);
        }
        #endregion

        #region Card CRUD
    

        public void EnrolNFCCard(string[] nfcKey)
        {
#if !DEBUG
            if (this.IsTransient()) throw BusinessCardDomainException.Create(new InvalidOperationException("Entity must persist in database first."));
#endif
            _cards.AddRange(nfcKey.Select(c => GenerateBindedCard(c)));
        }

        public void DisableNFCCard(string nfcKey)
        {
            var card = _cards.SingleOrDefault(c => c.Key == nfcKey);

            if(card == default) throw BusinessCardDomainException.Create(new KeyNotFoundException(nameof(nfcKey)));

            card.Deactivate();
        }
#endregion

        private NfcCard GenerateBindedCard(string nfcKey)
        {
            return new NfcCard(nfcKey, this.Id);
        }

    }

    public interface ICompanyRepository : IRepository<Company>
    {
        Company Create(string name);
        Company Update(Company company);
        
    }
}
