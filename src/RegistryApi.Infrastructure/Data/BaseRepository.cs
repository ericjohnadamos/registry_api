namespace RegistryApi.Infrastructure.Data;

using RegistryApi.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class BaseRepository<T> : IRepository<T>
    where T : class
{
    private readonly DbContext dbContext;

    public BaseRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<T> GetAllAsQueryable()
    {
        return this.dbContext.Set<T>().AsQueryable();
    }

    public async Task AddAsync(T newItem)
    {
        await this.dbContext.AddAsync(newItem).ConfigureAwait(false);
    }

    public async Task<T> FindAsync(int id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await this.dbContext.FindAsync<T>(id).ConfigureAwait(false);
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task<T?> FirstOrDefaultAsync(Specification<T> specification)
    {
        return await this.dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(specification.ToExpression());
    }

    public async Task<IReadOnlyCollection<T>> GetAllAttachedAsync()
    {
        return await this.dbContext.Set<T>().ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetAllDetachedAsync()
    {
        var query = this.dbContext.Set<T>().AsNoTracking();
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetAttachedAsync(Specification<T> specification)
    {
        var matched = this.dbContext.Set<T>().AsTracking().Where(specification.ToExpression());
        return await matched.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetAttachedAsync(
        Specification<T> specification, string navigationPropertyPath)
    {
        var matched = this.dbContext
            .Set<T>()
            .AsTracking()
            .Include(navigationPropertyPath)
            .Where(specification.ToExpression());
        return await matched.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetDetachedAsync(Specification<T> specification)
    {
        var query = this.dbContext.Set<T>().AsNoTracking().Where(specification.ToExpression());
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetDetachedAsync(
        Specification<T> specification, string navigationPropertyPath)
    {
        var query = this.dbContext
            .Set<T>()
            .AsNoTracking()
            .Include(navigationPropertyPath)
            .Where(specification.ToExpression());
        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<int> SaveChangesAsync(T entity)
    {
        if (   !this.dbContext.Set<T>().Local.Any(_ => _ != entity)
            && this.dbContext.Entry(entity).State == EntityState.Detached)
            this.dbContext.Set<T>().Attach(entity);
        return await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public void UpdateAsync(T updatedItem)
    {
        this.dbContext.Update(updatedItem);
    }
}
