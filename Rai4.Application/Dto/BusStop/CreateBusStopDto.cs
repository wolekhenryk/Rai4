using System.ComponentModel.DataAnnotations;

namespace Rai4.Application.Dto.BusStop;

public class CreateBusStopDto
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ZtmStopId must be greater than 0")]
    public int ZtmStopId { get; set; }
}
