using MediatR;
using Rai4.Application.Dto.BusStop;

namespace Rai4.Application.Features.BusStopFeatures.Commands;

public class UpdateBusStopCommand : IRequest<BusStopDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ZtmStopId { get; set; }
}
