namespace Rai4.Domain.Models.Base;

public abstract class BaseDbEntity
{
    public int Id { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}