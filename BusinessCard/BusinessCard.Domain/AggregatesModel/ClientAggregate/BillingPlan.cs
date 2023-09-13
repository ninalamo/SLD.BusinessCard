using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.ClientSubscriptionAggregate;

public class BillingPlan : Enumeration
{
    public static IEnumerable<BillingPlan> Plans = new BillingPlan[]
    {
        new (Guid.Parse("00000000-0000-0000-0000-000000000001"), "Free Trial"),
        new (Guid.Parse("00000000-0000-0000-0000-000000000002"), "Monthly"),
        new (Guid.Parse("00000000-0000-0000-0000-000000000003"), "Yearly"),
    };
    
    public BillingPlan(Guid id, string name) : base(id, name)
    {
    }
}