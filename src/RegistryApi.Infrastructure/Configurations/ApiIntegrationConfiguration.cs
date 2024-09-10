namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.ApiIntegrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class ApiIntegrationConfiguration
     : IEntityTypeConfiguration<ApiIntegration>
{
    public void Configure(EntityTypeBuilder<ApiIntegration> builder)
    {
        builder.ToTable("apiintegrations");
        builder.HasKey(_ => _.Id);
        builder
            .Property(_ => _.CustomerId)
            .HasColumnName("customerid");
        builder
            .Property(_ => _.Username)
            .HasColumnName("username");
        builder
            .Property(_ => _.Password)
            .HasColumnName("password");
        builder
            .Property(_ => _.Md5WebhookSecret)
            .HasColumnName("md5_webhooksecret");
    }
}
