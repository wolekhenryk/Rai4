using AutoMapper;
using MediatR;
using Rai4.Application.Dto.BusStop;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.BusStopFeatures.Queries;

public class GetBusStopByIdQueryHandler : IRequestHandler<GetBusStopByIdQuery, BusStopDto?>
{
    private readonly IBusStopRepository _busStopRepository;
    private readonly IMapper _mapper;

    public GetBusStopByIdQueryHandler(IBusStopRepository busStopRepository, IMapper mapper)
    {
        _busStopRepository = busStopRepository;
        _mapper = mapper;
    }

    public async Task<BusStopDto?> Handle(GetBusStopByIdQuery request, CancellationToken cancellationToken)
    {
        var busStop = await _busStopRepository.GetByIdWithUserAsync(request.Id, cancellationToken);
        return busStop == null ? null : _mapper.Map<BusStopDto>(busStop);
    }
}
