using BusinessCard.Domain.AggregatesModel.CompanyAggregate;
using Shouldly;
using CompanyFaker = Faker.Company;
using NameFaker = Faker.Name;
using Phone = Faker.Phone;

namespace BusinessCard.Domain.Tests
{
    public class CompanyTests
    {
        [Fact]
        public void shouldBeAbleToCreate()
        {
            string companyName = Faker.Company.Name();
            Company company = new(companyName);
            company.Name.ShouldBe(companyName);
        }

        [Fact]
        public void shouldBeAbleToRename()
        {
            Company company = new(Faker.Company.Name()) ;
            company.Name = "GMA";
            company.Name.ShouldBe("GMA");
        }

        [Fact]
        public void shouldBeAbleToGenerateAndSaveCards() {
            Company company = new(Faker.Company.Name());
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
                NameFaker.First(),
                NameFaker.Last(),
                NameFaker.Last(),
                Phone.Number(),
                Faker.Internet.Email(),
                Faker.DateOfBirth.Next().AddYears(-31).ToShortDateString(), null));

            company.Employees.ShouldNotBeNull();
            company.Employees.Count().ShouldBe(1);

            company.RemoveEmployee(company.Employees.First().Id);
            company.Employees.Count().ShouldBe(0);
        }

    }
}
