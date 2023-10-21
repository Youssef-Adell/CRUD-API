using Microsoft.AspNetCore.HttpOverrides;
using Web.Extensions;
using Core.Interfaces.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandlerMiddleware(logger);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
