namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.CustomerGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CustomerLanguageConfiguration : IEntityTypeConfiguration<CustomerLanguage>
{
    public void Configure(EntityTypeBuilder<CustomerLanguage> builder)
    {
        builder.ToTable("customerlanguages");

        builder.HasKey(_ => new { _.CustomerId, _.LanguageId });

        builder.Property(_ => _.CustomerId)
            .HasColumnName("customerId")
            .IsRequired();

        builder.Property(_ => _.LanguageId)
            .HasColumnName("languageId")
            .IsRequired();

        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Languages)
            .HasForeignKey(_ => _.CustomerId);
    }
}