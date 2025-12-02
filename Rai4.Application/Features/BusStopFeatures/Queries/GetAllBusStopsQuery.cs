using MediatR;
using Rai4.Application.Dto.BusStop;

namespace Rai4.Application.Features.BusStopFeatures.Queries;

public class GetAllBusStopsQuery : IRequest<IEnumerable<BusStopDto>>
{
}
