namespace RegistryApi.Infrastructure.Configurations;

using RegistryApi.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("languages", "registryglobal");

        builder.HasKey(_ => _.Id);

        builder
            .Property(_ => _.Abbreviation)
            .HasColumnName("abbreviation")
            .IsRequired(false);

        builder
            .Property(_ => _.Name)
            .HasColumnName("name")
            .IsRequired(false);
    }
}
