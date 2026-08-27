using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfMembershipRepository(FunBooksAndVideosDbContext dbContext) : IMembershipRepository
    {
        public async Task<IReadOnlyCollection<Membership>> GetActiveByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
            => await dbContext.Memberships
                .Where(membership => membership.CustomerId == customerId && membership.IsActive)
                .ToListAsync(cancellationToken);

        public async Task AddAsync(Membership membership, CancellationToken cancellationToken = default)
            => await dbContext.Memberships.AddAsync(membership, cancellationToken);
    }
}
