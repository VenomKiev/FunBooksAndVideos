namespace FunBooksAndVideos.API.Extensions;

public static class LoggingExtensions
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public static WebApplication UseCorrelationId(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = context.TraceIdentifier;
            }

            context.Response.Headers[CorrelationIdHeader] = correlationId;
            using (context.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("CorrelationId")
                .BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                await next(context);
            }
        });

        return app;
    }
}
