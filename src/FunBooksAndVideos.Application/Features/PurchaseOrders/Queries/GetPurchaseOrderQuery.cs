using MediatR;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Queries
{
    public sealed record GetPurchaseOrderQuery(Guid PurchaseOrderId) : IRequest<GetPurchaseOrderResult?>;

    public sealed record GetPurchaseOrderResult(Guid Id, Guid CustomerId, decimal TotalPrice, string Status);
}
