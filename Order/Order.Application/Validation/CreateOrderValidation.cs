using FluentValidation;
using Order.Application.DTO;
using Order.Domain.Enum;

namespace Order.Application.Validation
{
    public class CreateOrderValidation:AbstractValidator<OrderDTO>
    {
        public CreateOrderValidation()
        {
            RuleFor(order => order.OrderType)
            .IsInEnum()
            .WithMessage("Invalid OrderType.");

            RuleForEach(order => order.OrderItems)
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
    }
}
