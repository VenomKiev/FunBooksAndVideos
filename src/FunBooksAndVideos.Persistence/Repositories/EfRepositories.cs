using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Repositories
{
    public sealed class EfCustomerRepository(FunBooksAndVideosDbContext dbContext) : ICustomerRepository
    {
        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => dbContext.Customers.SingleOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }
}

public sealed class EfProductRepository(FunBooksAndVideosDbContext dbContext) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Products.SingleOrDefaultAsync(product => product.Id == id, cancellationToken);
}

public sealed class EfPurchaseOrderRepository(FunBooksAndVideosDbContext dbContext) : IPurchaseOrderRepository
{
    public Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
        => dbContext.PurchaseOrders.AddAsync(order, cancellationToken).AsTask();

    public Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.PurchaseOrders.Include(order => order.ItemLines)
            .SingleOrDefaultAsync(order => order.Id == id, cancellationToken);
}

public sealed class EfUnitOfWork(FunBooksAndVideosDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
