using Microsoft.AspNetCore.HttpOverrides;
using Web.Extensions;
using Core.IServices;
using Microsoft.AspNetCore.Diagnostics;
using Core.Entities.ErrorModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
var logger = app.Services.GetRequiredService<ILoggerService>();
app.UseExceptionHandler((altPipeline) =>
{
    // add middleware that builds a response to return it to the exceptionHandler middleware which will return it to the client
    altPipeline.Run(async (context) =>
    {
        // Response Headers
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (errorFeature != null)
        {
            logger.LogError($"Something went wrong: {errorFeature.Error}");
            // Response Body
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error."
            }.ToString()
            );
        }

    });
}
);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
