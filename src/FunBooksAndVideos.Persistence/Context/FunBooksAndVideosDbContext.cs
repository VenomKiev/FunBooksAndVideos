using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Context
{
    public sealed class FunBooksAndVideosDbContext(DbContextOptions<FunBooksAndVideosDbContext> options)
        : DbContext(options)
    {
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

        public DbSet<Membership> Memberships => Set<Membership>();

        public DbSet<ShippingSlip> ShippingSlips => Set<ShippingSlip>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FunBooksAndVideosDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
