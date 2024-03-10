namespace Order.Domain.Entity
{
    public class OrderItem : BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderItem(long productId, decimal quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            CreatedAt = DateTime.UtcNow;
        }
    }
   
}
