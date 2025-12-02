using MediatR;

namespace Rai4.Application.Features.BusStopFeatures.Commands;

public class DeleteBusStopCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
