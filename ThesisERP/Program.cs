using Microsoft.EntityFrameworkCore;
using Serilog;
using ThesisERP.Core.Extensions;
using ThesisERP.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = Environment.GetEnvironmentVariable("ThesisERPConnectionString");
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

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connectionString: connString)
);

builder.Services.ConfigureIdentity();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Information("App is starting...");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
