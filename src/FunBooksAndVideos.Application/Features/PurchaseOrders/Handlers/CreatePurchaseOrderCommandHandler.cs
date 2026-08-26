using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Application.Services;
using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Handlers
{
    public sealed class CreatePurchaseOrderCommandHandler(
        ICustomerRepository customerRepository,
        IPurchaseOrderRepository purchaseOrderRepository,
        PurchaseOrderValidationService validationService,
        IUnitOfWork unitOfWork,
        ILogger<CreatePurchaseOrderCommandHandler> logger)
        : IRequestHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResult>
    {
        public async Task<CreatePurchaseOrderResult> Handle(
            CreatePurchaseOrderCommand command,
            CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer is null)
            {
                return new(false, null, null, null, null, null, "CUSTOMER_NOT_FOUND", "The customer was not found.");
            }

            var validation = await validationService.ValidateAsync(command, cancellationToken);
            if (!validation.IsValid)
            {
                return new(false, null, null, null, null, null, validation.Code, validation.Message);
            }

            var lines = validation.Items
                .Select(item => new PurchaseOrderLine(
                    Guid.NewGuid(),
                    item.Product.Id,
                    item.Request.ItemType.ToLowerInvariant(),
                    item.Request.Quantity,
                    item.Product.Price))
                .ToList();
            var order = new PurchaseOrder(Guid.NewGuid(), customer.Id, lines.Sum(line => line.LineTotal), lines);

            await purchaseOrderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Purchase order {PurchaseOrderId} created for customer {CustomerId} with total {TotalPrice}",
                order.Id,
                customer.Id,
                order.TotalPrice);

            var resultLines = lines
                .Select(line => new CreatePurchaseOrderLineResult(
                    line.Id,
                    line.ItemType,
                    line.ItemId,
                    line.Quantity,
                    line.UnitPrice))
                .ToArray();

            return new(true, order.Id, order.CustomerId, order.TotalPrice, order.Status.ToString(), resultLines, null, null);
        }
    }
}
