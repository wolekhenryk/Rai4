using Microsoft.EntityFrameworkCore.Storage;
using Rai4.Infrastructure.Data;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Infrastructure.Repositories.Implementation;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        dbContext.Database.BeginTransactionAsync(cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}