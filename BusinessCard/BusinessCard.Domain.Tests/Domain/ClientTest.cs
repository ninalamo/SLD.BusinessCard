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
            Client company = new(companyName, false, Guid.Empty);
            company.CompanyName.ShouldBe(companyName);

            var assembly = Assembly.GetAssembly(typeof(LokiContext))?.GetName().Name;
            assembly.ShouldNotBeNullOrEmpty(assembly);
        }

        [Fact]
        public void ClientShouldBeAbleToRename()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false,Guid.Empty);
            company.CompanyName = "GMA";
            company.CompanyName.ShouldBe("GMA");
        }

        [Fact]
        public void PersonShouldHaveEmptyButNotNullContacts()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Guid.Empty);
            company.Persons.ShouldBeEmpty();
            company.Persons.ShouldNotBeNull();
        }

        [Fact]
        public async Task ClientShouldBeAbleToGenerateMultiplePlaceholders()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Guid.Empty);
            int count = 1000;
            
            var tasks = new List<Task<Tuple<Guid, Guid>>>(); 
            // Using Tuple to store result and loop index

            for (int i = 0; i < count; i++)
            {
                var task = company.AddMemberAsync(
                    "N/A", 
                    "N/A", 
                    "N/A",
                    "N/A",
                    Guid.NewGuid().ToString(),
                    $"{Guid.NewGuid().ToString()}@tuldok.co", 
                    "N/A",
                    "N/A",
                    "{\n  \"Facebook\": \"N/A\",\n  \"LinkedIn\": \"N/A\",\n  \"Pinterest\": \"N/A\",\n  \"Instagram\": \"N/A\",\n  \"Twitter\": \"N/A\"\n}");

                tasks.Add(task.ContinueWith(resultTask => Tuple.Create(resultTask.Result.Id, resultTask.Result.Card.Id)));
            }

            await Task.WhenAll(tasks);
            
            company.Persons.ShouldNotBeEmpty();
            company.Persons.Count.ShouldBe(1000);
            company.Persons.All(p => p.Card.HasKey()).ShouldBeFalse();
            company.Persons.All(p => p.Card.Key == string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void ClientCanBeDiscreet()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, Guid.Empty);
            company.IsDiscreet = false;

            company.IsDiscreet.ShouldBeFalse();
        }

    }
}
