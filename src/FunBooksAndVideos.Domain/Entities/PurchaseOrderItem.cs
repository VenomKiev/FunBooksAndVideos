namespace FunBooksAndVideos.Domain.Entities
{
    public sealed class PurchaseOrderItem
    {
        private PurchaseOrderItem()
        {
        }

        public static PurchaseOrderItem Create(Guid itemId, string itemType, int quantity, decimal unitPrice)
            => new PurchaseOrderItem
            {
                Id = Guid.NewGuid(),
                ItemId = itemId,
                ItemType = itemType,
                Quantity = quantity,
                UnitPrice = unitPrice
            };

        public Guid Id { get; private set; }

        public Guid ItemId { get; private set; }

        public string ItemType { get; private set; } = string.Empty;

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal LineTotal => Quantity * UnitPrice;
    }
}
