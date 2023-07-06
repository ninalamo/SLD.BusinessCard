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
            Client company = new(companyName, false, Tier.Basic);
            company.CompanyName.ShouldBe(companyName);

            var assembly = Assembly.GetAssembly(typeof(LokiContext))?.GetName().Name;
            assembly.ShouldNotBeNullOrEmpty(assembly);
        }

        [Fact]
        public void ClientShouldBeAbleToRename()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Tier.Basic);
            company.CompanyName = "GMA";
            company.CompanyName.ShouldBe("GMA");
        }

        [Fact]
        public void PersonShouldHaveEmptyButNotNullContacts()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Tier.Basic);
            company.Persons.ShouldBeEmpty();
            company.Persons.ShouldNotBeNull();
        }

        [Fact]
        public async Task ClientShouldBeAbleToGenerateMultiplePlaceholders()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Tier.Basic);

            await company.GenerateContactsAsync(1000);
    
            company.Persons.ShouldNotBeEmpty();
            company.Persons.Count().ShouldBe(1000);
            company.Persons.First().IsTransient().ShouldBeFalse();        
            company.Persons.First().Card.Key.ShouldBeNullOrEmpty();
        }

        [Fact]
        public void ClientCanBeDiscreet()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Tier.Basic);
            company.IsDiscreet = false;

            company.IsDiscreet.ShouldBeFalse();
        }

    }
}
