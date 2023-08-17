using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("client", LokiContext.DEFAULT_SCHEMA);
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CompanyName).IsRequired();
        builder.HasIndex(b => b.CompanyName).IsUnique();

        builder.Metadata
            .FindNavigation(nameof(Client.Persons))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Property<Guid>("_memberTierId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("MemberTierId")
            .IsRequired();

        builder.HasOne<MemberTier>()
            .WithMany()
            .HasForeignKey("_memberTierId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}