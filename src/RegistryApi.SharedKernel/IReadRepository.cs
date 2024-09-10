namespace RegistryApi.SharedKernel;

using System;
using System.Linq;
using System.Security.Claims;

public interface IReadRepository<T> : IDisposable
{
    IQueryable<T> Query(ClaimsPrincipal user);
}
