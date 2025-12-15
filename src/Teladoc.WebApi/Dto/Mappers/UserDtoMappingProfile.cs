using AutoMapper;
using Teladoc.Services.Domain;
using Teladoc.WebApi.Dto.v1;

namespace Teladoc.WebApi.Dto.Mappers;

public class UserDtoMappingProfile: Profile
{
    public UserDtoMappingProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<User, UserResponseDto>();
    }
}