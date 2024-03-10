﻿using FluentValidation;
using Order.Application.DTO;
using Order.Application.Infrastructure;
using Order.Application.Interface;
using Order.Application.Mapper;
using Order.Application.Validation;
using Order.Domain.Entity;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
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
