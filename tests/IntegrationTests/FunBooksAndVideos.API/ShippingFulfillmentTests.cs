using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FunBooksAndVideos.API.Contracts.PurchaseOrders.Request;
using FunBooksAndVideos.Persistence.Context;
using FunBooksAndVideos.Persistence.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FunBooksAndVideos.API.IntegrationTests
{
    public sealed class ShippingFulfillmentTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        public ShippingFulfillmentTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("Persistence:DatabaseName", $"ShippingTest-{Guid.NewGuid():N}");
                builder.UseEnvironment("Development");
            });
        }

        [Fact]
        public async Task PostPhysicalBookOrder_CreatesShippingSlip()
        {
            // Arrange
            using var client = factory.CreateClient();
            var request = new CreatePurchaseOrderRequest(
                SeedData.CustomerId,
                [new(SeedData.BookId, "product")]);

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FunBooksAndVideosDbContext>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            dbContext.ShippingSlips.Should().ContainSingle(slip =>
                slip.ProductId == SeedData.BookId && slip.Status == Domain.Enums.ShippingSlipStatus.Created);
        }

        [Fact]
        public async Task PostDigitalVideoOrder_DoesNotCreateShippingSlip()
        {
            // Arrange
            using var client = factory.CreateClient();
            var request = new CreatePurchaseOrderRequest(
                SeedData.CustomerId,
                [new(SeedData.VideoId, "product")]);

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FunBooksAndVideosDbContext>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            dbContext.ShippingSlips.Should().BeEmpty();
        }
    }
}
