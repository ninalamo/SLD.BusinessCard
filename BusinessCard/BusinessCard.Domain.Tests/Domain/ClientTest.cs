using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Faker.Resources;
using Shouldly;
using CompanyFaker = Faker.Company;
using NameFaker = Faker.Name;

namespace BusinessCard.Domain.Tests.Domain;

public class ClientTest
{
   
    private Client CreateClient() =>new(CompanyFaker.Name(), "industry");

    [Fact]
    public void ClientShouldBeCreated()
    {
        string companyName = CompanyFaker.Name();
        Client company = new(companyName, "industry");
        company.Name.ShouldBe(companyName);
        company.Industry.ShouldBe("industry");
    }

    [Fact]
    public void ClientShouldBeAbleToRename()
    {
        var company = CreateClient();
        company.Name = "GMA";
        company.Name.ShouldBe("GMA");
    }
    
       
    [Fact]
    public void ClientSubscriptionListShouldNotBeNull()
    {
        Client client = CreateClient();
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

        Client client = CreateClient();
        var addedSubscription = client.AddSubscription(
            subscription.GetBillingPlanId(), 
            subscription.StartDate, 
            subscription.EndDate.Month - subscription.StartDate.Month);
            
        addedSubscription.StartDate.Date.ShouldBe(subscription.StartDate.Date);
            
    }

}