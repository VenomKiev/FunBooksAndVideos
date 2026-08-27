using System;
using System.Collections.Generic;
using System.Text;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Commands
{
    public sealed record CreatePurchaseOrderResult(
        bool IsSuccess,
        Guid? OrderId,
        Guid? CustomerId,
        decimal? TotalPrice,
        string? Status,
        IReadOnlyCollection<CreatePurchaseOrderItemResult>? Items,
        string? ErrorCode,
        string? ErrorMessage)
    {
        public static CreatePurchaseOrderResult Error(string? code, string? message) 
            => new(false, null, null, null, null, null, code, message);
    }

    public sealed record CreatePurchaseOrderItemResult(
        Guid Id,
        string ItemType,
        Guid ItemId,
        int Quantity,
        decimal UnitPrice);
}
