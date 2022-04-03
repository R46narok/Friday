using AutoMapper;
using Friday.Services.Authorization.Commands.User.CreateApplicationUser;
using Friday.Services.Authorization.DTOs;
using Friday.Services.Authorization.Entities;

namespace Friday.Services.Authorization.Profiles;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<CreateApplicationUserCommand, ApplicationUser>();
        CreateMap<CreateUserDto, CreateApplicationUserCommand>();

        CreateMap<ApplicationUser, GetUserDto>();
    }
}