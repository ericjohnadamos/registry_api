namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers", "users");
        builder.HasKey("Id");
        builder.Property(_ => _.Key).IsRequired(false);
        builder.Property(_ => _.Name).IsRequired(false);
        builder.Property(_ => _.TimeZoneId).IsRequired(false);
    }
}