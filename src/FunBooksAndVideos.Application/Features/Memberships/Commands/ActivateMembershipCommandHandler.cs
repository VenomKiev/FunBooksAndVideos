using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Services;
using MediatR;

namespace FunBooksAndVideos.Application.Features.Memberships.Commands
{
    public sealed class ActivateMembershipCommandHandler(
        ICustomerRepository customerRepository,
        MembershipActivationService activationService,
        IMembershipRepository membershipRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<ActivateMembershipCommand, ActivateMembershipResult>
    {
        public async Task<ActivateMembershipResult> Handle(
            ActivateMembershipCommand command,
            CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer is null)
            {
                return new(false, "CUSTOMER_NOT_FOUND", "The customer was not found.");
            }

            var activation = activationService.Activate(customer, command.MembershipType);
            if (!activation.IsSuccess)
            {
                return new(false, activation.ErrorCode, activation.ErrorMessage);
            }

            await membershipRepository.AddAsync(activation.Membership!, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new(true, null, null);
        }
    }
}
