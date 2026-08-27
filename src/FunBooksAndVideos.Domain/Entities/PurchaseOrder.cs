using FunBooksAndVideos.Domain.Enums;

namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class PurchaseOrder
    {
        private PurchaseOrder()
        {
        }

        public static PurchaseOrder Create(Guid customerId, decimal totalPrice, IReadOnlyCollection<PurchaseOrderItem> itemLines) 
            => new PurchaseOrder
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                TotalPrice = totalPrice,
                Status = PurchaseOrderStatus.Processed,
                Items = itemLines.ToList()
            };

        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public decimal TotalPrice { get; private set; }

        public PurchaseOrderStatus Status { get; private set; }

        public List<PurchaseOrderItem> Items { get; private set; } = [];
    }
}
