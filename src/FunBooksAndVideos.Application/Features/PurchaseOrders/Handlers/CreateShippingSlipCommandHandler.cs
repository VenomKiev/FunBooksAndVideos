using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Services;
using MediatR;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Handlers
{
    public sealed class CreateShippingSlipCommandHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IProductRepository productRepository,
        IShippingSlipRepository shippingSlipRepository,
        ShippingSlipService shippingSlipService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateShippingSlipCommand, CreateShippingSlipResult>
    {
        public async Task<CreateShippingSlipResult> Handle(
            CreateShippingSlipCommand command,
            CancellationToken cancellationToken)
        {
            var order = await purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken);
            var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);
            if (order is null || product is null)
            {
                return new(false, null, "FULFILLMENT_ITEM_NOT_FOUND", "The purchase order or product was not found.");
            }

            var result = shippingSlipService.CreateForPhysicalProduct(order, product);
            if (!result.IsSuccess)
            {
                return new(false, null, result.ErrorCode, result.ErrorMessage);
            }

            await shippingSlipRepository.AddAsync(result.ShippingSlip!, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new(true, result.ShippingSlip!.Id, null, null);
        }
    }
}
