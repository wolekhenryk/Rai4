using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Rai4.Application.Dto.Json;
using Rai4.Application.Services.Interfaces;
using Rai4.Infrastructure.Options;

namespace Rai4.Application.Services.Implementation;

public class ZtmClient(
    HttpClient httpClient,
    IOptions<ZtmOptions> options,
    IMemoryCache memoryCache) : IZtmClient
{
    private const string BusStopsCacheKey = "ZtmBusStops";

    public async Task<List<FriendlyStop>> GetAllBusStopsAsync(CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue<List<FriendlyStop>>(BusStopsCacheKey, out var busStops))
            return busStops!;

        var opts = options.Value;

        using var resp = await httpClient.GetAsync(opts.GetAllStopsUrl, cancellationToken);
        resp.EnsureSuccessStatusCode();

        var json =
            await resp.Content.ReadFromJsonAsync<Dictionary<string, TransitData>>(cancellationToken: cancellationToken);
        var stops = json?.First().Value.Stops ?? [];
        var friendlyStops = stops
            .Where(s => s.StopName != null && s.StopName != "no name")
            .DistinctBy(s => s.StopName)
            .Select(s => new FriendlyStop
            {
                StopId = s.StopId,
                StopName = s.StopName ?? "no name"
            })
            .OrderBy(s => s.StopName)
            .ToList();
        memoryCache.Set(BusStopsCacheKey, friendlyStops, TimeSpan.FromHours(69));
        return friendlyStops;
    }

    public async Task<StopDepartures> GetStopDeparturesAsync(int stopId, CancellationToken cancellationToken = default)
    {
        var opts = options.Value;
        var url = $"{opts.GetDeparturesUrl}?stopId={stopId}";

        var resp = await httpClient.GetFromJsonAsync<StopDepartures>(url, cancellationToken: cancellationToken);
        return resp ?? new StopDepartures();
    }
}