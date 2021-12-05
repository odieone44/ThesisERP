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
                               opt => opt.MapFrom(val => Entities.EntityTypes.client));            

            CreateMap<Entity, CreateClientDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(val => Entities.EntityTypes.client))
                    .ForMember(dest => dest.DateCreated,
                               opt => opt.MapFrom(val => DateTime.UtcNow));

            CreateMap<Entity, UpdateClientDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(val => Entities.EntityTypes.client))
                    .ForMember(dest => dest.DateCreated, 
                               opt => opt.UseDestinationValue())
                    .ForMember(dest => dest.DateUpdated,
                               opt => opt.MapFrom(val => DateTime.UtcNow));

            CreateMap<Entity, SupplierDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(val => Entities.EntityTypes.supplier));

            CreateMap<Entity, CreateSupplierDTO>()
               .ReverseMap()
                   .ForMember(dest => dest.EntityType,
                              opt => opt.MapFrom(val => Entities.EntityTypes.supplier))
                   .ForMember(dest => dest.DateCreated,
                               opt => opt.MapFrom(val => DateTime.UtcNow));

            CreateMap<Entity, UpdateSupplierDTO>()
                .ReverseMap()
                    .ForMember(dest => dest.EntityType,
                               opt => opt.MapFrom(val => Entities.EntityTypes.supplier))
                    .ForMember(dest => dest.DateCreated,
                               opt => opt.UseDestinationValue())
                    .ForMember(dest => dest.DateUpdated,
                               opt => opt.MapFrom(val => DateTime.UtcNow));


            CreateMap<Address, AddressDTO>().
                ForMember(dest=>dest.CountryCode, 
                          opt => opt.MapFrom(src => src.Country))
                .ReverseMap();



        }
    }
}
