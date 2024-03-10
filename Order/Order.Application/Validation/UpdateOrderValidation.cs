using FluentValidation;
using Order.Application.DTO;
using Order.Application.Infrastructure;

namespace Order.Application.Validation
{
    public record UpdateOrderValidationRequest(UpdateOrderDTO OrderDTO, long id);
    internal class UpdateOrderValidation:AbstractValidator<UpdateOrderValidationRequest>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderValidation(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            RuleFor(x => x.id).Equal(x => x.OrderDTO.OrderId).WithMessage("Invalid order");

            RuleFor(x => x.id).MustAsync(OrderExist)
          .WithMessage("Invalid order");

            RuleForEach(order => order.OrderDTO.OrderItems)
           .ChildRules(item =>
           {
               item.RuleFor(orderItem => orderItem.ProductId)
                   .GreaterThan(0)
                       .WithMessage("Invalid product");

               item.RuleFor(orderItem => orderItem.Quantity)
                   .GreaterThanOrEqualTo(0)
                       .WithMessage("Invalid quantity");

               item.RuleFor(orderItem => orderItem.Price)
                   .GreaterThanOrEqualTo(0)
                       .WithMessage("Invalid price");
           });
        }

        private async Task<bool> OrderExist(long id, CancellationToken cancellationToken)
        {
            var test= await _orderRepository.Exist(x => x.Id == id && x.IsActive);
            return test;
        }
    }
}
