using MediatR;
using Rai4.Application.Services.Interfaces;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Application.Features.BusStopFeatures.Commands;

public class DeleteBusStopCommandHandler : IRequestHandler<DeleteBusStopCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBusStopRepository _busStopRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteBusStopCommandHandler(
        IUnitOfWork unitOfWork,
        IBusStopRepository busStopRepository,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _busStopRepository = busStopRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteBusStopCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var busStop = await _busStopRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"BusStop with ID {request.Id} not found");

        if (busStop.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own bus stops");

        _busStopRepository.Remove(busStop);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
