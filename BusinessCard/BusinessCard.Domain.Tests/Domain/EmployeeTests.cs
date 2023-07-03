using BusinessCard.Domain.AggregatesModel.CompanyAggregate;
using BusinessCard.Domain.AggregatesModel.NFCAggregate;
using Faker;
using Shouldly;
using NameFaker = Faker.Name;



namespace BusinessCard.Tests.Domain
{
    public class EmployeeTests
    {
        [Fact]
        public void shouldBeAbleToCreateWithoutCard()
        {
            Employee customer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines", null);
            customer.GetCardId().ShouldBe(default);
            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldBe("nin.alamo@outlook.com");
            customer.PhoneNumber.ShouldBe("09091234567");
        }

        [Fact]
        public void shouldBeAbleToCreate()
        {
            NfcCard card = new("Test", Guid.NewGuid());
            Employee customer = new(
                NameFaker.First(),
                NameFaker.Last(),
                NameFaker.Last(),
                Phone.Number(),
                Internet.Email(),
              Address.StreetName(),
                card.Id);

            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldNotBeNullOrEmpty();
            customer.PhoneNumber.ShouldNotBeNullOrEmpty();

            customer.GetCardId().ShouldNotBe(default);
        }

        [Fact]
        public void shouldBeAbleToBindCard()
        {
            ThisCustomer.AddCard(Guid.NewGuid());
            ThisCustomer.GetCardId().ShouldNotBeNull();

        }


        private static Employee ThisCustomer = new(
                NameFaker.First(),
                NameFaker.Last(),
                NameFaker.Last(),
                Phone.Number(),
                Address.StreetName(),
                Internet.Email(),
                Guid.NewGuid());
    }
}
