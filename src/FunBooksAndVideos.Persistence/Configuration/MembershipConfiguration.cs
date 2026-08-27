using FunBooksAndVideos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunBooksAndVideos.Persistence.Configuration
{
    internal sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(membership => membership.Id);
            builder.Property(membership => membership.MembershipType)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(membership => membership.ActivatedAt);
        }
    }
}
