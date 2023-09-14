using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscription", LokiContext.DefaultSchema);
        builder.HasKey(b => b.Id);


        builder.Property<Guid>("_billingPlanId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BillingPlanId")
            .IsRequired();

        builder.HasOne<BillingPlan>()
            .WithMany()
            .HasForeignKey("_billingPlanId")
            .OnDelete(DeleteBehavior.Restrict);
        
        
        builder.OwnsOne(b => b.Setting, setting =>
        {
            setting.ToTable("cardsetting", LokiContext.DefaultSchema);
            setting.Property(b => b.Level).IsRequired();
            setting.Property(b => b.ExpiresInMonths).IsRequired().HasDefaultValue(12);
            setting.Property(b => b.Description).HasMaxLength(50);
        });
        
        
    }
}