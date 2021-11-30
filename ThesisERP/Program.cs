using Serilog;
using ThesisERP.Infrastracture;
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

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
                .AddNewtonsoftJson(op =>
                    op.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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
app.UseSwaggerUI(c =>
{
    string swaggerJsonBasePath = string.IsNullOrEmpty(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "ThesisERPApi v1");
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

//app.MapControllers();
Log.Information("App is starting...");
app.Run();
