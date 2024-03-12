using Order.Application.DTO;
using Order.Domain.Entity;

namespace Order.Application.Mapper
{
    public static class OrderMapper
    {
        public static Domain.Entity.Order ToOrder(this OrderDTO DTO)
            => new Domain.Entity.Order
            {
                OrderDate = DateTime.SpecifyKind(DTO.OrderDate, DateTimeKind.Utc),
                OrderType = DTO.OrderType,
                OrderItems = DTO.OrderItems?.Select(item => new OrderItem(item.ProductId, item.Quantity, item.Price)).ToList() ?? new List<OrderItem>()
            };

        public static OrderResponseDTO ToOrderResponse(this Domain.Entity.Order reqest)
           => new OrderResponseDTO
           {
               OrderId= reqest.Id,
               OrderDate = reqest.OrderDate.ToString("yyyy-MM-dd"),
               CreatedAt = reqest.CreatedAt.ToString("yyyy-MM-dd"),
               OrderType = reqest.OrderType.ToString(),
               OrderItems = reqest.OrderItems?.Select(item => new OrderItemDTO { Id=item.Id, Price = item.Price, ProductId = item.ProductId, Quantity = item.Quantity}).ToList() ?? new List<OrderItemDTO>()
           };
    }
}
