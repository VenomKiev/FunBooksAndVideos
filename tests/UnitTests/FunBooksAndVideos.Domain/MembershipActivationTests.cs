using FluentAssertions;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;
using FunBooksAndVideos.Domain.Services;

namespace FunBooksAndVideos.Domain.UnitTests
{
    public sealed class MembershipActivationTests
    {
        [Fact]
        public void Activate_NewClubMembership_ActivatesImmediately()
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Test Customer");
            var service = new MembershipActivationService();

            // Act
            var result = service.Activate(customer, MembershipType.BookClub);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Membership.Should().NotBeNull();
            result.Membership!.IsActive.Should().BeTrue();
            result.Membership.ActivatedAt.Should().NotBeNull();
            customer.Memberships.Should().ContainSingle();
        }

        [Fact]
        public void Activate_BothClubMemberships_DerivesPremiumStatus()
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Test Customer");
            var service = new MembershipActivationService();

            // Act
            service.Activate(customer, MembershipType.BookClub);
            service.Activate(customer, MembershipType.VideoClub);

            // Assert
            customer.IsPremium.Should().BeTrue();
        }

        [Fact]
        public void Activate_ExistingActiveMembership_ReturnsDuplicateFailure()
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Test Customer");
            var service = new MembershipActivationService();
            service.Activate(customer, MembershipType.BookClub);

            // Act
            var result = service.Activate(customer, MembershipType.BookClub);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorCode.Should().Be("DUPLICATE_ACTIVE_MEMBERSHIP");
            customer.Memberships.Should().ContainSingle();
        }
    }
}
