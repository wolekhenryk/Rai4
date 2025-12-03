using Rai4.Application.Dto.Json;

namespace Rai4.Application.Services.Interfaces;

public interface IZtmClient
{
    Task<List<FriendlyStop>> GetAllBusStopsAsync(CancellationToken cancellationToken = default);
    Task<StopDepartures> GetStopDeparturesAsync(int stopId, CancellationToken cancellationToken = default);
}