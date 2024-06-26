using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class BillingPlan : Enumeration
{
    public static IEnumerable<BillingPlan> SeededValue = new BillingPlan[]
    {
        new (Guid.Parse("00000000-0000-0000-0000-000000000001"), "Free Trial"),
        new (Guid.Parse("00000000-0000-0000-0000-000000000002"), "Monthly"),
        new (Guid.Parse("00000000-0000-0000-0000-000000000003"), "Yearly"),
    };
    
    public BillingPlan(Guid id, string name) : base(id, name)
    {
    }
}