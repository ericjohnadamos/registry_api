namespace RegistryApi.Infrastructure.Data;

using RegistryApi.Core.ApiIntegrations;
using RegistryApi.Core.Customers;
using RegistryApi.Core.Roles;
using RegistryApi.Core.Users;
using RegistryApi.Infrastructure.Configurations;
using RegistryApi.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UsersContext : DbContext
{
    private readonly IMediator mediator;
    private readonly ILogger<UsersContext> logger;

    public UsersContext(DbContextOptions<UsersContext> options,
        IMediator mediator,
        ILogger<UsersContext> logger)
        : base(options)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Customer> Customers { get; set; } = null!;

    public DbSet<ApiIntegration> ApiIntegrations { get; set; } = null!;

    public DbSet<UsersInRole> UsersInRoles { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;

    public async Task SetupNewMySqlUser(User user, string unhashedPassword)
    {
        this.logger.LogInformation($"Setting up new MySql user for username: {user.Username}.");

        var customer = await Customers.SingleOrDefaultAsync(_ => _.Id == user.CustomerId);
        if (customer == null)
        {
            this.logger.LogError($"Cannot set up MySql user for username: {user.Username}. Customer with id: {user.CustomerId} does not exist.");
            return;
        }

        using (var command = Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = $"select count(*) from mysql.user where upper(user) = '{user.Username.ToUpper()}' and host = '%';";
            Database.OpenConnection();
            var result = Convert.ToInt32(await command.ExecuteScalarAsync());
            if (result <= 0)
            {
                using (var createUserCommand = Database.GetDbConnection().CreateCommand())
                {
                    this.logger.LogInformation($"Creating MySql user: {user.Username}...");

                    createUserCommand.CommandText = "CREATE USER @user IDENTIFIED WITH mysql_native_password BY @password";
                    var userParameter = createUserCommand.CreateParameter();
                    userParameter.Direction = ParameterDirection.Input;
                    userParameter.DbType = DbType.String;
                    userParameter.ParameterName = "@user";
                    userParameter.Value = user.Username;
                    createUserCommand.Parameters.Add(userParameter);
                    var passwordParameter = createUserCommand.CreateParameter();
                    passwordParameter.Direction = ParameterDirection.Input;
                    passwordParameter.DbType = DbType.String;
                    passwordParameter.ParameterName = "@password";
                    passwordParameter.Value = unhashedPassword;
                    createUserCommand.Parameters.Add(passwordParameter);

                    await createUserCommand.ExecuteNonQueryAsync();
                    this.logger.LogInformation($"MySql user: {user.Username} was created successfully.");
                }

                using (var grantAccessCommand = Database.GetDbConnection().CreateCommand())
                {
                    this.logger.LogInformation($"Preparing to grant access to registry{customer.Key} for user: {user.Username}.");

                    // Hack: Cant get syntax right for parameterized SQL.
                    grantAccessCommand.CommandText = "GRANT ALL ON registry" + customer.Key + ".* TO '" + user.Username.Replace("--", string.Empty).Replace("'", "''") + "'@'%'";
                    await grantAccessCommand.ExecuteNonQueryAsync();

                    this.logger.LogInformation($"Access granted to registry{customer.Key} for user: {user.Username}.");

                    grantAccessCommand.CommandText = "GRANT SELECT ON registryglobal.* TO '" + user.Username.Replace("--", string.Empty).Replace("'", "''") + "'@'%'";
                    await grantAccessCommand.ExecuteNonQueryAsync();

                    this.logger.LogInformation($"Select access granted to registryglobal for user: {user.Username}.");
                }
            }
            else this.logger.LogError($"Unable to create MySql user: {user.Username}. A user with that username already exists.");
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var stateEntries = await base.SaveChangesAsync(cancellationToken);
        var entitiesWithEvents = ChangeTracker.Entries<DomainEntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();
        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var domainEvent in domainEvents)
            {
                await this.mediator.Publish(domainEvent, CancellationToken.None);
                this.logger.LogInformation($"{domainEvent.GetType().Name} published successfully.");
            }
        }

        return stateEntries;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new UsersInRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}