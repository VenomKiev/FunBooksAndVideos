using MediatR;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Commands
{
    public sealed record CreatePurchaseOrderItem(Guid ItemId, string ItemType, int Quantity);

    public sealed record CreatePurchaseOrderCommand(
        Guid CustomerId,
        IReadOnlyCollection<CreatePurchaseOrderItem> Items) : IRequest<CreatePurchaseOrderResult>;
}
