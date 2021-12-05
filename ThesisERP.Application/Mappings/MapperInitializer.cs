using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.Mappings
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<AppUser, UserDTO>().ReverseMap();
            CreateMap<AppUser, RegisterUserDTO>().ReverseMap();
            CreateMap<AppUser, LoginUserDTO>().ReverseMap();

            CreateMap<Entity, ClientDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(src => Entities.EntityTypes.client));            

            CreateMap<Entity, CreateClientDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(src => Entities.EntityTypes.client));

            CreateMap<Entity, SupplierDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(src => Entities.EntityTypes.supplier));

            CreateMap<Address, AddressDTO>().
                ForMember(dest=>dest.CountryCode, 
                          opt => opt.MapFrom(src => src.Country))
                .ReverseMap();

        }
    }
}
