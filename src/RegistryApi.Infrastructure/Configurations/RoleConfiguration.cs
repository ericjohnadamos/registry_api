namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleConfiguration
    : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("my_aspnet_roles");
        builder.HasKey(_ => _.Id);
        builder
            .Property(_ => _.ApplicationId)
            .HasColumnName("applicationId");
        builder
            .Property(_ => _.Name)
            .HasColumnName("name");
    }
}
