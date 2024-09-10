namespace RegistryApi.Infrastructure.Data;

using RegistryApi.Core.CustomerGroup;
using RegistryApi.Core.Messages;
using RegistryApi.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

public class RegistryContext : DbContext
{
    public RegistryContext(DbContextOptions<RegistryContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Stay> Stays { get; set; } = null!;
    public DbSet<IncomingMessage> IncomingMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerOptOutConfiguration());
        modelBuilder.ApplyConfiguration(new StayConfiguration());
        modelBuilder.ApplyConfiguration(new GuestConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerLanguageConfiguration());
        modelBuilder.ApplyConfiguration(new ApiIntegrationConfiguration());
    }
}