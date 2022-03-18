using Microsoft.Extensions.DependencyInjection;
using ThesisERP.Application.Mappings;

namespace ThesisERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.ConfigureAutoMapper();
        return services;
    }

    private static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperInitializer));     
    }
}