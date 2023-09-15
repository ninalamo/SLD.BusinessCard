﻿using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Faker.Resources;
using Shouldly;
using CompanyFaker = Faker.Company;
using NameFaker = Faker.Name;

namespace BusinessCard.Domain.Tests.Domain
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
        public async Task ClientShouldBeAbleToGenerateMultiplePlaceholders()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            int count = 1000;
            
            // for (int i = 0; i < count; i++)
            // {
            //     company.AddMember(
            //         "N/A", 
            //         "N/A", 
            //         "N/A",
            //         "N/A",
            //         Guid.NewGuid().ToString(),
            //         $"{Guid.NewGuid().ToString()}@tuldok.co", 
            //         "N/A",
            //         "N/A",
            //         "{\n  \"Facebook\": \"N/A\",\n  \"LinkedIn\": \"N/A\",\n  \"Pinterest\": \"N/A\",\n  \"Instagram\": \"N/A\",\n  \"Twitter\": \"N/A\"\n}");
            // }

        
            // company.Persons.ShouldNotBeEmpty();
            // company.Persons.Count.ShouldBe(1000);
        }
        
        [Fact]
        public async Task CanAddMemberUsingEntityObject()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            int count = 1000;

            for (int i = 0; i < count; i++)
            {
                var person = new Person("N/A",
                        "N/A",
                        "N/A",
                        "N/A",
                        Guid.NewGuid().ToString(),
                        $"{Guid.NewGuid().ToString()}@tuldok.co",
                        "N/A",
                        "N/A",
                        "{\n  \"Facebook\": \"N/A\",\n  \"LinkedIn\": \"N/A\",\n  \"Pinterest\": \"N/A\",\n  \"Instagram\": \"N/A\",\n  \"Twitter\": \"N/A\"\n}");
    
                // company.AddMember(person);
            }
            
            // company.Persons.ShouldNotBeEmpty();
            // company.Persons.Count.ShouldBe(1000);
        }

        [Fact]
        public void ClientCanBeDiscreet()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            company.IsDiscreet = false;
            company.IsDiscreet.ShouldBeFalse();
        }

        [Fact]
        public void ClientCanAddMembers()
        {
            Client company = new(CompanyFaker.Name(), false, "");
            company.IsDiscreet = false;
            company.IsDiscreet.ShouldBeFalse();
            
            // company.AddMember(NameFaker.First(), NameFaker.Last(), NameFaker.Last(), NameFaker.Last(),
            //     Faker.Phone.Number(), Internet.FreeMail, Address.Country, "", "");

            // Person person = company.Persons.FirstOrDefault();
            //
            // company.Persons.ShouldNotBeEmpty();
            // company.Persons.FirstOrDefault().ShouldBeEquivalentTo(person);

        }

        [Fact]
        public void ClientPersonListShouldNotBeNull()
        {
            Client client = new(CompanyFaker.Name(), false, "");
            // client.Persons.ShouldNotBeNull();
            // client.Persons.ShouldBeEmpty();
        }
        
        [Fact]
        public void ClientSubscriptionListShouldNotBeNull()
        {
            Client client = new(CompanyFaker.Name(), false, "");
            client.Subscriptions.ShouldNotBeNull();
            client.Subscriptions.ShouldBeEmpty();
        }

        [Fact]
        public void CanAddSubscription()
        {
            Subscription subscription = new(
                billingPlanId: Guid.NewGuid(),
                startDate: DateTime.Today,
                DateTime.Today.AddMonths(Faker.RandomNumber.Next(1, 12)));
            
            Client client = new(CompanyFaker.Name(), false, "");
            var addedSubscription = client.AddSubscription(subscription.GetBillingPlanId(), 
                subscription.StartDate, subscription.EndDate.Month - subscription.StartDate.Month);
            
            addedSubscription.StartDate.Date.ShouldBe(subscription.StartDate.Date);
            
        }

    }
}
