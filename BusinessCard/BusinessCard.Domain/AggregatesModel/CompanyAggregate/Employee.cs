
namespace BusinessCard.Domain.AggregatesModel.CompanyAggregate
{
    public class Employee : Entity
    {
        public Employee(string firstName, string lastName, string middleName, string phoneNumber, string email, string address, Guid? cardId)
        {

            if (cardId == default)
                throw new ArgumentNullException(nameof(cardId));

            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;

            _cardId = cardId;

        }

        public Employee(string firstName, string lastName, string middleName, string phoneNumber, string email, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            _cardId = null;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        private Guid? _cardId;
        public Guid? GetCardId() => _cardId;



        public void UpdateContactDetails(string phoneNumber, string email, string address)
        {
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }

        public void AddCard(Guid cardId)
        {
            _cardId = cardId;
        }
        public void RemoveCard()
        {
            _cardId = null;
        }

    }


}
