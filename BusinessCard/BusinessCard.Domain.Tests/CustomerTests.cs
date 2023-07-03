using BusinessCard.Domain.AggregatesModel.CustomerAggregate;
using BusinessCard.Domain.AggregatesModel.NfcAggregate;
using Moq;
using Shouldly;

namespace BusinessCard.Domain.Tests
{
    public class CustomerTests
    {
        [Fact]
        public void shouldBeAbleToCreateWithoutCard()
        {
            Customer customer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines");
            customer.GetCardId().ShouldBe(default);
            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldBe("nin.alamo@outlook.com");
            customer.PhoneNumber.ShouldBe("09091234567");
        }

        [Fact]
        public void shouldBeAbleToCreate()
        {
            NfcCard card = new("Test", Guid.NewGuid());
            Customer customer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines");
            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldBe("nin.alamo@outlook.com");
            customer.PhoneNumber.ShouldBe("09091234567");

            customer.GetCardId().ShouldBe(default);
        }

        [Fact]
        public void shouldBeAbleToBindCard()
        {
            ThisCustomer.GetCardId().ShouldBe(default);

            ThisCustomer.BindToNFC(It.IsAny<Guid>());
            ThisCustomer.GetCardId().ShouldBe(It.IsAny<Guid>());
           
        }

        private static Customer ThisCustomer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines");

    }
}
