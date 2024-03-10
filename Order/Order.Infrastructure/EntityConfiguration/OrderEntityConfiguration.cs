using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entity;

namespace Order.Infrastructure.EntityConfiguration
{
    internal class OrderEntityConfiguration : IEntityTypeConfiguration<Domain.Entity.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.Order> builder)
        {
            builder.Property(x => x.OrderDate).IsRequired(true);
            builder.Property(x => x.OrderType).IsRequired(true);

            builder.HasMany(order => order.OrderItems)
                  .WithOne()
                  .HasForeignKey(x => x.OrderId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
