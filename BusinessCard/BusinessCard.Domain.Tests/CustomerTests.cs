using BusinessCard.Domain.AggregatesModel.CustomerAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests
{
    public class CustomerTests
    {
        [Fact]
        public void shouldBeAbleToCreateWithoutCard()
        {
            Customer customer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines");
            customer.Card.ShouldBeNull();
            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldBe("nin.alamo@outlook.com");
            customer.PhoneNumber.ShouldBe("09091234567");
        }

        [Fact]
        public void shouldBeAbleToCreate()
        {
            NfcCard card = new("Test", Guid.NewGuid());
            Customer customer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines",card);
            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldBe("nin.alamo@outlook.com");
            customer.PhoneNumber.ShouldBe("09091234567");

            customer.Card.ShouldNotBeNull();
            customer.Card.Key.ShouldBe("Test");
            customer.Card.CompanyId.ShouldNotBe(default);
        }

        [Fact]
        public void shouldBeAbleToBindCard()
        {
            ThisCustomer.Card.ShouldBe(default);

            ThisCustomer.BindToNFC(new NfcCard("Test",Guid.NewGuid()));
            ThisCustomer.Card.ShouldNotBeNull();
            ThisCustomer.Card.Key.ShouldBe("Test");
        }

        private static Customer ThisCustomer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines");

    }
}
