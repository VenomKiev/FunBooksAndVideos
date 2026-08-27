using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfCustomerRepository(FunBooksAndVideosDbContext dbContext) : ICustomerRepository
    {
        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await dbContext.Customers.Include(customer => customer.Memberships)
                .SingleOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }
}
