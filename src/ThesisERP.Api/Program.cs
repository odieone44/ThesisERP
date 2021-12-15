using AspNetCoreRateLimit;
using Serilog;
using System.Text.Json.Serialization;
using ThesisERP;
using ThesisERP.Application;
using ThesisERP.Application.Models;
using ThesisERP.Infrastracture;
using ThesisERP.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog(
    (ctx, loggerConf) =>
    {
        loggerConf
        .WriteTo.Console()
        .WriteTo.File(path: "logs\\log-.txt",
                      outputTemplate: "{Timestamp:yyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                      rollingInterval: RollingInterval.Day,
                      restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
    });

var jwtConfig = builder.Configuration.GetSection("JwtSettings");


builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.Configure<JwtSettings>(jwtConfig);
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimiting();
builder.Services.AddHttpContextAccessor();


builder.Services.ConfigureVersioning();



builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.InjectStylesheet("../swagger/logo.css");
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.RoutePrefix = "api";
    var swaggerJsonBasePath = string.IsNullOrEmpty(c.RoutePrefix) ? "." : "..";

    c.SwaggerEndpoint($"{swaggerJsonBasePath}/api/v1/swagger.json", "ThesisERP API v1");

});

app.ConfigureExceptionHandler();
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {               
        SeedDatabase.Initialize(services);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to seed database.");
    }
}

Log.Information("App is starting...");
app.Run();