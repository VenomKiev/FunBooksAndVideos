using FunBooksAndVideos.Domain.Entities;

namespace FunBooksAndVideos.Domain.Services
{
    public sealed class ShippingSlipService : IShippingSlipService
    {
        public ShippingSlipResult CreateForPhysicalProduct(PurchaseOrder order, Product product)
        {
            if (!product.IsPhysical)
            {
                return ShippingSlipResult.Failure("PRODUCT_NOT_PHYSICAL", "A shipping slip can only be created for a physical product.");
            }

            return ShippingSlipResult.Success(ShippingSlip.Create(order.Id, product.Id));
        }
    }

    public sealed record ShippingSlipResult(
        bool IsSuccess,
        string? ErrorCode,
        string? ErrorMessage,
        ShippingSlip? ShippingSlip)
    {
        public static ShippingSlipResult Success(ShippingSlip shippingSlip)
            => new(true, null, null, shippingSlip);

        public static ShippingSlipResult Failure(string errorCode, string errorMessage)
            => new(false, errorCode, errorMessage, null);
    };
}
