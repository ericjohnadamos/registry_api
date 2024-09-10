namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UsersInRoleConfiguration
     : IEntityTypeConfiguration<UsersInRole>
{
    public void Configure(EntityTypeBuilder<UsersInRole> builder)
    {
        builder.ToTable("my_aspnet_usersinroles");
        builder.HasKey(_ => new { _.UserId, _.RoleId });
        builder
            .HasOne(u => u.User)
            .WithMany(u => u.UsersInRole)
            .HasForeignKey(u => u.UserId);
        builder
            .HasOne(u => u.Role)
            .WithMany(u => u.UsersInRole)
            .HasForeignKey(u => u.RoleId);
    }
}
