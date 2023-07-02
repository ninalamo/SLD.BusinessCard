namespace BusinessCard.Domain.AggregatesModel.CustomerAggregate
{
    public class Customer : Entity
    {
        public Customer(string firstName, string lastName, string middleName, string phoneNumber, string email, string address, NfcCard card)
        {
            FirstName = firstName;
            LastName = lastName ;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            Card = card ?? throw new ArgumentNullException(nameof(card));
        }

        public Customer(string firstName, string lastName, string middleName, string phoneNumber, string email, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }


        public NfcCard? Card { get; private set; }
        public void BindToNFC(NfcCard card)
        {
            Card = card ?? throw new ArgumentException(nameof(card));
        }

    }
}
