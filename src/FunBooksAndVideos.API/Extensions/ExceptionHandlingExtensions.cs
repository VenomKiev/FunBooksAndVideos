using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.API.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        public static IServiceCollection AddCentralExceptionHandling(this IServiceCollection services)
        {
            services.AddProblemDetails();
            return services;
        }

        public static WebApplication UseCentralExceptionHandling(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                        .CreateLogger("ExceptionHandling");

                    logger.LogError(exception, "Unhandled exception while processing {RequestPath}", context.Request.Path);

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/problem+json";

                    var problem = new ProblemDetails
                    {
                        Type = "https://funbooksandvideos.example/errors/internal",
                        Title = "An unexpected error occurred.",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "The request could not be completed.",
                        Instance = context.Request.Path
                    };
                    problem.Extensions["traceId"] = context.TraceIdentifier;
                    problem.Extensions["code"] = "INTERNAL_ERROR";

                    await context.Response.WriteAsJsonAsync(problem, context.RequestAborted);
                });
            });

            return app;
        }
    }
}
