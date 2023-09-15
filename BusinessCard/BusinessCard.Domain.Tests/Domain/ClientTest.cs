using BusinessCard.Domain.AggregatesModel.ClientAggregate;
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
        public void ClientCanBeDiscreet()
        {
            string companyName = CompanyFaker.Name();
            Client company = new(companyName, false, "");
            company.IsDiscreet = false;
            company.IsDiscreet.ShouldBeFalse();
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
