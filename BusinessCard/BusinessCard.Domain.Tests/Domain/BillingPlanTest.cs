using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Shouldly;

namespace BusinessCard.Tests.Domain;

public class BillingPlanTest
{
    [Fact]
    public void CanCreateBilingPlan()
    {
        var id = Guid.NewGuid();
        var name = Faker.Company.Name();
        
        BillingPlan plan = new BillingPlan(id,name);
        plan.ShouldNotBeNull();
        plan.Id.ShouldNotBe(Guid.Empty);
        plan.Name.ShouldBe(name);

    }
}