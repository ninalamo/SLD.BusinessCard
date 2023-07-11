using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class CardEntityTypeConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("card", LokiContext.DEFAULT_SCHEMA);
        builder.HasKey(b => b.Id);
        builder.Property(b => b.IsActive).HasDefaultValue(false);
    }
}

internal class MemberTierEntityTypeConfiguration : IEntityTypeConfiguration<MemberTier>
{
    public void Configure(EntityTypeBuilder<MemberTier> builder)
    {
        builder.ToTable("membertier", LokiContext.DEFAULT_SCHEMA);
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Level).IsUnique();
        builder.Property(b => b.Level).IsRequired();
        builder.HasIndex(b => b.Name).IsUnique();

        builder.HasData(MemberTier.GetLevels());
    }
}