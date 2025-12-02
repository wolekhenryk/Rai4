using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rai4.Application.Services.Interfaces;
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
            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            
            var clientIds = connectedClientManager.GetConnectedClients().ToList();
            if (clientIds.Count == 0)
                continue;
            
            var stopIds = await GetBusStopsForUsersAsync(clientIds, stoppingToken);
            if (stopIds.Count == 0)
                continue;
            
            await PushToClientsAsync(stopIds, stoppingToken);
        }
    }

    private async Task<List<int>> GetBusStopsForUsersAsync(List<int> users, CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var busStopRepository = scope.ServiceProvider.GetRequiredService<IBusStopRepository>();
        var stopIds = await busStopRepository.GetByUserIdsAsync(users, stoppingToken);
        return stopIds;
    }

    private async Task PushToClientsAsync(List<int> stopIds, CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var ztmClient = scope.ServiceProvider.GetRequiredService<IZtmClient>();
        var stopTrackingService = scope.ServiceProvider.GetRequiredService<IStopTrackingService>();
        
        await Parallel.ForEachAsync(stopIds, stoppingToken, async (stopId, _) =>
        {
            var schedule = await ztmClient.GetStopDeparturesAsync(stopId, stoppingToken);
            await stopTrackingService.BroadcastStopSchedule(stopId, schedule, stoppingToken);
        });
    }
}