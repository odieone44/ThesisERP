using Microsoft.Extensions.DependencyInjection;
using ThesisERP.Application.Interfaces.Entities;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Services.Entities;
using ThesisERP.Application.Services.Transactions;
using ThesisERP.Application.Services;
using ThesisERP.Application.Interfaces.Pricing;
using ThesisERP.Application.Services.Pricing;
using ThesisERP.Application.Interfaces.TransactionTemplates;
using ThesisERP.Application.Services.TransactionTemplates;
using ThesisERP.Application.Interfaces.Products;
using ThesisERP.Application.Services.Products;

namespace ThesisERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.ConfigureAutoMapper();
        services.AddScoped<IApiService, ApiService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IDocumentTemplateService, DocumentTemplateService>();
        services.AddScoped<IOrderTemplateService, OrderTemplateService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ILocationService, LocationService>();        

        return services;
    }

    private static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperInitializer));        
    }
}