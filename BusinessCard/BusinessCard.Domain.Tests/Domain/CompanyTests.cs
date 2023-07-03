using BusinessCard.Domain.AggregatesModel.CompanyAggregate;
using Shouldly;
using CompanyFaker = Faker.Company;
using NameFaker = Faker.Name;
using Phone = Faker.Phone;

namespace BusinessCard.Tests.Domain
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
            Company company = new(CompanyFaker.Name());
            company.Name = "GMA";
            company.Name.ShouldBe("GMA");
        }

        [Fact]
        public void shouldBeAbleToGenerateAndSaveCards()
        {
            Company company = new(CompanyFaker.Name());
            company.EnrolNFCCard(new[] { "ABC", "DEF", "XYZ", "AAA", "BBB", "CCC" });
            company.Cards.Count().ShouldBe(6);
            company.Cards.Select(c => c.Key).Contains("ABC").ShouldBeTrue();
        }

        [Fact]
        public void shouldBeAbleToAddEmployee()
        {
            Company company = new(CompanyFaker.Name());
            company.AddEmployee(new Employee("Nin", "Alamo", "Calzada", "09091234567", "nin.alamo@outlook.com", "General Trias, Cavite, Philippines", null));
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
