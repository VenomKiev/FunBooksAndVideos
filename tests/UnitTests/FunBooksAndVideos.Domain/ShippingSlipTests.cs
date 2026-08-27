using FluentAssertions;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Services;

namespace FunBooksAndVideos.Domain.UnitTests
{
    public sealed class ShippingSlipTests
    {
        [Fact]
        public void CreateForPhysicalProduct_CreatesSlipWithCreatedStatus()
        {
            // Arrange
            var order = PurchaseOrder.Create(Guid.NewGuid(),10m, []);
            var product = Product.Create("Book", ProductType.Book, 10m, true);
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
            var order = PurchaseOrder.Create(Guid.NewGuid(), 10m, []);
            var product = Product.Create("Video", ProductType.Video, 10m, false);
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
