using System.Collections.Generic;
using System.Linq;
using Application.Orders.Model;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Services
{
	public class OrderService : IOrderService
	{
		private readonly IRepository<Order> _orderRepository;

		public OrderService(IRepository<Order> orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public OrderDto GetOrder(int id, short? isActive = null)
		{
			var order = _orderRepository.GetAllNoTracking
				.Include(o => o.Customer)
				.Include(o => o.Product)
				.FirstOrDefault(c => c.Id == id && c.IsActive == 1);
			return order?.AsDto();
		}

		public IEnumerable<OrderDto> GetOrders(short? isActive = null)
		{
			var q = _orderRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(order => order.IsActive == isActive);
			}
			var orders = q
				.Include(o => o.Customer)
				.Include(o => o.Product)
				.Select(order => order.AsDto());

			return orders.ToList();
		}

		public OrderDto CreateOrder(CreateOrderDto createOrderDto)
		{
			var newOrder = _orderRepository.InsertWithoutCommit(createOrderDto.AsOrder());

			_orderRepository.Commit();

			return newOrder.AsDto();
		}

		public void UpdateOrder(int id, UpdateOrderDto updateOrderDto)
		{
			_orderRepository.Update(updateOrderDto.AsOrder(id));
		}

		public void DeleteOrder(int id)
		{
			_orderRepository.Delete(new Order()
			{
				Id = id,
			});
		}
	}
}