using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Persistence.Configuration
{
    internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(customer => customer.Memberships)
                .WithOne()
                .HasForeignKey(membership => membership.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
