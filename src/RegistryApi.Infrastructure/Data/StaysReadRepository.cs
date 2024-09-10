namespace RegistryApi.Infrastructure.Data;

using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RegistryApi.Core;
using RegistryApi.Core.CustomerGroup;
using RegistryApi.SharedKernel;
using Microsoft.EntityFrameworkCore;

public class StaysReadRepository : IReadRepository<Stay>
{
    private readonly RegistryContext dbContext;

    public StaysReadRepository(RegistryContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<Stay> Query(ClaimsPrincipal user)
    {
        return this.dbContext.Stays
            .AsNoTracking()
            .Include(stay => stay.Customer)
            .ThenInclude(customer => customer.Guests)
            .Include(stay => stay.Customer)
            .ThenInclude(customer => customer.Languages);
    }

    public async Task<AccountOptions> GetAccountOptions()
    {
        var connection = this.dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync().ConfigureAwait(false);
        }

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "select * from accountoptions";

        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow).ConfigureAwait(false);
        await reader.ReadAsync().ConfigureAwait(false);
        var accountOptions = new AccountOptions
        {
            ShouldDropOldRequestsOffWaitList = reader.GetBoolean("dropOldRequestsOffWaitList")
        };
        await connection.CloseAsync().ConfigureAwait(false);
        return accountOptions;
    }

    public void Dispose()
    {
        this.dbContext.Dispose();
    }
}