using Rai4.Domain.Models;

namespace Rai4.Infrastructure.Repositories.Interfaces;

public interface IBusStopRepository : IRepository<BusStop>
{
    Task<List<BusStop>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<int>> GetByUserIdsAsync(List<int> userIds, CancellationToken cancellationToken = default);
    Task<BusStop?> GetByIdWithUserAsync(int id, CancellationToken cancellationToken = default);
}
