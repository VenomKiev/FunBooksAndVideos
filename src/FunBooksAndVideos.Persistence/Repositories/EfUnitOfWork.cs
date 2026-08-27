using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Persistence.Context;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfUnitOfWork(FunBooksAndVideosDbContext dbContext) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await dbContext.SaveChangesAsync(cancellationToken);
    }
}
