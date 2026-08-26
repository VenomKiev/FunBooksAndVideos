using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class PurchaseOrder
    {
        private PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id, Guid customerId, decimal totalPrice, IReadOnlyCollection<PurchaseOrderLine> itemLines)
        {
            Id = id;
            CustomerId = customerId;
            TotalPrice = totalPrice;
            Status = PurchaseOrderStatus.Processed;
            ItemLines = itemLines.ToList();
        }

        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public decimal TotalPrice { get; private set; }

        public PurchaseOrderStatus Status { get; private set; }

        public List<PurchaseOrderLine> ItemLines { get; private set; } = [];
    }
}
