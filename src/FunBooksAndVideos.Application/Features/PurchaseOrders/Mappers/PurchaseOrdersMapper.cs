using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Mappers
{
    internal static class PurchaseOrdersMapper
    {
        public static CreatePurchaseOrderResult ToSuccessResult(this PurchaseOrder order, CreatePurchaseOrderItemResult[]? items)
            => new(true, order.Id, order.CustomerId, order.TotalPrice, order.Status.ToString(), items, null, null);

        public static CreatePurchaseOrderItemResult ToItemResult(this PurchaseOrderItem item)
            => new(item.Id, item.ItemType, item.ItemId, item.Quantity, item.UnitPrice);
    }
}
