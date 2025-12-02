using Microsoft.EntityFrameworkCore;
using Rai4.Domain.Models;
using Rai4.Infrastructure.Data;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Infrastructure.Repositories.Implementation;

public class BusStopRepository(AppDbContext dbContext) : Repository<BusStop>(dbContext), IBusStopRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<BusStop>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<BusStop>()
            .Where(bs => bs.UserId == userId)
            .Include(bs => bs.User)
            .ToListAsync(cancellationToken);
    }

    public Task<List<int>> GetByUserIdsAsync(List<int> userIds, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<BusStop>()
            .AsNoTracking()
            .Where(bs => userIds.Contains(bs.UserId))
            .Select(bs => bs.ZtmStopId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<BusStop?> GetByIdWithUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<BusStop>()
            .Include(bs => bs.User)
            .FirstOrDefaultAsync(bs => bs.Id == id, cancellationToken);
    }
}
