namespace RegistryApi.Infrastructure.Data;

using RegistryApi.Core.Shared;
using RegistryApi.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

public class GlobalRegistryContext : DbContext
{
    public GlobalRegistryContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Language> Language { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // should probably move this to a shared context if possible
        modelBuilder.ApplyConfiguration(new LanguageConfiguration());
    }
}
