using AspNetCoreRateLimit;
using Serilog;
using System.Text.Json.Serialization;
using ThesisERP;
using ThesisERP.Application;
using ThesisERP.Application.Models;
using ThesisERP.Infrastracture;
using ThesisERP.Api.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen( opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "ThesisERP Api",
        Description = "An ASP.NET Core 6.0 Web API for **ThesisERP**, " +
                      "a simple ERP application created for my BSc Computer Science Thesis @ University Of Pireaus." +
                      "<br />  <br />" +
                      "After logging in, you can authenticate your requests by including an <code>Authorization: Bearer *YourToken*</code> header. <br />" +
                      "To test the API from this page, you can authorize by clicking the button on the right and entering your JWT in the required field. "

    });

    var securityScheme = new OpenApiSecurityScheme()
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Insert your JWT into the field",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Reference = new OpenApiReference()
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    opt.AddSecurityDefinition("Bearer", securityScheme);
    opt.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer"} }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments:true);

});

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
    //var swaggerJsonBasePath = string.IsNullOrEmpty(c.RoutePrefix) ? "." : "..";

    c.SwaggerEndpoint($"../api/v1/swagger.json", "ThesisERP API v1");
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
        await SeedDatabase.Initialize(services);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to seed database.");
    }
}

Log.Information("App is starting...");
app.Run();
