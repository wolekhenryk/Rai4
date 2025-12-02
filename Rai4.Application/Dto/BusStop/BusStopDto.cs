namespace Rai4.Application.Dto.BusStop;

public class BusStopDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ZtmStopId { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
