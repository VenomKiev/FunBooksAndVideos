using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Domain.Services
{
    public sealed class ShippingSlipService
    {
        public ShippingSlipResult CreateForPhysicalProduct(PurchaseOrder order, Product product)
        {
            if (!product.IsPhysical)
            {
                return new(false, "PRODUCT_NOT_PHYSICAL", "A shipping slip can only be created for a physical product.", null);
            }

            return new(true, null, null, new ShippingSlip(Guid.NewGuid(), order.Id, product.Id));
        }
    }

    public sealed record ShippingSlipResult(
        bool IsSuccess,
        string? ErrorCode,
        string? ErrorMessage,
        ShippingSlip? ShippingSlip);
}
