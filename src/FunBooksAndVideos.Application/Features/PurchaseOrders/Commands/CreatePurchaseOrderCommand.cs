using MediatR;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Commands
{
    public sealed record CreatePurchaseOrderItem(Guid ItemId, string ItemType, int Quantity);

    public sealed record CreatePurchaseOrderCommand(
        Guid CustomerId,
        IReadOnlyCollection<CreatePurchaseOrderItem> Items) : IRequest<CreatePurchaseOrderResult>;

    public sealed record CreatePurchaseOrderResult(
        bool IsSuccess,
        Guid? OrderId,
        Guid? CustomerId,
        decimal? TotalPrice,
        string? Status,
        IReadOnlyCollection<CreatePurchaseOrderLineResult>? ItemLines,
        string? ErrorCode,
        string? ErrorMessage);

    public sealed record CreatePurchaseOrderLineResult(
        Guid Id,
        string ItemType,
        Guid ItemId,
        int Quantity,
        decimal UnitPrice);
}
