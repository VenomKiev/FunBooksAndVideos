namespace FunBooksAndVideos.API.Contracts.PurchaseOrders.Response
{
    public sealed record OrderItem(
        Guid Id,
        string ItemType,
        Guid ItemId,
        int Quantity,
        decimal UnitPrice);
}
