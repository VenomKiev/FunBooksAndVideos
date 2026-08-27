using FunBooksAndVideos.Application.Features.PurchaseOrders.Mappers;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Entities;
using FunBooksAndVideos.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideos.Application.Features.PurchaseOrders.Commands
{
    public sealed class CreatePurchaseOrderCommandHandler(
        ICustomerRepository customerRepository,
        IPurchaseOrderRepository purchaseOrderRepository,
        IPurchaseOrderValidationService validationService,
        IUnitOfWork unitOfWork,
        ILogger<CreatePurchaseOrderCommandHandler> logger,
        IMembershipActivationService membershipActivationService,
        IMembershipRepository membershipRepository,
        IShippingSlipService shippingSlipService,
        IShippingSlipRepository shippingSlipRepository)
        : IRequestHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResult>
    {
        public async Task<CreatePurchaseOrderResult> Handle(
            CreatePurchaseOrderCommand command,
            CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                return CreatePurchaseOrderResult.Error("CUSTOMER_NOT_FOUND", "The customer was not found.");
            }

            var validation = await validationService.ValidateAsync(command, customer, cancellationToken);

            if (!validation.IsValid)
            {
                return CreatePurchaseOrderResult.Error(validation.Code, validation.Message);
            }

            var items = validation.Items
                .Select(item => PurchaseOrderItem.Create(
                    item.Product.Id,
                    item.Product.Type.ToString().ToLowerInvariant(),
                    item.Request.Quantity,
                    item.Product.Price))
                .ToList();

            var order = PurchaseOrder.Create(customer.Id, items.Sum(line => line.LineTotal), items);

            var activatedMemberships = new List<Membership>();

            foreach (var membershipItem in validation.Items.Where(item => item.Product.MembershipType.HasValue))
            {
                var activation = membershipActivationService.Activate(customer, membershipItem.Product.MembershipType!.Value);
                if (!activation.IsSuccess)
                {
                    return CreatePurchaseOrderResult.Error(activation.ErrorCode, activation.ErrorMessage);
                }

                var membership = activation.Membership!;
                activatedMemberships.Add(membership);

                await membershipRepository.AddAsync(membership, cancellationToken);

                logger.LogInformation(
                    "Membership {MembershipType} activated for customer {CustomerId}",
                    membership.MembershipType,
                    customer.Id);
            }

            foreach (var physicalItem in validation.Items.Where(item => item.Product.IsPhysical))
            {
                var shippingResult = shippingSlipService.CreateForPhysicalProduct(order, physicalItem.Product);

                if (!shippingResult.IsSuccess)
                {
                    return CreatePurchaseOrderResult.Error(shippingResult.ErrorCode, shippingResult.ErrorMessage);
                }

                var shippingSlip = shippingResult.ShippingSlip!;

                await shippingSlipRepository.AddAsync(shippingSlip, cancellationToken);

                logger.LogInformation(
                    "Shipping slip {ShippingSlipId} created for purchase order {PurchaseOrderId} and product {ProductId}",
                    shippingSlip.Id,
                    order.Id,
                    physicalItem.Product.Id);
            }

            await purchaseOrderRepository.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Purchase order {PurchaseOrderId} created for customer {CustomerId} with total {TotalPrice}",
                order.Id,
                customer.Id,
                order.TotalPrice);

            var resultItems = items
                .Select(item => item.ToItemResult())
                .ToArray();

            return order.ToSuccessResult(resultItems);
        }
    }
}
