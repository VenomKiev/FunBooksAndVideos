namespace FunBooksAndVideos.API.Contracts.PurchaseOrders.Response
{
    public sealed record CreatePurchaseOrderResponse(
        Guid Id,
        Guid CustomerId,
        decimal TotalPrice,
        string Status,
        IReadOnlyCollection<OrderItem> Items);
}
