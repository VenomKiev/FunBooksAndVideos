namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class Customer
    {
        private Customer()
        {
        }

        public Customer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public List<Membership> Memberships { get; private set; } = [];

        public bool IsPremium => Memberships.Any(membership =>
            membership.IsActive && membership.MembershipType == Enums.MembershipType.BookClub)
            && Memberships.Any(membership =>
                membership.IsActive && membership.MembershipType == Enums.MembershipType.VideoClub);

        public void AddMembership(Membership membership)
        {
            Memberships.Add(membership);
        }

        public void RemoveMembership(Membership membership)
        {
            Memberships.Remove(membership);
        }
    }
}
