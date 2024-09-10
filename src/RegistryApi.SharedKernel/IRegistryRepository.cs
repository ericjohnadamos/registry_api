namespace RegistryApi.SharedKernel;

public interface IRegistryRepository<T> : IRepository<T> where T : class
{
}
