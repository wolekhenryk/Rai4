using MediatR;
using Rai4.Application.Dto.BusStop;

namespace Rai4.Application.Features.BusStopFeatures.Queries;

public class GetBusStopByIdQuery : IRequest<BusStopDto?>
{
    public int Id { get; set; }
}
