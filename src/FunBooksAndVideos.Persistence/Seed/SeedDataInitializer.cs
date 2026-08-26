using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Seed;

public sealed class SeedDataInitializer(FunBooksAndVideosDbContext dbContext) : ISeedDataProvider
{
    public Task SeedAsync(CancellationToken cancellationToken = default)
        => dbContext.Database.EnsureCreatedAsync(cancellationToken);
}
