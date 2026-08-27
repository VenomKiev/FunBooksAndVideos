using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Services
{
    public interface IMembershipActivationService
    {
        MembershipActivationResult Activate(Customer customer, MembershipType membershipType);
    }
}
