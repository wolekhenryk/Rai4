using Rai4.Application.Dto.Json;

namespace Rai4.Application.Services.Interfaces;

public interface IStopTrackingService
{
    Task BroadcastStopSchedule(int stopId, StopDepartures stopDeparture,
        CancellationToken cancellationToken = default);
}