using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

public class BillingPlanEntityTypeConfiguration : IEntityTypeConfiguration<BillingPlan>
{
    public void Configure(EntityTypeBuilder<BillingPlan> builder)
    {
        builder.ToTable("billingplan", LokiContext.DefaultSchema);
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Name).IsUnique();
        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Name).HasMaxLength(50);
        
        builder.HasData(BillingPlan.SeededValue);
    }
}