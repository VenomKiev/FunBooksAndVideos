using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Application.Features.PurchaseOrders.Handlers;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Application.Services;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunBooksAndVideos.Application.UnitTests
{

public sealed class CreatePurchaseOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_MixedProductOrder_CreatesItemizedOrderWithCatalogTotal()
    {
        var customer = new Customer(Guid.NewGuid(), "Test Customer");
        var products = new[]
        {
            new Product(Guid.NewGuid(), "Book", ProductType.Book, 14.99m, true),
            new Product(Guid.NewGuid(), "Video", ProductType.Video, 29.99m, false)
        };
        var orders = new List<PurchaseOrder>();
        var handler = new CreatePurchaseOrderCommandHandler(
            new FakeCustomerRepository(customer),
            new FakeOrderRepository(orders),
            new PurchaseOrderValidationService(new FakeProductRepository(products)),
            new FakeUnitOfWork(),
            NullLogger<CreatePurchaseOrderCommandHandler>.Instance);

        var result = await handler.Handle(
            new CreatePurchaseOrderCommand(
                customer.Id,
                [
                    new(products[0].Id, "product", 1),
                    new(products[1].Id, "product", 2)
                ]),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(74.97m, result.TotalPrice);
        Assert.Equal(2, result.ItemLines!.Count);
        Assert.Single(orders);
    }

    [Fact]
    public async Task Handle_EmptyItems_ReturnsValidationFailure()
    {
        var customer = new Customer(Guid.NewGuid(), "Test Customer");
        var handler = new CreatePurchaseOrderCommandHandler(
            new FakeCustomerRepository(customer),
            new FakeOrderRepository([]),
            new PurchaseOrderValidationService(new FakeProductRepository([])),
            new FakeUnitOfWork(),
            NullLogger<CreatePurchaseOrderCommandHandler>.Instance);

        var result = await handler.Handle(new CreatePurchaseOrderCommand(customer.Id, []), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("ITEMS_REQUIRED", result.ErrorCode);
    }

    private sealed class FakeCustomerRepository(Customer customer) : ICustomerRepository
    {
        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult<Customer?>(id == customer.Id ? customer : null);
    }

    private sealed class FakeProductRepository(IEnumerable<Product> products) : IProductRepository
    {
        private readonly Dictionary<Guid, Product> products = products.ToDictionary(product => product.Id);

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(products.GetValueOrDefault(id));
    }

    private sealed class FakeOrderRepository(List<PurchaseOrder> orders) : IPurchaseOrderRepository
    {
        public Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
        {
            orders.Add(order);
            return Task.CompletedTask;
        }

        public Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(orders.FirstOrDefault(order => order.Id == id));
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(1);
    }
}
}
