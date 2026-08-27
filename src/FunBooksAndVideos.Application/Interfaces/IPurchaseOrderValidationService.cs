using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Application.Interfaces
{
    public interface IPurchaseOrderValidationService
    {
        Task<(bool IsValid, string? Code, string? Message, List<(CreatePurchaseOrderItem Request, Product Product)> Items)> ValidateAsync(
            CreatePurchaseOrderCommand command,
            Customer customer,
            CancellationToken cancellationToken);
    }
}
