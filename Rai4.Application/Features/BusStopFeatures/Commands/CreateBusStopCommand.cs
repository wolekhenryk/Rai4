using MediatR;
using Rai4.Application.Dto.BusStop;

namespace Rai4.Application.Features.BusStopFeatures.Commands;

public class CreateBusStopCommand : IRequest<BusStopDto>
{
    public string Name { get; set; }
    public int ZtmStopId { get; set; }
}
