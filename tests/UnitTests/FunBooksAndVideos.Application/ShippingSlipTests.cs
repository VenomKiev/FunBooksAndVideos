using FluentAssertions;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Services;

namespace FunBooksAndVideos.Application.UnitTests
{
    public sealed class ShippingSlipTests
    {
        [Fact]
        public void CreateForPhysicalProduct_CreatesSlipWithCreatedStatus()
        {
            // Arrange
            var order = new PurchaseOrder(Guid.NewGuid(), Guid.NewGuid(), 10m, []);
            var product = new Product(Guid.NewGuid(), "Book", ProductType.Book, 10m, true);
            var service = new ShippingSlipService();

            // Act
            var result = service.CreateForPhysicalProduct(order, product);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ShippingSlip.Should().NotBeNull();
            result.ShippingSlip!.PurchaseOrderId.Should().Be(order.Id);
            result.ShippingSlip.ProductId.Should().Be(product.Id);
            result.ShippingSlip.Status.Should().Be(ShippingSlipStatus.Created);
        }

        [Fact]
        public void CreateForDigitalProduct_ReturnsValidationFailure()
        {
            // Arrange
            var order = new PurchaseOrder(Guid.NewGuid(), Guid.NewGuid(), 10m, []);
            var product = new Product(Guid.NewGuid(), "Video", ProductType.Video, 10m, false);
            var service = new ShippingSlipService();

            // Act
            var result = service.CreateForPhysicalProduct(order, product);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorCode.Should().Be("PRODUCT_NOT_PHYSICAL");
            result.ShippingSlip.Should().BeNull();
        }
    }
}
