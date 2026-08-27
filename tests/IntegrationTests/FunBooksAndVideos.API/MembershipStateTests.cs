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
    public sealed class MembershipStateTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        public MembershipStateTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("Persistence:DatabaseName", $"MembershipTest-{Guid.NewGuid():N}");
                builder.UseEnvironment("Development");
            });
        }

        [Fact]
        public async Task PostTwoClubMemberships_ActivatesBothAndDerivesPremium()
        {
            // Arrange
            using var client = factory.CreateClient();
            var request = new CreatePurchaseOrderRequest(
                SeedData.CustomerId,
                [
                    new(SeedData.BookClubMembershipId, "membership"),
                    new(SeedData.VideoClubMembershipId, "membership")
                ]);

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FunBooksAndVideosDbContext>();
            var customer = await dbContext.Customers.FindAsync(SeedData.CustomerId);
            var memberships = dbContext.Memberships.Where(membership => membership.CustomerId == SeedData.CustomerId).ToList();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            customer.Should().NotBeNull();
            memberships.Should().HaveCount(2);
            memberships.Should().Contain(membership => membership.MembershipType == Domain.Enums.MembershipType.BookClub && membership.IsActive);
            memberships.Should().Contain(membership => membership.MembershipType == Domain.Enums.MembershipType.VideoClub && membership.IsActive);
            customer!.IsPremium.Should().BeTrue();
        }

        [Fact]
        public async Task PostDuplicateMembership_ReturnsBadRequestAndDoesNotAddMembership()
        {
            // Arrange
            using var client = factory.CreateClient();
            var request = new CreatePurchaseOrderRequest(
                SeedData.CustomerId,
                [new(SeedData.BookClubMembershipId, "membership")]);

            // Act
            var initialResponse = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);
            var duplicateResponse = await client.PostAsJsonAsync("/api/v1/purchase-orders", request);

            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FunBooksAndVideosDbContext>();

            // Assert
            initialResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            duplicateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
            dbContext.Memberships.Where(membership => membership.CustomerId == SeedData.CustomerId).Should().ContainSingle();
        }
    }
}
