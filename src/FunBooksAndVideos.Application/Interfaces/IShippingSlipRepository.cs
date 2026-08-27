using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Interfaces
{
    public interface IShippingSlipRepository
    {
        Task AddAsync(ShippingSlip shippingSlip, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<ShippingSlip>> GetByOrderIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);
    }
}
