using Rai4.Application.Dto.BusStop;
using Rai4.Application.Dto.Json;

namespace Rai4.Application.WebSockets;

public interface IBusStopClient
{
    Task ReceiveStopUpdatesAsync(StopDepartures busLocationDto);
}