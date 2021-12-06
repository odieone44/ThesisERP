using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.Mappings;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {

        CreateMap<Address, AddressDTO>().
           ForMember(dest => dest.CountryCode,
                     opt => opt.MapFrom(src => src.Country))
           .ReverseMap();

        _createAppUserMaps();
        _createEntityMaps();
        _createInventoryLocationMaps();
        _createProductMaps();
    }

    private void _createAppUserMaps()
    {
        CreateMap<AppUser, UserDTO>().ReverseMap();
        CreateMap<AppUser, RegisterUserDTO>().ReverseMap();
        CreateMap<AppUser, LoginUserDTO>().ReverseMap();
    }

    private void _createEntityMaps()
    {
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
    }

    private void _createInventoryLocationMaps()
    {
        CreateMap<InventoryLocation, InventoryLocationDTO>()
            .ReverseMap()
                .ForMember(dest => dest.StockLevels,
                           opt => opt.UseDestinationValue());

        CreateMap<InventoryLocation, CreateInventoryLocationDTO>()
            .ReverseMap()
                .ForMember(dest => dest.StockLevels,
                           opt => opt.UseDestinationValue());

        CreateMap<InventoryLocation, UpdateInventoryLocationDTO>()
            .ReverseMap()
                .ForMember(dest => dest.StockLevels,
                           opt => opt.UseDestinationValue());
    }

    private void _createProductMaps()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.RelatedClients, 
                       opt=> opt.MapFrom(val => val.RelatedEntities
                                                .Where(x=>x.EntityType == Entities.EntityTypes.client)
                                                .ToList()))
            .ForMember(dest => dest.RelatedSuppliers,
                       opt => opt.MapFrom(val => val.RelatedEntities
                                                 .Where(x => x.EntityType == Entities.EntityTypes.supplier)
                                                 .ToList()))
             .ReverseMap()
                 .ForMember(dest => dest.RelatedEntities,
                            opt => opt.UseDestinationValue());

        CreateMap<Product, CreateProductDTO>()
            .ReverseMap()
                 .ForMember(dest => dest.RelatedEntities,
                            opt => opt.UseDestinationValue());

        CreateMap<Product, UpdateProductDTO>()
              .ReverseMap()
                 .ForMember(dest => dest.RelatedEntities,
                            opt => opt.UseDestinationValue());
    }
}
