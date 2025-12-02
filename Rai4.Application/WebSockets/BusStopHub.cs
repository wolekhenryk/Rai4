using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Rai4.Application.Services.Interfaces;
using Rai4.Domain.Models;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.WebSockets;

public class BusStopHub(
    IConnectedClientManager connectedClientManager,
    IBusStopRepository busStopRepository,
    IUnitOfWork unitOfWork) : Hub<IBusStopClient>
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? throw new Exception("User ID not found in claims.");

        connectedClientManager.AddClient(int.Parse(userId));

        // Get user's tracked stops from database
        var busStops = await busStopRepository.GetByUserIdAsync(int.Parse(userId));

        // Add to group for each tracked stop
        foreach (var busStop in busStops)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"stop_{busStop.ZtmStopId}");
        }

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? throw new Exception("User ID not found in claims.");
        connectedClientManager.RemoveClient(int.Parse(userId));
        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Start tracking a bus stop
    /// </summary>
    /// <param name="ztmStopId">The ZTM stop ID to track</param>
    /// <param name="stopName">The name of the stop</param>
    public async Task TrackStop(int ztmStopId, string stopName)
    {
        var userId = int.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Add to database
        var busStop = new BusStop
        {
            UserId = userId,
            ZtmStopId = ztmStopId,
            Name = stopName,
            CreatedAtUtc = DateTime.UtcNow
        };

        await busStopRepository.AddAsync(busStop);
        await unitOfWork.SaveChangesAsync();

        // Add to SignalR group
        await Groups.AddToGroupAsync(Context.ConnectionId, $"stop_{ztmStopId}");
    }

    /// <summary>
    /// Stop tracking a bus stop
    /// </summary>
    /// <param name="ztmStopId">The ZTM stop ID to untrack</param>
    public async Task UntrackStop(int ztmStopId)
    {
        var userId = int.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Remove from database
        var busStop = await busStopRepository.GetByPredicateAsync(
            query => query.Where(bs => bs.UserId == userId && bs.ZtmStopId == ztmStopId));

        if (busStop != null)
        {
            busStopRepository.Remove(busStop);
            await unitOfWork.SaveChangesAsync();
        }

        // Remove from SignalR group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"stop_{ztmStopId}");
    }
}