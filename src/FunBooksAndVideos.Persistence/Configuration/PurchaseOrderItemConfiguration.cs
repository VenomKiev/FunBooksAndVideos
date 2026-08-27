using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Persistence.Configuration
{
    internal sealed class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            builder.HasKey(item => item.Id);
            builder.Property(item => item.ItemType)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(item => item.UnitPrice)
                .HasPrecision(18, 2);
            builder.Property<Guid>("PurchaseOrderId");
        }
    }
}
