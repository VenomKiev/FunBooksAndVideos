using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default);

        Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
