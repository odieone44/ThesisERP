using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ThesisERP.Core.Extensions;
using ThesisERP.Core.Models;
using ThesisERP.Data;

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

string? connString = Environment.GetEnvironmentVariable("ThesisERPConnectionString");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connectionString: connString ?? string.Empty)
);

var jwtConfig = builder.Configuration.GetSection("JwtSettings");

builder.Services.Configure<JwtSettings>(jwtConfig);

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddControllers()
                .AddNewtonsoftJson(op =>
                    op.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.ConfigureAutoMapper();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseEndpoints(endpoints=> endpoints.MapControllers());

app.MapControllers();
Log.Information("App is starting...");
app.Run();
