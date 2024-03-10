using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entity;

namespace Order.Infrastructure.EntityConfiguration
{
    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.ProductId).IsRequired(true);
            builder.Property(x=> x.Quantity).IsRequired(true);
            builder.Property(x => x.OrderId).IsRequired(true);
            builder.Property(x => x.Price)
                 .HasPrecision(10, 2);
        }
    }
}
