namespace RegistryApi.Infrastructure.Data;

using RegistryApi.SharedKernel;

public class RegistryRepository<T> : BaseRepository<T>, IRegistryRepository<T>
    where T : class, IEntity
{
    public RegistryRepository(RegistryContext context)
        : base(context)
    {
    }
}
