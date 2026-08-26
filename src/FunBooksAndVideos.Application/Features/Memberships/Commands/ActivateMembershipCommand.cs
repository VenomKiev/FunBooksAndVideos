using FunBooksAndVideos.Domain.Enums;
using MediatR;

namespace FunBooksAndVideos.Application.Features.Memberships.Commands
{
    public sealed record ActivateMembershipCommand(Guid CustomerId, MembershipType MembershipType)
        : IRequest<ActivateMembershipResult>;

    public sealed record ActivateMembershipResult(
        bool IsSuccess,
        string? ErrorCode,
        string? ErrorMessage);
}
