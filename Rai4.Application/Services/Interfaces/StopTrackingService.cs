using Microsoft.AspNetCore.SignalR;
using Rai4.Application.Dto.Json;
using Rai4.Application.WebSockets;

namespace Rai4.Application.Services.Interfaces;

public class StopTrackingService(IHubContext<BusStopHub, IBusStopClient> hubContext) : IStopTrackingService
{
    public Task BroadcastStopSchedule(int stopId, string stopName, StopDepartures stopDeparture, CancellationToken cancellationToken = default)
    {
        return hubContext.Clients
            .Group($"stop_{stopId}")
            .ReceiveStopUpdatesAsync(stopId, stopName, stopDeparture);
    }
}