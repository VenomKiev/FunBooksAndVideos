using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class Membership
    {
        private Membership()
        {
        }

        public static Membership Create(Guid customerId, MembershipType membershipType)
            => new Membership
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                MembershipType = membershipType
            };

        public void Activate(DateTimeOffset activatedAt)
        {
            IsActive = true;
            ActivatedAt = activatedAt;
        }

        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public MembershipType MembershipType { get; private set; }

        public bool IsActive { get; private set; }

        public DateTimeOffset? ActivatedAt { get; private set; }
    }
}
