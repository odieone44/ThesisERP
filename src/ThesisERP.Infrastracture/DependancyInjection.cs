using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using ThesisERP.Core.Entities;
using ThesisERP.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using ThesisERP.Infrastracture.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Services;
using ThesisERP.Core.Exceptions;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Services.Transactions;

namespace ThesisERP.Infrastracture;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        string connString = Environment.GetEnvironmentVariable("ThesisERPConnectionString");
        //string connString = Environment.GetEnvironmentVariable("ThesisERPConnection_2");

        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connectionString: connString)
        );      

        services.ConfigureIdentity();

        //add scoped services: one instance per request.
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<DatabaseContext>());
        services.AddScoped(typeof(IRepositoryBase<>), typeof(ThesisEFRepository<>));
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddScoped<IDocumentService, DocumentService>();

        return services;
    }

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<AppUser>(u => u.User.RequireUniqueEmail = true);

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
    }

}
