using Rai4.Domain.Models.Base;

namespace Rai4.Domain.Models;

public class BusStop : BaseDbEntity
{
    public string Name { get; set; }
    
    public int UserId { get; set; }
    public int ZtmStopId { get; set; }
    public User User { get; set; }
}