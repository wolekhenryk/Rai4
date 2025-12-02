using AutoMapper;
using MediatR;
using Rai4.Application.Dto.BusStop;
using Rai4.Application.Services.Interfaces;
using Rai4.Domain.Models;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.BusStopFeatures.Commands;

public class CreateBusStopCommandHandler : IRequestHandler<CreateBusStopCommand, BusStopDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBusStopRepository _busStopRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateBusStopCommandHandler(
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

    public async Task<BusStopDto> Handle(CreateBusStopCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var busStop = new BusStop
        {
            Name = request.Name,
            ZtmStopId = request.ZtmStopId,
            UserId = userId,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _busStopRepository.AddAsync(busStop, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdBusStop = await _busStopRepository.GetByIdWithUserAsync(busStop.Id, cancellationToken);
        return _mapper.Map<BusStopDto>(createdBusStop);
    }
}
