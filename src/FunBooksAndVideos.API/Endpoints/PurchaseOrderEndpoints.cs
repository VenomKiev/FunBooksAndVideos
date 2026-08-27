using FunBooksAndVideos.API.Contracts.PurchaseOrders.Request;
using FunBooksAndVideos.API.Contracts.PurchaseOrders.Response;
using FunBooksAndVideos.API.Mappers;
using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using MediatR;
using OrderItem = FunBooksAndVideos.API.Contracts.PurchaseOrders.Response.OrderItem;

namespace FunBooksAndVideos.API.Endpoints
{
    public static class PurchaseOrderEndpoints
    {
        public static IEndpointRouteBuilder MapPurchaseOrderEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/api/v1/purchase-orders", CreatePurchaseOrderAsync)
                .WithName("CreatePurchaseOrder")
                .Produces<CreatePurchaseOrderResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            return endpoints;
        }

        private static async Task<IResult> CreatePurchaseOrderAsync(
            CreatePurchaseOrderRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CreatePurchaseOrderCommand(
                request.CustomerId,
                request.Items.Select(item => new CreatePurchaseOrderItem(item.ItemId, item.ItemType, item.Quantity)).ToArray());
            var result = await sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorCode == "DUPLICATE_ACTIVE_MEMBERSHIP"
                    ? StatusCodes.Status409Conflict
                    : StatusCodes.Status400BadRequest;

                return Results.Problem(
                    statusCode: statusCode,
                    title: "Purchase order validation failed.",
                    detail: result.ErrorMessage,
                    instance: "/api/v1/purchase-orders",
                    extensions: new Dictionary<string, object?> { ["code"] = result.ErrorCode });
            }
            
            var response = result.ToResponse();
            return Results.Created($"/api/v1/purchase-orders/{response.Id}", response);
        }
    }
}
