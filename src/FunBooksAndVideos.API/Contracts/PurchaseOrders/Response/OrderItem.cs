namespace FunBooksAndVideos.API.Contracts.PurchaseOrders.Response
{
    public sealed record OrderItem(
        Guid Id,
        string ItemType,
        Guid ItemId,
        string ItemName,
        int Quantity,
        decimal UnitPrice);
}
