using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.DTOs.Documents;
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
        _createStockLevelMaps();
        _createEntityMaps();
        _createInventoryLocationMaps();
        _createProductMaps();
        _createDocumentMaps();

    }

    private void _createAppUserMaps()
    {
        CreateMap<AppUser, UserDTO>().ReverseMap();
        CreateMap<AppUser, RegisterUserDTO>().ReverseMap();
        CreateMap<AppUser, LoginUserDTO>().ReverseMap();
    }

    private void _createEntityMaps()
    {
        CreateMap<Entity, EntityBaseInfoDTO>();

        CreateMap<Entity, ClientDTO>()
            .ReverseMap()
                .ForMember(dest => dest.EntityType,
                           opt => opt.MapFrom(val => EntityType.client));

        CreateMap<Entity, ClientBaseInfoDTO>()
            .ReverseMap()
                .ForMember(dest => dest.EntityType,
                           opt => opt.MapFrom(val => EntityType.client));

        CreateMap<Entity, CreateClientDTO>()
            .ReverseMap()
                .ForMember(dest => dest.EntityType,
                           opt => opt.MapFrom(val => EntityType.client))
                .ForMember(dest => dest.DateCreated,
                           opt => opt.MapFrom(val => DateTime.UtcNow));

        CreateMap<Entity, UpdateClientDTO>()
            .ReverseMap()
                .ForMember(dest => dest.EntityType,
                           opt => opt.MapFrom(val => EntityType.client))
                .ForMember(dest => dest.DateCreated,
                           opt => opt.UseDestinationValue())
                .ForMember(dest => dest.DateUpdated,
                           opt => opt.MapFrom(val => DateTime.UtcNow));

        CreateMap<Entity, SupplierDTO>()
            .ReverseMap()
                .ForMember(dest => dest.EntityType,
                           opt => opt.MapFrom(val => EntityType.supplier));

        CreateMap<Entity, SupplierBaseInfoDTO>()
         .ReverseMap()
             .ForMember(dest => dest.EntityType,
                        opt => opt.MapFrom(val => EntityType.supplier));

        CreateMap<Entity, CreateSupplierDTO>()
           .ReverseMap()
               .ForMember(dest => dest.EntityType,
                          opt => opt.MapFrom(val => EntityType.supplier))
               .ForMember(dest => dest.DateCreated,
                           opt => opt.MapFrom(val => DateTime.UtcNow));

        CreateMap<Entity, UpdateSupplierDTO>()
            .ReverseMap()
                .ForMember(dest => dest.EntityType,
                           opt => opt.MapFrom(val => EntityType.supplier))
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

        CreateMap<InventoryLocation, InventoryLocationBaseDTO>();

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
                                                .Where(x=>x.EntityType == EntityType.client)
                                                .ToList()))
            .ForMember(dest => dest.RelatedSuppliers,
                       opt => opt.MapFrom(val => val.RelatedEntities
                                                 .Where(x => x.EntityType == EntityType.supplier)
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

    private void _createStockLevelMaps()
    {
        CreateMap<StockLevel, StockLevelDTO>().ReverseMap();

        //CreateMap<StockLevel, ProductStockLevelDTO>()
        //    .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product.SKU))
        //    .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description));

    }

    private void _createDocumentMaps()
    {
        CreateMap<DocumentRow, DocumentRowDTO>()
            .ForMember(x => x.ProductSKU, opt => opt.MapFrom(src => src.Product.SKU))
            .ForMember(x => x.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
            .ForMember(x => x.TaxName, opt => opt.MapFrom(src => src.Tax.Name))
            .ForMember(x => x.DiscountName, opt => opt.MapFrom(src => src.Discount.Name));


        CreateMap<Document, DocumentDTO>();

        CreateMap<Document, SalesDocumentDTO>()
            .ForMember(x=>x.Client, opt => opt.MapFrom(src=>src.Entity));

        CreateMap<Document, PurchaseDocumentDTO>()
            .ForMember(x => x.Supplier, opt => opt.MapFrom(src => src.Entity));       

    }
}
