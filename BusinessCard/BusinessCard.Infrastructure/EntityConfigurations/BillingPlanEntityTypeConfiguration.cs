using BusinessCard.Domain.AggregatesModel.ClientSubscriptionAggregate;
using BusinessCard.Domain.AggregatesModel.SubscriptionAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class BillingPlanEntityTypeConfiguration : IEntityTypeConfiguration<BillingPlan>
{
    public void Configure(EntityTypeBuilder<BillingPlan> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Name).IsUnique();
        builder.Property(b => b.Name).IsRequired();

        builder.HasData(BillingPlan.Plans);
    }
}