using System.Collections.Generic;
using System.Linq;
using Application.Orders.Model;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Application.Orders.Services
{
	public class OrderService : IOrderService
	{
		private readonly IRepository<Order> _orderRepository;

		public OrderService(IRepository<Order> orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public OrderDto GetOrderById(int id)
		{
			var order = _orderRepository.GetById(id);
			if (order is null)
			{
				return null;
			}

			return new OrderDto()
			{
				Id = order.Id,
				CustomerId = order.CustomerId,
				ProductId = order.ProductId,
				Quantity = order.Quantity
			};
		}

		public List<OrderDto> GetCustomerOrders(int customerId)
		{
			var orders = _orderRepository.GetAll.Where(order => order.CustomerId == customerId).Select(f =>
				new OrderDto()
				{
					Id = f.Id,
					CustomerId = f.CustomerId,
					ProductId = f.ProductId,
					Quantity = f.Quantity,
					IsActive = f.IsActive,
				});
			return orders.ToList();
		}

		public OrderDto CreateOrder(CreateOrderDto order)
		{
			var newOrder = _orderRepository.InsertWithoutCommit(new Order()
			{
				CustomerId = order.CustomerId,
				ProductId = order.ProductId,
				Quantity = order.Quantity
			});

			_orderRepository.Commit();

			return new OrderDto
			{
				Id = newOrder.Id,
				CustomerId = newOrder.CustomerId,
				ProductId = newOrder.ProductId,
				Quantity = newOrder.Quantity,
			};
		}
	}
}