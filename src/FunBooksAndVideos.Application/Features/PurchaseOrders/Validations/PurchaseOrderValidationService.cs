using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Validations
{
    public sealed class PurchaseOrderValidationService(IProductRepository productRepository) : IPurchaseOrderValidationService
    {
        public async Task<(bool IsValid, string? Code, string? Message, List<(CreatePurchaseOrderItem Request, Product Product)> Items)> ValidateAsync(
            CreatePurchaseOrderCommand command,
            Customer customer,
            CancellationToken cancellationToken)
        {
            if (command.CustomerId == Guid.Empty)
            {
                return (false, "CUSTOMER_REQUIRED", "A valid customer is required.", []);
            }

            if (command.Items is null || command.Items.Count == 0)
            {
                return (false, "ITEMS_REQUIRED", "At least one order item is required.", []);
            }

            var resolvedItems = new List<(CreatePurchaseOrderItem Item, Product Product)>();

            var requestedMembershipTypes = new HashSet<MembershipType>();

            foreach (var item in command.Items)
            {
                if (item.ItemId == Guid.Empty || item.Quantity <= 0 || string.IsNullOrWhiteSpace(item.ItemType))
                {
                    return (false, "INVALID_ITEM", "Each order item must include a valid item ID, type, and positive quantity.", []);
                }

                var product = await productRepository.GetByIdAsync(item.ItemId, cancellationToken);

                if (product is null)
                {
                    return (false, "ITEM_NOT_FOUND", $"Order item '{item.ItemId}' was not found.", []);
                }

                if (product.MembershipType is { } membershipType &&
                    (!requestedMembershipTypes.Add(membershipType) || customer.Memberships.Any(membership =>
                        membership.IsActive && membership.MembershipType == membershipType)))
                {
                    return (false, "DUPLICATE_ACTIVE_MEMBERSHIP", "The customer already has an active membership of this type.", []);
                }

                resolvedItems.Add((item, product));
            }

            return (true, null, null, resolvedItems);
        }
    }
}
