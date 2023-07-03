using BusinessCard.Domain.AggregatesModel.CompanyAggregate;
using Castle.Core.Resource;
using Faker;
using Shouldly;

namespace BusinessCard.Domain.Tests
{
    public class CompanyTests
    {
        [Fact]
        public void shouldBeAbleToCreate()
        {
            string companyName = CompanyFaker.Name();
            Company company = new(companyName);
            company.Name.ShouldBe(companyName);
        }

        [Fact]
        public void shouldBeAbleToRename()
        {
            Company company = new(CompanyFaker.Name()) ;
            company.Name = "GMA";
            company.Name.ShouldBe("GMA");
        }

        [Fact]
        public void shouldBeAbleToGenerateAndSaveCards() {
            Company company = new(CompanyFaker.Name());
            company.EnrolNFCCard(new[] { "ABC", "DEF", "XYZ", "AAA", "BBB", "CCC" });
            company.Cards.Count().ShouldBe(6);
            company.Cards.Select(c => c.Key).Contains("ABC").ShouldBeTrue();
        }

        [Fact]
        public void shouldBeAbleToAddEmployee()
        {
            Company company = new (CompanyFaker.Name()) ;
            company.AddEmployee(It.IsAny<Employee>());
            company.Employees.ShouldNotBeNull();
            company.Employees.Count().ShouldBe(1);
        }

        [Fact]
        public void shouldBeAbleToRemoveEmployee()
        {
            Company company = new("ABC");
            company.AddEmployee(new(
                NameFaker.MaleFirstName(),
                NameFaker.LastName(),
                NameFaker.LastName(),
                PhoneFaker.Phone(),
                InternetFaker.Email(),
                LocationFaker.StreetName()));

            company.Employees.ShouldNotBeNull();
            company.Employees.Count().ShouldBe(1);

            company.RemoveEmployee(company.Employees.First().Id);
            company.Employees.Count().ShouldBe(0);
        }

    }
}
