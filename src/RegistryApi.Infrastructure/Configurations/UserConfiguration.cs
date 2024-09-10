namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("apiusers", "users");
        builder.HasKey("Id");
        builder.Property("Id")
            .HasColumnName("id");
        builder.Property(_ => _.Username)
            .HasColumnName("username")
            .IsRequired();
        builder.Property(_ => _.Password)
            .HasColumnName("password")
            .IsRequired(false);
        builder.Property("CustomerId")
            .HasColumnName("customerid");
    }
}
