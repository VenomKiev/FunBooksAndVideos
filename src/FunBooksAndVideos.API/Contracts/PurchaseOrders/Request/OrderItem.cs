namespace FunBooksAndVideos.API.Contracts.PurchaseOrders.Request
{
    public sealed record OrderItem(
        Guid ItemId,
        string ItemType,
        int Quantity = 1);
}
