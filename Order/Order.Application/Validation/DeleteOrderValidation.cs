using FluentValidation;
using Order.Application.Infrastructure;

namespace Order.Application.Validation
{
    public class DeleteOrderValidation:AbstractValidator<long>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderValidation(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            RuleFor(x => x).MustAsync(OrderExist)
         .WithMessage("Invalid Order").WithName("id");
        }

        private async Task<bool> OrderExist(long id, CancellationToken cancellationToken)
        {
            return await _orderRepository.Exist(x => x.Id == id && x.IsActive);
        }
    }
}
