using FluentAssertions;
using FunBooksAndVideos.Persistence.Context;
using FunBooksAndVideos.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.UnitTests
{
    public sealed class SeedDataInitializerTests
    {
        [Fact]
        public async Task SeedAsync_AddsRequiredCustomerAndCatalogData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FunBooksAndVideosDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            await using var dbContext = new FunBooksAndVideosDbContext(options);
            var initializer = new SeedDataInitializer(dbContext);

            // Act
            await initializer.SeedAsync();

            // Assert
            dbContext.Customers.Should().ContainSingle();
            dbContext.Products.Should().HaveCount(4);
            dbContext.Products.Should().Contain(product => product.IsPhysical);
            dbContext.Products.Should().Contain(product => product.Type == Domain.Enums.ProductType.Video);
            dbContext.Products.Should().Contain(product => product.MembershipType == Domain.Enums.MembershipType.BookClub);
            dbContext.Products.Should().Contain(product => product.MembershipType == Domain.Enums.MembershipType.VideoClub);
        }
    }
}
