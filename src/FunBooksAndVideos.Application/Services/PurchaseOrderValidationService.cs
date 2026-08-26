using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Application.Services
{
    public sealed class PurchaseOrderValidationService(IProductRepository productRepository)
    {
        public async Task<(bool IsValid, string? Code, string? Message, List<(CreatePurchaseOrderItem Request, Product Product)> Items)> ValidateAsync(
            CreatePurchaseOrderCommand command,
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

            var resolvedItems = new List<(CreatePurchaseOrderItem Request, Product Product)>();
            foreach (var request in command.Items)
            {
                if (request.ItemId == Guid.Empty || request.Quantity <= 0 || string.IsNullOrWhiteSpace(request.ItemType))
                {
                    return (false, "INVALID_ITEM", "Each order item must include a valid item ID, type, and positive quantity.", []);
                }

                var product = await productRepository.GetByIdAsync(request.ItemId, cancellationToken);
                var expectedType = product?.Type == ProductType.Membership ? "membership" : "product";
                if (product is null || !string.Equals(expectedType, request.ItemType, StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "ITEM_NOT_FOUND", $"Order item '{request.ItemId}' was not found or has an invalid type.", []);
                }

                resolvedItems.Add((request, product));
            }

            return (true, null, null, resolvedItems);
        }
    }
}
