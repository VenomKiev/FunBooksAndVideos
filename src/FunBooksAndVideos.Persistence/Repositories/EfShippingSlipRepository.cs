using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfShippingSlipRepository(FunBooksAndVideosDbContext dbContext) : IShippingSlipRepository
    {
        public async Task AddAsync(ShippingSlip shippingSlip, CancellationToken cancellationToken = default)
            => await dbContext.ShippingSlips.AddAsync(shippingSlip, cancellationToken);

        public async Task<IReadOnlyCollection<ShippingSlip>> GetByOrderIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default)
            => await dbContext.ShippingSlips
                .Where(shippingSlip => shippingSlip.PurchaseOrderId == purchaseOrderId)
                .ToListAsync(cancellationToken);
    }
}
