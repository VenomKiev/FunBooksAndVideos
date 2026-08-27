using FunBooksAndVideos.Application.Features.PurchaseOrders.Validations;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Services;

namespace FunBooksAndVideos.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPurchaseOrderValidationService, PurchaseOrderValidationService>();
            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IMembershipActivationService, MembershipActivationService>();
            services.AddScoped<IShippingSlipService, ShippingSlipService>();
            return services;
        }
    }
}
