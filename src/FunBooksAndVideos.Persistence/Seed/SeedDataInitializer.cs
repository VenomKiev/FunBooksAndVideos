using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Seed
{
    public sealed class SeedDataInitializer(FunBooksAndVideosDbContext dbContext) : ISeedDataProvider
    {
        public Task SeedAsync(CancellationToken cancellationToken = default)
            => SeedAndSaveAsync(cancellationToken);

        private async Task SeedAndSaveAsync(CancellationToken cancellationToken)
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            SeedData.AddInitialData(dbContext);
            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
