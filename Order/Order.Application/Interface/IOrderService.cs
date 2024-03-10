using Order.Application.DTO;

namespace Order.Application.Interface
{
    public interface IOrderService
    {
        Task<long> Create(OrderDTO requestDTO);
        Task Update(UpdateOrderDTO requestDTO, long id);
        Task Delete(long orderId);
        Task<IEnumerable<OrderResponseDTO>> Get();
    }
}
