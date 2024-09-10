namespace RegistryApi.Infrastructure.Data;

using System.Linq;
using System.Security.Claims;
using RegistryApi.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class RegistryContextFactory
{
    private readonly IConfiguration configuration;

    public RegistryContextFactory(IConfiguration configuration) => this.configuration = configuration;

    public RegistryContext CreateContext(ClaimsPrincipal principal)
    {
        var connectionString = string.Format(this.configuration["ConnectionStrings:Registry"],
            principal.Claims.SingleOrDefault(_ => _.Type == ApiUserClaimTypes.CustomerKey)?.Value,
            principal.Claims.SingleOrDefault(_ => _.Type == ApiUserClaimTypes.Username)?.Value,
            principal.Claims.SingleOrDefault(_ => _.Type == ApiUserClaimTypes.Password)?.Value);
        return new RegistryContext(
            new DbContextOptionsBuilder<RegistryContext>()
                .UseMySQL(connectionString)
                .Options);
    }
}