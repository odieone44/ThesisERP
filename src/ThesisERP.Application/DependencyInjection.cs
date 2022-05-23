using Microsoft.Extensions.DependencyInjection;
using ThesisERP.Application.Interfaces.Entities;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Services.Entities;
using ThesisERP.Application.Services.Transactions;
using ThesisERP.Application.Services;

namespace ThesisERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.ConfigureAutoMapper();
        services.AddScoped<IApiService, ApiService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IClientService, ClientService>();

        return services;
    }

    private static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperInitializer));        
    }
}