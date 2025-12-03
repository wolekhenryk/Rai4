using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rai4.Application.Services.Interfaces;
using Rai4.Domain.Models;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Background;

public class BackgroundSignalRPush(
    IConnectedClientManager connectedClientManager,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            
            var clientIds = connectedClientManager.GetConnectedClients().ToList();
            if (clientIds.Count == 0)
                continue;
            
            var busStops = await GetBusStopsForUsersAsync(clientIds, stoppingToken);
            if (busStops.Count == 0)
                continue;
            
            await PushToClientsAsync(busStops, stoppingToken);
        }
    }

    private async Task<List<BusStop>> GetBusStopsForUsersAsync(List<int> users, CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var busStopRepository = scope.ServiceProvider.GetRequiredService<IBusStopRepository>();
        var stopIds = await busStopRepository.GetByUserIdsAsync(users, stoppingToken);
        return stopIds;
    }

    private async Task PushToClientsAsync(List<BusStop> busStops, CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var ztmClient = scope.ServiceProvider.GetRequiredService<IZtmClient>();
        var stopTrackingService = scope.ServiceProvider.GetRequiredService<IStopTrackingService>();
        
        await Parallel.ForEachAsync(busStops, stoppingToken, async (stop, _) =>
        {
            var stopId = stop.ZtmStopId;
            var stopName = stop.Name;
            var schedule = await ztmClient.GetStopDeparturesAsync(stopId, stoppingToken);
            await stopTrackingService.BroadcastStopSchedule(stopId, stopName, schedule, stoppingToken);
        });
    }
}