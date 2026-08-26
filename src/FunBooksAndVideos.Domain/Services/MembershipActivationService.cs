using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Services
{
    public sealed class MembershipActivationService
    {
        public MembershipActivationResult Activate(Customer customer, MembershipType membershipType)
        {
            if (membershipType == MembershipType.Premium)
            {
                return new(false, "PREMIUM_DERIVED", "Premium membership is derived from Book Club and Video Club memberships.", null);
            }

            if (customer.Memberships.Any(membership =>
                membership.IsActive && membership.MembershipType == membershipType))
            {
                return new(false, "DUPLICATE_ACTIVE_MEMBERSHIP", "The customer already has an active membership of this type.", null);
            }

            var membership = new Membership(Guid.NewGuid(), customer.Id, membershipType);
            membership.Activate(DateTimeOffset.UtcNow);
            customer.AddMembership(membership);
            return new(true, null, null, membership);
        }
    }

    public sealed record MembershipActivationResult(
        bool IsSuccess,
        string? ErrorCode,
        string? ErrorMessage,
        Membership? Membership);
}
