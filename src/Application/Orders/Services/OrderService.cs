using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

		public async Task<OrderDto> GetOrderAsync(int id, short? isActive = null)
		{
			var order = await _orderRepository.GetAllNoTracking
				.Include(o => o.Customer)
				.Include(o => o.Product)
				.FirstOrDefaultAsync(c => c.Id == id && c.IsActive == 1);
			return order?.AsDto();
		}

		public async Task<IEnumerable<OrderDto>> GetOrdersAsync(short? isActive = null)
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

			return await orders.ToListAsync();
		}

		public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var newOrder = _orderRepository.InsertWithoutCommit(createOrderDto.AsOrder());

			await _orderRepository.CommitAsync(CancellationToken.None);

			return newOrder.AsDto();
		}

		public async Task UpdateOrderAsync(OrderDto orderDto)
		{
			_orderRepository.UpdateWithoutCommit(orderDto.AsOrder());
			await _orderRepository.CommitAsync(CancellationToken.None);
		}

		public async Task DeleteOrderAsync(int id)
		{
			await _orderRepository.DeleteAsync(new Order()
			{
				Id = id,
			});
		}
	}
}