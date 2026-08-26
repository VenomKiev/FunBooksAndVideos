using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class ShippingSlip
    {
        private ShippingSlip()
        {
        }

        public ShippingSlip(Guid id, Guid purchaseOrderId, Guid productId)
        {
            Id = id;
            PurchaseOrderId = purchaseOrderId;
            ProductId = productId;
            Status = ShippingSlipStatus.Created;
        }

        public Guid Id { get; private set; }

        public Guid PurchaseOrderId { get; private set; }

        public Guid ProductId { get; private set; }

        public ShippingSlipStatus Status { get; private set; }
    }
}
