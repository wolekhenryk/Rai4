using Rai4.Domain.Models.Base;

namespace Rai4.Domain.Models;

public class User : BaseDbEntity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<BusStop> BusStops { get; set; } = [];
}