namespace FunBooksAndVideos.Persistence.Seed;

public interface ISeedDataProvider
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
