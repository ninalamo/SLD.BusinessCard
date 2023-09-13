using System.Reflection;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Infrastructure;
using Shouldly;
using CompanyFaker = Faker.Company;
using NameFaker = Faker.Name;
using Phone = Faker.Phone;

namespace BusinessCard.Tests.Domain
{
    public class ClientTest
    {
        [Fact]
        public void ClientShouldBeCreated()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "industry");
            company.Name.ShouldBe(companyName);
            company.IsDiscreet.ShouldBe(false);
            company.Industry.ShouldBe("industry");

            
        }

        [Fact]
        public void ClientShouldBeAbleToRename()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false,"");
            company.Name = "GMA";
            company.Name.ShouldBe("GMA");
        }

        [Fact]
        public void PersonShouldHaveEmptyButNotNullContacts()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            company.Persons.ShouldBeEmpty();
            company.Persons.ShouldNotBeNull();
        }

        [Fact]
        public async Task ClientShouldBeAbleToGenerateMultiplePlaceholders()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            int count = 1000;
            
            for (int i = 0; i < count; i++)
            {
                company.AddMemberAsync(
                    "N/A", 
                    "N/A", 
                    "N/A",
                    "N/A",
                    Guid.NewGuid().ToString(),
                    $"{Guid.NewGuid().ToString()}@tuldok.co", 
                    "N/A",
                    "N/A",
                    "{\n  \"Facebook\": \"N/A\",\n  \"LinkedIn\": \"N/A\",\n  \"Pinterest\": \"N/A\",\n  \"Instagram\": \"N/A\",\n  \"Twitter\": \"N/A\"\n}");
            }

        
            company.Persons.ShouldNotBeEmpty();
            company.Persons.Count.ShouldBe(1000);
        }

        [Fact]
        public void ClientCanBeDiscreet()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            company.IsDiscreet = false;

            company.IsDiscreet.ShouldBeFalse();
        }

    }
}
