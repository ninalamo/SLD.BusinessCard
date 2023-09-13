using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("client", LokiContext.DefaultSchema);
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired();
        builder.HasIndex(b => b.Name).IsUnique();

        builder.Metadata
            .FindNavigation(nameof(Client.Persons))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        // builder.Property<Guid>("_subscriptionId")
        //     .UsePropertyAccessMode(PropertyAccessMode.Field)
        //     .HasColumnName("SubscriptionId")
        //     .IsRequired();
        //
        // builder.HasOne(b => b.MembershipTier)
        //     .WithMany()
        //     .HasForeignKey("_subscriptionId")
        //     .OnDelete(DeleteBehavior.NoAction);
    }
}