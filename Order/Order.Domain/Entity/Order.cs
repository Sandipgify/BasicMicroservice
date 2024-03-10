using Order.Domain.Enum;

namespace Order.Domain.Entity
{
    public class Order:BaseEntity
    {
        public ICollection<OrderItem> OrderItems { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsActive { get; set; }
    }
}
