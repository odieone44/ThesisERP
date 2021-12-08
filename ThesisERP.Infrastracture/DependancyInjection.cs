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

namespace ThesisERP.Infrastracture
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //string connString = Environment.GetEnvironmentVariable("ThesisERPConnectionString");
            string connString = Environment.GetEnvironmentVariable("ThesisERPConnection_2");

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString: connString)
            );

            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<DatabaseContext>());

            var jwtConfig = configuration.GetSection("JwtSettings");

            services.Configure<JwtSettings>(jwtConfig);
            services.AddMemoryCache();
            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();
            //services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(configuration);
            services.ConfigureAutoMapper();
            services.ConfigureVersioning();

            services.AddScoped(typeof(IRepositoryBase<>), typeof(ThesisEFRepository<>));
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IDocumentService, DocumentService>();
            
            return services;
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>(u => u.User.RequireUniqueEmail = true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 10,
                    Period = "1s"
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperInitializer));
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Environment.GetEnvironmentVariable("THESIS_JWT_KEY");

            services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string responseMessage = string.Empty;
                                                
                        if (contextFeature.Error is ThesisERPException)
                        {
                            responseMessage = contextFeature.Error.Message;
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        } 
                        else
                        {
                            Log.Error($"Something went wrong in {contextFeature.Error}");
                            responseMessage = "Internal Server Error. Please try again.";
                        }
                        await context.Response
                        .WriteAsync(
                            new AppError
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = responseMessage
                            }
                            .ToString()
                        );
                    }
                });
            });
        }
    }
}
