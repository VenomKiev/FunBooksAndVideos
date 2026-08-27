using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Persistence.Configuration
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(product => product.Id);
            builder.Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(300);
            builder.Property(product => product.Type)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(product => product.Price)
                .HasPrecision(18, 2);
            builder.Property(product => product.MembershipType)
                .HasConversion<string>();
        }
    }
}
