using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCard.Infrastructure.EntityConfigurations;

internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("people", LokiContext.DefaultSchema);
        builder.HasKey(b => b.Id);
        builder.Property(b => b.FirstName).IsRequired();
        builder.Property(b => b.LastName).IsRequired();
        
        builder.Property(b => b.PhoneNumber).IsRequired();
        builder.HasIndex(b => b.PhoneNumber).IsUnique();
        
        builder.Property(b => b.Email).IsRequired();
        builder.HasIndex(b => b.Email).IsUnique();
        
        builder.Property(b => b.Address).IsRequired();
        builder.Property(b => b.Occupation).IsRequired();
       
        builder.Property(b => b.IdentityUserId).HasDefaultValue(string.Empty);

        builder.Property<Guid>("ClientId").IsRequired();
        
        builder.Metadata
            .FindNavigation(nameof(Person.Cards))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(b => b.SocialMediaAccounts, soc =>
        {
            soc.ToTable("socialmedia", LokiContext.DefaultSchema);
            soc.Property(b => b.Facebook).HasMaxLength(56);
            soc.Property(b => b.Instagram).HasMaxLength(56);
            soc.Property(b => b.LinkedIn).HasMaxLength(56);
            soc.Property(b => b.Pinterest).HasMaxLength(56);
            soc.Property(b => b.Twitter).HasMaxLength(56);
        });


    }
}