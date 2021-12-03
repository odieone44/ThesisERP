using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Mappings
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<AppUser, UserDTO>().ReverseMap();
            CreateMap<AppUser, RegisterUserDTO>().ReverseMap();
            CreateMap<AppUser, LoginUserDTO>().ReverseMap();
        }
    }
}
