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

		public OrderDto GetOrder(int id)
		{
			var order = _orderRepository.GetAllNoTracking.FirstOrDefault(c => c.Id == id && c.IsActive == 1);
			return order?.AsDto();
		}

		public OrderDto CreateOrder(CreateOrderDto createOrderDto)
		{
			var newOrder = _orderRepository.InsertWithoutCommit(createOrderDto.AsOrder());

			_orderRepository.Commit();

			return newOrder.AsDto();
		}
	}
}