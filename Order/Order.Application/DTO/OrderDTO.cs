using Order.Domain.Enum;

namespace Order.Application.DTO
{
    public class OrderDTO
    {
        public OrderType OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class UpdateOrderDTO
    {
        public long OrderId { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderResponseDTO
    {
        public long OrderId { get; set; }
        public string OrderType { get; set; }
        public string OrderDate { get; set; }
        public string CreatedAt { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateAvailableQuantityDTO
    {
        public decimal Quantity { get; set; }
        public int Type { get; set; }
    }
}
