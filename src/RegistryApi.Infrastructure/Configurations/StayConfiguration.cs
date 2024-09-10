namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.CustomerGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class StayConfiguration : IEntityTypeConfiguration<Stay>
{
    public void Configure(EntityTypeBuilder<Stay> builder)
    {
        builder.ToTable("stay");
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.WaitListDateTime)
            .IsRequired(false)
            .HasColumnName("waitListTime");
        builder.Property(_ => _.CancelledDateTime)
            .IsRequired(false)
            .HasColumnName("cancelledTime");
        builder.Property(_ => _.CheckInDateTime)
            .IsRequired(false)
            .HasColumnName("checkIn");
        builder.Property(_ => _.CheckOutDateTime)
            .IsRequired(false)
            .HasColumnName("checkOut");
        builder.Property(_ => _.ExpectedCheckOut)
            .IsRequired(false)
            .HasColumnName("expectedCheckOut");
        builder.Property(_ => _.RequestedCheckIn)
            .IsRequired(false)
            .HasColumnName("requesttime");

        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Stays);
    }
}