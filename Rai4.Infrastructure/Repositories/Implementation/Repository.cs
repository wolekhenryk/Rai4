using Microsoft.EntityFrameworkCore;
using Rai4.Domain.Models.Base;
using Rai4.Infrastructure.Data;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Infrastructure.Repositories.Implementation;

public class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : BaseDbEntity
{
    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<T?> GetByIdWithNoTrackingAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Set<T>().ToListAsync(cancellationToken);

    public Task<T?> GetByPredicateAsync(Func<IQueryable<T>, IQueryable<T>>? queryShaper = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbContext.Set<T>();

        if (queryShaper != null)
            query = queryShaper(query);

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        dbContext.Set<T>().AddAsync(entity, cancellationToken).AsTask();

    public void Update(T entity) => dbContext.Set<T>().Update(entity);

    public void Remove(T entity) => dbContext.Set<T>().Remove(entity);
}