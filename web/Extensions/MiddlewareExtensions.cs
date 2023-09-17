using Core.Entities.ErrorModel;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureExceptionHandlerMiddleware(this WebApplication app, ILoggerService logger)
    {
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
    }
}
