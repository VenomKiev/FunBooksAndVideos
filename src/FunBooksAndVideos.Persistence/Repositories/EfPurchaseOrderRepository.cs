using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfPurchaseOrderRepository(FunBooksAndVideosDbContext dbContext) : IPurchaseOrderRepository
    {
        public async Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
            => await dbContext.PurchaseOrders.AddAsync(order, cancellationToken);

        public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await dbContext.PurchaseOrders.Include(order => order.Items)
                .SingleOrDefaultAsync(order => order.Id == id, cancellationToken);
    }
}
