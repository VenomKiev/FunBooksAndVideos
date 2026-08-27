using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfProductRepository(FunBooksAndVideosDbContext dbContext) : IProductRepository
    {
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await dbContext.Products.SingleOrDefaultAsync(product => product.Id == id, cancellationToken);
    }
}
