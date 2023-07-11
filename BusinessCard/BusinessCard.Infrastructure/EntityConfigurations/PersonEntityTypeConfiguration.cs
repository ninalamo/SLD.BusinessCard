﻿using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("people", LokiContext.DEFAULT_SCHEMA);
        builder.HasKey(b => b.Id);
        builder.Property(b => b.FirstName).IsRequired();
        builder.Property(b => b.LastName).IsRequired();
        builder.Property(b => b.PhoneNumber).IsRequired();
        builder.Property(b => b.Email).IsRequired();
        builder.Property(b => b.Address).IsRequired();
        builder.Property(b => b.Occupation).IsRequired();
        builder.Property(b => b.SocialMedia).IsRequired();

        builder.Property<Guid>("ClientId").IsRequired();
        
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