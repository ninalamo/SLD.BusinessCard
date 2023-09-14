using BusinessCard.Domain.AggregatesModel.CardSettingAggregate;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("clientsubscription", LokiContext.DefaultSchema);
        builder.HasKey(b => b.Id);


        builder.Property<Guid>("_billingPlanId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BillingPlanId")
            .IsRequired();

        builder.HasOne<BillingPlan>()
            .WithMany()
            .HasForeignKey("_billingPlanId")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property<Guid>("_clientId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ClientId")
            .IsRequired();

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey("_clientId")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property<Guid>("_configId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ConfigId")
            .IsRequired();

        builder.HasOne<CardSetting>()
            .WithMany()
            .HasForeignKey("_configId")
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
    }
}