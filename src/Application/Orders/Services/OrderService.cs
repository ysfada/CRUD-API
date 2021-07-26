using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Orders.Model;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Services
{
	public class OrderService : IOrderService
	{
		private readonly IRepository<Order> _orderRepository;
		private readonly IMapper _mapper;

		public OrderService(IRepository<Order> orderRepository, IMapper mapper)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
		}

		public async Task<OrderDto> GetOrderAsync(int id, short? isActive = null)
		{
			var order = await _orderRepository.GetAllNoTracking
				.Include(o => o.Customer)
				.Include(o => o.Product)
				.FirstOrDefaultAsync(c => c.Id == id && c.IsActive == 1);

			var orderDto = _mapper.Map<OrderDto>(order);
			return orderDto;
		}

		public async Task<IEnumerable<OrderDto>> GetOrdersAsync(short? isActive = null)
		{
			var q = _orderRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(order => order.IsActive == isActive);
			}

			var orders = await q
				.Include(o => o.Customer)
				.Include(o => o.Product)
				.ToListAsync();

			var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
			return ordersDto;
		}

		public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var createOrder = _mapper.Map<Order>(createOrderDto);
			var newOrder = _orderRepository.InsertWithoutCommit(createOrder);

			await _orderRepository.CommitAsync(CancellationToken.None);

			var orderDto = _mapper.Map<OrderDto>(newOrder);
			return orderDto;
		}

		public async Task UpdateOrderAsync(OrderDto orderDto)
		{
			var order = _mapper.Map<Order>(orderDto);

			_orderRepository.UpdateWithoutCommit(order);
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