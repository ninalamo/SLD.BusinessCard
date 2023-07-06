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