using Rai4.Domain.Models.Base;

namespace Rai4.Infrastructure.Repositories.Interfaces;

public interface IRepository<T> where T : BaseDbEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdWithNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByPredicateAsync(Func<IQueryable<T>, IQueryable<T>>? queryShaper = null, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
}