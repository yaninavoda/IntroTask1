using AutoMapper;
using Entities.Models;
using Shared.Dtos.UserDtos;

namespace IntroTaskWebApi.Mapping;

public class UserPrifile : Profile
{
    public UserPrifile()
    {
        CreateMap<UserRegistrationDto, User>();
    }
}
