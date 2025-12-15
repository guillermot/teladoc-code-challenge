using AutoMapper;
using Teladoc.Services.Domain;

using UserEntity = Teladoc.DataAccess.Entities.User;

namespace Teladoc.Services.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Entity -> Domain
        CreateMap<UserEntity, User>();
        
        // Domain -> Entity
        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}


