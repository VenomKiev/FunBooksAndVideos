using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Persistence.Context;
using FunBooksAndVideos.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FunBooksAndVideos.Persistence.Configuration
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string databaseName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);

            services.AddDbContext<FunBooksAndVideosDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));
            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<IProductRepository, EfProductRepository>();
            services.AddScoped<IPurchaseOrderRepository, EfPurchaseOrderRepository>();
            services.AddScoped<IMembershipRepository, EfMembershipRepository>();
            services.AddScoped<IShippingSlipRepository, EfShippingSlipRepository>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }
    }
}
