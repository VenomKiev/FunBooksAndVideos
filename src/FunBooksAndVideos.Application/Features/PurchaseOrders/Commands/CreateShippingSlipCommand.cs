using MediatR;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Commands
{
    public sealed record CreateShippingSlipCommand(Guid PurchaseOrderId, Guid ProductId)
        : IRequest<CreateShippingSlipResult>;

    public sealed record CreateShippingSlipResult(
        bool IsSuccess,
        Guid? ShippingSlipId,
        string? ErrorCode,
        string? ErrorMessage);
}
