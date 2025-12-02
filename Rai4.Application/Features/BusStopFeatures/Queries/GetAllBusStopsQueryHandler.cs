using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rai4.Application.Dto.BusStop;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.BusStopFeatures.Queries;

public class GetAllBusStopsQueryHandler : IRequestHandler<GetAllBusStopsQuery, IEnumerable<BusStopDto>>
{
    private readonly IBusStopRepository _busStopRepository;
    private readonly IMapper _mapper;

    public GetAllBusStopsQueryHandler(IBusStopRepository busStopRepository, IMapper mapper)
    {
        _busStopRepository = busStopRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BusStopDto>> Handle(GetAllBusStopsQuery request, CancellationToken cancellationToken)
    {
        var busStops = await _busStopRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BusStopDto>>(busStops);
    }
}
