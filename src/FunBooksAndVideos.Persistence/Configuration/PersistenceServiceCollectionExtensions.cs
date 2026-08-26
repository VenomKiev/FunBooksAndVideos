using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FunBooksAndVideos.Persistence.Configuration;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);

        services.AddDbContext<FunBooksAndVideosDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        return services;
    }
}
