using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Services;
using ThesisERP.Application.Services.Transactions;
using ThesisERP.Core.Entities;
using ThesisERP.Infrastracture.Data;

namespace ThesisERP.Infrastracture;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connString = configuration.GetConnectionString("ThesisERPConnectionString");
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connectionString: connString)
        );

        services.ConfigureIdentity();

        //add scoped services: one instance per request.
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<DatabaseContext>());
        services.AddScoped(typeof(IRepositoryBase<>), typeof(ThesisEFRepository<>));
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddScoped<IApiService, ApiService>();
        services.AddScoped<IDocumentService, DocumentService>();

        return services;
    }

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<AppUser>(u => u.User.RequireUniqueEmail = true)
                               .AddRoles<IdentityRole>()
                               .AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

        //builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        //builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
    }

}
