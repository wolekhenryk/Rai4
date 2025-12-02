using AutoMapper;
using Rai4.Application.Features.UserFeatures.Commands;
using Rai4.Domain.Models;

namespace Rai4.Application.Mapping;

public class UserMappingProfiles : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<RegisterUserCommand, User>()
            .ForMember(dst => dst.PasswordHash,
                opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
            .ForMember(dst => dst.Email,
                opt => opt.MapFrom(src => src.Email.ToLower().Trim()))
            .ForMember(dst => dst.FirstName, 
                opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dst => dst.LastName, 
                opt => opt.MapFrom(src => src.LastName.Trim()));
    }
}