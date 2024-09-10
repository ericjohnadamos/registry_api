namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.CustomerGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CustomerOptOutConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customer");
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.ExcludeFromApi)
            .HasConversion(new BoolToShortConverter())
            .HasColumnName("optOutApi");
        builder.Property(_ => _.Name)
            .HasColumnName("name");
        builder.Property(_ => _.PrimaryPhone)
            .HasColumnName("primaryPhone");
        builder.Property(_ => _.PrimaryPhoneDescription)
            .HasColumnName("primaryPhoneDescription");
    }
}