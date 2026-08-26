using FunBooksAndVideos.API.Contracts.PurchaseOrders.Response;
using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;

namespace FunBooksAndVideos.API.Mappers
{
    public static class PurchaseOrderMapper
    {
        public static Contracts.PurchaseOrders.Response.CreatePurchaseOrderResponse ToResponse(this CreatePurchaseOrderResult purchaseOrder)
        {
            return new Contracts.PurchaseOrders.Response.CreatePurchaseOrderResponse(
                purchaseOrder.OrderId!.Value,
                purchaseOrder.CustomerId!.Value,
                purchaseOrder.TotalPrice!.Value,
                purchaseOrder.Status!,
                purchaseOrder.ItemLines!
                    .Select(item => new OrderItem(
                        item.Id,
                        item.ItemType,
                        item.ItemId,
                        item.Quantity,
                        item.UnitPrice))
                    .ToArray());
        }
    }
}
