using AutoMapper;
using MediatR;
using Rai4.Application.Dto.BusStop;
using Rai4.Application.Services.Interfaces;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.BusStopFeatures.Commands;

public class UpdateBusStopCommandHandler : IRequestHandler<UpdateBusStopCommand, BusStopDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBusStopRepository _busStopRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBusStopCommandHandler(
        IUnitOfWork unitOfWork,
        IBusStopRepository busStopRepository,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _busStopRepository = busStopRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BusStopDto> Handle(UpdateBusStopCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var busStop = await _busStopRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"BusStop with ID {request.Id} not found");

        if (busStop.UserId != userId)
            throw new UnauthorizedAccessException("You can only update your own bus stops");

        busStop.Name = request.Name;
        busStop.ZtmStopId = request.ZtmStopId;

        _busStopRepository.Update(busStop);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedBusStop = await _busStopRepository.GetByIdWithUserAsync(busStop.Id, cancellationToken);
        return _mapper.Map<BusStopDto>(updatedBusStop);
    }
}
