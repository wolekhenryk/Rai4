namespace Rai4.Application.Dto.Json;

public class TransitData
{
    public string LastUpdate { get; set; }
    public List<BusStopJson> Stops { get; set; }
}

public class FriendlyStop
{
    public int StopId { get; set; }
    public string StopName { get; set; }
}

public class BusStopJson
{
    public int StopId { get; set; }
    public string? StopCode { get; set; }
    public string? StopName { get; set; }
    public string? StopShortName { get; set; }
    public string? StopDesc { get; set; }
    public string? SubName { get; set; }
    public string? Date { get; set; }
    public int? ZoneId { get; set; }
    public string? ZoneName { get; set; }
    public int? WheelchairBoarding { get; set; }
    public int? Virtual { get; set; }
    public int? Nonpassenger { get; set; }
    public int? Depot { get; set; }
    public int? TicketZoneBorder { get; set; }
    public int? OnDemand { get; set; }
    public string? ActivationDate { get; set; }
    public double? StopLat { get; set; }
    public double? StopLon { get; set; }
    public string? Type { get; set; }
    public string? StopUrl { get; set; }
    public string? LocationType { get; set; }
    public string? ParentStation { get; set; }
    public string? StopTimezone { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Departure
{
    public string id { get; set; }
    public int? delayInSeconds { get; set; }
    public DateTime estimatedTime { get; set; }
    public string headsign { get; set; }
    public int routeId { get; set; }
    public string routeShortName { get; set; }
    public DateTime scheduledTripStartTime { get; set; }
    public int tripId { get; set; }
    public string status { get; set; }
    public DateTime theoreticalTime { get; set; }
    public DateTime timestamp { get; set; }
    public int trip { get; set; }
    public int? vehicleCode { get; set; }
    public int? vehicleId { get; set; }
    public string vehicleService { get; set; }
}

public class StopDepartures
{
    public DateTime lastUpdate { get; set; }
    public List<Departure> departures { get; set; }
}

