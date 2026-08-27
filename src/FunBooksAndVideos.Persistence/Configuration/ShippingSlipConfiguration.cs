using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Persistence.Configuration
{
    internal sealed class ShippingSlipConfiguration : IEntityTypeConfiguration<ShippingSlip>
    {
        public void Configure(EntityTypeBuilder<ShippingSlip> builder)
        {
            builder.HasKey(shippingSlip => shippingSlip.Id);
            builder.Property(shippingSlip => shippingSlip.Status)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}
