using AutoMapper;
using Rai4.Application.Dto.BusStop;
using Rai4.Application.Features.BusStopFeatures.Commands;
using Rai4.Domain.Models;

namespace Rai4.Application.Mapping;

public class BusStopMappingProfiles : Profile
{
    public BusStopMappingProfiles()
    {
        // Entity to DTO
        CreateMap<BusStop, BusStopDto>()
            .ForMember(dest => dest.UserFullName,
                opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

        // DTO to Command
        CreateMap<CreateBusStopDto, CreateBusStopCommand>();
        CreateMap<UpdateBusStopDto, UpdateBusStopCommand>();

        // Command to Entity
        CreateMap<CreateBusStopCommand, BusStop>();
        CreateMap<UpdateBusStopCommand, BusStop>();
    }
}
