using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IReadOnlyCollection<Membership>> GetActiveByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

        Task AddAsync(Membership membership, CancellationToken cancellationToken = default);
    }
}
