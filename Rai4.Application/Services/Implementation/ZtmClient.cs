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
    public async Task<List<BusStopJson>> GetAllBusStopsAsync(CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue<List<BusStopJson>>(BusStopsCacheKey, out var busStops))
            return busStops!;
        
        var opts = options.Value;

        var resp = await httpClient.GetAsync(opts.GetAllStopsUrl, cancellationToken);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, TransitData>>(cancellationToken: cancellationToken);
        var stops = json?.First().Value.Stops ?? [];
        memoryCache.Set(BusStopsCacheKey, stops, TimeSpan.FromHours(69));
        return stops;
    }

    public async Task<StopDepartures> GetStopDeparturesAsync(int stopId, CancellationToken cancellationToken = default)
    {
        var opts = options.Value;
        var url = $"{opts.GetDeparturesUrl}?stopId={stopId}";

        var resp = await httpClient.GetFromJsonAsync<StopDepartures>(url, cancellationToken: cancellationToken);
        return resp ?? new StopDepartures();
    }
}