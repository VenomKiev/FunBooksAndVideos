using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Services
{
    public sealed class MembershipActivationService : IMembershipActivationService
    {
        public MembershipActivationResult Activate(Customer customer, MembershipType membershipType)
        {
            if (customer.Memberships.Any(membership =>
                membership.IsActive && membership.MembershipType == membershipType))
            {
                return MembershipActivationResult.Error("DUPLICATE_ACTIVE_MEMBERSHIP", "The customer already has an active membership of this type.");
            }

            var membership = Membership.Create(customer.Id, membershipType);
            membership.Activate(DateTimeOffset.UtcNow);
            customer.AddMembership(membership);

            return MembershipActivationResult.Success(membership);
        }
    }

    public sealed record MembershipActivationResult(
        bool IsSuccess,
        string? ErrorCode,
        string? ErrorMessage,
        Membership? Membership)
    {
        public static MembershipActivationResult Success(Membership membership)
            => new(true, null, null, membership);

        public static MembershipActivationResult Error(string errorCode, string errorMessage)
            => new(false, errorCode, errorMessage, null);
    };
}
