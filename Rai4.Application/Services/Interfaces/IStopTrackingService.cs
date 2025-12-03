using Rai4.Application.Dto.Json;

namespace Rai4.Application.Services.Interfaces;

public interface IStopTrackingService
{
    Task BroadcastStopSchedule(int stopId, string stopName, StopDepartures stopDeparture,
        CancellationToken cancellationToken = default);
}