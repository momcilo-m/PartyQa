using AutoMapper;
using TestiranjeAPI.Models;
namespace Backend.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserRegister>();
        CreateMap<UserRegister, User>();
        CreateMap<User, UserUpdate>();
        CreateMap<UserUpdate, User>();
        CreateMap<UserViewModel, User>();
        CreateMap<User, UserViewModel>();
    }
}