namespace RegistryApi.Infrastructure.Data;

using RegistryApi.Core.Users;
using RegistryApi.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IUserRepository<T> : IRepository<T>
{
    Task SetupNewMySqlUser(User user, string unhashedPassword);
}

public class UserRepository<T> : IUserRepository<T>
    where T : class, IEntity
{
    private readonly DbSet<T> _dbSet;

    public UserRepository(UsersContext context)
    {
        Context = context;
        _dbSet = Context.Set<T>();
    }

    protected UsersContext Context { get; }

    public virtual IQueryable<T> GetAllAsQueryable()
        => this.Context.Set<T>().AsQueryable();

    public virtual async Task AddAsync(T newItem)
        => await Context.AddAsync(newItem).ConfigureAwait(false);

    public virtual async Task<IReadOnlyCollection<T>> GetAttachedAsync(Specification<T> specification)
    {
        var matched = _dbSet.AsTracking()
            .Where(specification.ToExpression());

        return await matched.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAttachedAsync(
        Specification<T> specification, string navigationPropertyPath)
    {
        var matched = _dbSet.AsTracking()
            .Include(navigationPropertyPath)
            .Where(specification.ToExpression());

        return await matched.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetDetachedAsync(Specification<T> specification)
    {
        var query = _dbSet.AsNoTracking()
            .Where(specification.ToExpression());
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetDetachedAsync(
        Specification<T> specification, string navigationPropertyPath)
    {
        var query = _dbSet.AsNoTracking()
            .Include(navigationPropertyPath)
            .Where(specification.ToExpression());
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllDetachedAsync()
    {
        var query = _dbSet.AsNoTracking();
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAttachedAsync()
    {
        var query = _dbSet;
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<T?> FirstOrDefaultAsync(Specification<T> specification)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(specification.ToExpression());
    }

    public async Task<T> FindAsync(int id)
#pragma warning disable CS8603 // Possible null reference return.
        => await Context.FindAsync<T>(id).ConfigureAwait(false);
#pragma warning restore CS8603 // Possible null reference return.

    public void UpdateAsync(T updatedItem)
        => _dbSet.Update(updatedItem);

    public async Task<int> SaveChangesAsync(T entity)
    {
        // ReSharper disable once SimplifyLinqExpression, content with linq expression
        if (!_dbSet.Local.Any(_ => _ != entity) && Context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        return await Context.SaveChangesAsync()
            .ConfigureAwait(false);
    }

    public async Task SetupNewMySqlUser(User user, string unhashedPassword)
        => await Context.SetupNewMySqlUser(user, unhashedPassword).ConfigureAwait(false);
}