using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Persistence.Configuration
{
    internal sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.HasKey(order => order.Id);
            builder.Property(order => order.TotalPrice)
                .HasPrecision(18, 2);
            builder.Property(order => order.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasMany(order => order.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
