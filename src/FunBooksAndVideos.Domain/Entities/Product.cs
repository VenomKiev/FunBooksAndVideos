using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class Product
    {
        private Product()
        {
        }

        public static Product Create(string name, ProductType type, decimal price, bool isPhysical, MembershipType? membershipType = null)
            => new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                Price = price,
                IsPhysical = isPhysical,
                MembershipType = membershipType,
            };

        public static Product Create(Guid id, string name, ProductType type, decimal price, bool isPhysical, MembershipType? membershipType = null)
            => new Product
            {
                Id = id,
                Name = name,
                Type = type,
                Price = price,
                IsPhysical = isPhysical,
                MembershipType = membershipType,
            };

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public ProductType Type { get; private set; }

        public decimal Price { get; private set; }

        public bool IsPhysical { get; private set; }

        public MembershipType? MembershipType { get; private set; }
    }
}
