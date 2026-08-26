using Serilog;
using ILogger = Serilog.ILogger;

namespace FunBooksAndVideos.API.Extensions
{
    public static class SerilogExtensions
    {
        public static ILogger CreateBootstrapLogger()
            => new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

        public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId();
            });

            return builder;
        }
    }
}
