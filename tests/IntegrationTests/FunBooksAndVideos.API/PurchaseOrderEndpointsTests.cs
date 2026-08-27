using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FunBooksAndVideos.API.Contracts.PurchaseOrders.Request;
using FunBooksAndVideos.API.Contracts.PurchaseOrders.Response;
using FunBooksAndVideos.Persistence.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FunBooksAndVideos.API.IntegrationTests
{

public sealed class PurchaseOrderEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public PurchaseOrderEndpointsTests(WebApplicationFactory<Program> factory)
    {
        client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("Persistence:DatabaseName", $"Test-{Guid.NewGuid():N}");
            builder.UseEnvironment("Development");
        }).CreateClient();
    }

    [Fact]
    public async Task PostMixedOrder_ReturnsCreatedOrderWithCalculatedTotal()
    {
        // Arrange
        var request = new CreatePurchaseOrderRequest(
            SeedData.CustomerId,
            [
                new(SeedData.VideoId, "product"),
                new(SeedData.BookId, "product"),
                new(SeedData.BookClubMembershipId, "membership")
            ]);

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);
        var body = await response.Content.ReadFromJsonAsync<CreatePurchaseOrderResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        body.Should().NotBeNull();
        body!.TotalPrice.Should().Be(54.97m);
        body.Items.Should().HaveCount(3);
        body.Items.Should().Contain(item => item.ItemId == SeedData.VideoId && item.ItemName == "Comprehensive First Aid Training");
        body.Items.Should().Contain(item => item.ItemId == SeedData.BookId && item.ItemName == "The Girl on the Train");
        body.Items.Should().Contain(item => item.ItemId == SeedData.BookClubMembershipId && item.ItemName == "Book Club Membership");
        response.Headers.GetValues("X-Correlation-ID").Should().NotBeEmpty();
    }

    [Fact]
    public async Task PostOrderWithoutItems_ReturnsProblemDetails()
    {
        // Arrange
        var request = new CreatePurchaseOrderRequest(SeedData.CustomerId, []);

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/v1/purchase-orders",
            request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
    }
}
}
