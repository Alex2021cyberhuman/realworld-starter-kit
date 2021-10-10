using AutoMapper;
using Conduit.Auth.ApplicationLayer.Users.Register;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UsersApplicationLayerMappingProfile : Profile
    {
        public UsersApplicationLayerMappingProfile()
        {
            CreateMap<RegisterUserModel, User>();
        }
    }
}
