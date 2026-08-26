using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Persistence.Context;

namespace FunBooksAndVideos.Persistence.Seed
{
    public static class SeedData
    {
        public static readonly Guid CustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid VideoId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid BookId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        public static readonly Guid BookClubMembershipId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        public static readonly Guid VideoClubMembershipId = Guid.Parse("55555555-5555-5555-5555-555555555555");

        public static void AddInitialData(FunBooksAndVideosDbContext dbContext)
        {
            if (dbContext.Customers.Any())
            {
                return;
            }

            dbContext.Customers.Add(new Customer(CustomerId, "Alex Customer"));
            dbContext.Products.AddRange(
                new Product(VideoId, "Comprehensive First Aid Training", ProductType.Video, 29.99m, false),
                new Product(BookId, "The Girl on the Train", ProductType.Book, 14.99m, true),
                new Product(BookClubMembershipId, "Book Club Membership", ProductType.Membership, 9.99m, false),
                new Product(VideoClubMembershipId, "Video Club Membership", ProductType.Membership, 12.99m, false));
        }
    }
}
