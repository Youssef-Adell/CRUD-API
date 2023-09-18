using Core.Entities.ErrorModel;
using Core.Entities.Exceptions;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureExceptionHandlerMiddleware(this WebApplication app, ILoggerService logger)
    {
        app.UseExceptionHandler((altPipeline) =>
        {
            /*
            Add middleware that builds a response (based on the type of thrown exception)
            to return it to the exceptionHandler middleware which will return it to the client.
            */
            altPipeline.Run(async (context) =>
            {
                // Response Header
                context.Response.ContentType = "application/json";

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (errorFeature != null)
                {
                    logger.LogError($"Something went wrong: {errorFeature.Error}");

                    // Response Header
                    context.Response.StatusCode = errorFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    // Response Body
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = errorFeature.Error.Message
                    }.ToString()
                    );
                }

            });
        }
        );
    }
}
