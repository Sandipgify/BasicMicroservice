using Confluent.Kafka;
using FluentValidation;
using Order.Application.DTO;
using Order.Application.Infrastructure;
using Order.Application.Interface;
using Order.Application.Mapper;
using Order.Application.Provider.Interfaces;
using Order.Application.Validation;
using Order.Domain.Entity;
using Newtonsoft;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IKafkaProducerProvider _kafkaProducerProvider;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IKafkaProducerProvider kafkaProducerProvider)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _kafkaProducerProvider = kafkaProducerProvider;
        }

        public async Task<long> Create(OrderDTO requestDTO)
        {
            #region Validation
            var validation = new CreateOrderValidation();
            await validation.ValidateAndThrowAsync(requestDTO);
            #endregion

            var order = requestDTO.ToOrder();
            order.CreatedAt = DateTime.UtcNow;
            order.IsActive =true;
            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveAsync();

            using var producer = _kafkaProducerProvider.GetProducer<string, string>();

            foreach (var item in order.OrderItems) {
                var value = JsonConvert.SerializeObject(new UpdateAvailableQuantityDTO { Quantity = item.Quantity, Type = (int)order.OrderType });
                var result = await producer.ProduceAsync("ItemOrdered", new Message<string, string> { Key = item.ProductId.ToString(), Value =  value });
                Console.WriteLine($"Item {item.ProductId} {item.Quantity} Ordered message sent");
            }

            return order.Id;
        }

        public async Task Update(UpdateOrderDTO requestDTO, long id)
        {
            #region Validation
            var validation = new UpdateOrderValidation(_orderRepository);
            await validation.ValidateAndThrowAsync(new UpdateOrderValidationRequest(requestDTO, id));
            #endregion

            var order = await _orderRepository.GetById(id);

            order.OrderDate = DateTime.SpecifyKind(requestDTO.OrderDate, DateTimeKind.Utc);
            order.OrderType = requestDTO.OrderType;

            foreach (var updatedOrderItem in requestDTO.OrderItems)
            {
                UpdateOrAddOrderItem(order, updatedOrderItem);
            }

            _orderRepository.Update(order);

            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(long orderId)
        {
            #region Validation
            var validation = new DeleteOrderValidation(_orderRepository);
            await validation.ValidateAndThrowAsync(orderId);
            #endregion

            var order = await _orderRepository.GetById(orderId);
            order.IsActive = false;
            _orderRepository.Update(order);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<OrderResponseDTO>> Get()
        {
            var orders = await _orderRepository.GetAll();
            var orderResponses = orders.Select(x => x.ToOrderResponse());
            return orderResponses;
        }

        private static void UpdateOrAddOrderItem(Domain.Entity.Order order, OrderItemDTO updatedOrderItem)
        {
            var existingOrderItem = order.OrderItems.FirstOrDefault(item => item.Id == updatedOrderItem.Id);

            if (existingOrderItem == null)
            {
                order.OrderItems.Add(new OrderItem(updatedOrderItem.ProductId, updatedOrderItem.Quantity, updatedOrderItem.Price));                
            }
            else
            {
                existingOrderItem.ProductId = updatedOrderItem.ProductId;
                existingOrderItem.Quantity = updatedOrderItem.Quantity;
                existingOrderItem.Price = updatedOrderItem.Price;
            }
        }
    }
}

