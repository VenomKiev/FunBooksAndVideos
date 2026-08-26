namespace FunBooksAndVideos.API.Contracts.PurchaseOrders.Request
{
    public sealed record CreatePurchaseOrderRequest(
        Guid CustomerId,
        IReadOnlyCollection<OrderItem> Items);
}
