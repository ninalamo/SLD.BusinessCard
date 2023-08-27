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
            Client company = new(companyName, false, 1);
            company.CompanyName.ShouldBe(companyName);

            var assembly = Assembly.GetAssembly(typeof(LokiContext))?.GetName().Name;
            assembly.ShouldNotBeNullOrEmpty(assembly);
        }

        [Fact]
        public void ClientShouldBeAbleToRename()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false,1);
            company.CompanyName = "GMA";
            company.CompanyName.ShouldBe("GMA");
        }

        [Fact]
        public void PersonShouldHaveEmptyButNotNullContacts()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, 1);
            company.Persons.ShouldBeEmpty();
            company.Persons.ShouldNotBeNull();
        }

        [Fact]
        public async Task ClientShouldBeAbleToGenerateMultiplePlaceholders()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, 1);
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
            company.Persons.All(p => p.Card.HasKey()).ShouldBeFalse();
            company.Persons.All(p => p.Card.Key == string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void ClientCanBeDiscreet()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, 1);
            company.IsDiscreet = false;

            company.IsDiscreet.ShouldBeFalse();
        }

    }
}
