using System.Net;
using System.Net.Http.Json;
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
        var request = new CreatePurchaseOrderRequest(
            SeedData.CustomerId,
            [
                new(SeedData.VideoId, "product"),
                new(SeedData.BookId, "product"),
                new(SeedData.BookClubMembershipId, "membership")
            ]);

        var response = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);
        var body = await response.Content.ReadFromJsonAsync<CreatePurchaseOrderResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(body);
        Assert.Equal(54.97m, body.TotalPrice);
        Assert.Equal(3, body.Items.Count);
        Assert.NotEmpty(response.Headers.GetValues("X-Correlation-ID"));
    }

    [Fact]
    public async Task PostOrderWithoutItems_ReturnsProblemDetails()
    {
        var response = await client.PostAsJsonAsync(
            "/api/v1/purchase-orders",
            new CreatePurchaseOrderRequest(SeedData.CustomerId, []));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
}
