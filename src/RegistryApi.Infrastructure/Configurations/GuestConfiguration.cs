namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.CustomerGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("guest");
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.FirstName)
            .HasColumnName("firstName");
        builder.Property(_ => _.LastName)
            .HasColumnName("lastName");
        builder.Property(_ => _.MiddleName)
            .HasColumnName("middleName");
        builder.Property(_ => _.Honorific)
            .HasColumnName("honorific");
        builder.Property(_ => _.Phone)
            .HasColumnName("phone");
        builder.Property(_ => _.Phone2)
            .HasColumnName("phone2");
        builder.Property(_ => _.PhoneIsPrimary)
            .HasColumnName("phoneIsPrimary");
        builder.Property(_ => _.Phone2IsPrimary)
            .HasColumnName("phone2IsPrimary");
        builder.Property(_ => _.Phone2Description)
            .HasColumnName("phone2Description");
        builder.Property(_ => _.Email)
            .HasColumnName("email");
        builder.Property("RelationshipId")
            .HasColumnName("relationshipId");
        builder.Property(_ => _.IsGuardian)
            .HasConversion(new BoolToShortConverter())
            .HasColumnName("isguardian");

        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Guests);

        builder.Ignore(_ => _.IsPatient);
    }
}