using BusinessCard.Domain.AggregatesModel.CompanyAggregate;
using BusinessCard.Domain.AggregatesModel.NFCAggregate;
using Shouldly;

namespace BusinessCard.Domain.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void shouldBeAbleToCreateWithoutCard()
        {
            Employee customer = new("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines");
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
                NameFaker.MaleFirstName(),
                NameFaker.LastName(), 
                NameFaker.LastName(), 
                PhoneFaker.Phone(),
                InternetFaker.Email(),
                LocationFaker.StreetName(),
                card.Id);

            customer.FullName.ShouldBe($"{customer.FirstName} {customer.MiddleName} {customer.LastName}");
            customer.Email.ShouldNotBeNullOrEmpty();
            customer.PhoneNumber.ShouldNotBeNullOrEmpty();

            customer.GetCardId().ShouldNotBe(default);
        }

        [Fact]
        public void shouldBeAbleToBindCard()
        {
            ThisCustomer.GetCardId().ShouldBe(default);

            ThisCustomer.AddCard(It.IsAny<Guid>());
            ThisCustomer.GetCardId().ShouldBe(It.IsAny<Guid>());
           
        }


        private static Employee ThisCustomer = new(
                NameFaker.MaleFirstName(),
                NameFaker.LastName(),
                NameFaker.LastName(),
                PhoneFaker.Phone(),
                InternetFaker.Email(),
                LocationFaker.StreetName());
    }
}
