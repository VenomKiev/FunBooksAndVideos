using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FluentAssertions;
using FunBooksAndVideos.Application.Features.PurchaseOrders.Handlers;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Application.Services;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunBooksAndVideos.Application.UnitTests
{

public sealed class CreatePurchaseOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_MixedProductOrder_CreatesItemizedOrderWithCatalogTotal()
    {
        // Arrange
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
            NullLogger<CreatePurchaseOrderCommandHandler>.Instance,
            new MembershipActivationService(),
            new FakeMembershipRepository());

        // Act
        var result = await handler.Handle(
            new CreatePurchaseOrderCommand(
                customer.Id,
                [
                    new(products[0].Id, "product", 1),
                    new(products[1].Id, "product", 2)
                ]),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.TotalPrice.Should().Be(74.97m);
        result.ItemLines.Should().HaveCount(2);
        orders.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_EmptyItems_ReturnsValidationFailure()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "Test Customer");
        var handler = new CreatePurchaseOrderCommandHandler(
            new FakeCustomerRepository(customer),
            new FakeOrderRepository([]),
            new PurchaseOrderValidationService(new FakeProductRepository([])),
            new FakeUnitOfWork(),
            NullLogger<CreatePurchaseOrderCommandHandler>.Instance,
            new MembershipActivationService(),
            new FakeMembershipRepository());

        // Act
        var result = await handler.Handle(new CreatePurchaseOrderCommand(customer.Id, []), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("ITEMS_REQUIRED");
    }

    [Fact]
    public async Task Handle_SaveFailure_RemovesActivatedMembershipFromCustomer()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "Test Customer");
        var membershipProduct = new Product(Guid.NewGuid(), "Book Club", ProductType.Membership, 9.99m, false, MembershipType.BookClub);
        var handler = new CreatePurchaseOrderCommandHandler(
            new FakeCustomerRepository(customer),
            new FakeOrderRepository([]),
            new PurchaseOrderValidationService(new FakeProductRepository([membershipProduct])),
            new ThrowingUnitOfWork(),
            NullLogger<CreatePurchaseOrderCommandHandler>.Instance,
            new MembershipActivationService(),
            new FakeMembershipRepository());

        // Act
        Func<Task> act = () => handler.Handle(
            new CreatePurchaseOrderCommand(customer.Id, [new(membershipProduct.Id, "membership", 1)]),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        customer.Memberships.Should().BeEmpty();
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

    private sealed class ThrowingUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("Persistence failure");
    }

    private sealed class FakeMembershipRepository : IMembershipRepository
    {
        public Task<IReadOnlyCollection<Membership>> GetActiveByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyCollection<Membership>>([]);

        public Task AddAsync(Membership membership, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
}
