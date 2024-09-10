namespace RegistryApi.SharedKernel;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IReadOnlyRepository<T>
{
    IQueryable<T> GetAllAsQueryable();
    Task<IReadOnlyCollection<T>> GetAttachedAsync(Specification<T> specification);
    Task<IReadOnlyCollection<T>> GetAttachedAsync(Specification<T> specification, string navigationPropertyPath);
    Task<IReadOnlyCollection<T>> GetDetachedAsync(Specification<T> specification);
    Task<IReadOnlyCollection<T>> GetDetachedAsync(Specification<T> specification, string navigationPropertyPath);
    Task<IReadOnlyCollection<T>> GetAllDetachedAsync();
    Task<IReadOnlyCollection<T>> GetAllAttachedAsync();
    Task<T> FindAsync(int id);
    Task<T?> FirstOrDefaultAsync(Specification<T> specification);
}

public interface IRepository<T> : IReadOnlyRepository<T>
{
    Task AddAsync(T newItem);
    void UpdateAsync(T updatedItem);
    Task<int> SaveChangesAsync(T entity);
}
