using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Orders.Model;

namespace Application.Orders.Services
{
	public interface IOrderService
	{
		Task<OrderDto> GetOrderAsync(int id, short? isActive = null);
		Task<IEnumerable<OrderDto>> GetOrdersAsync(short? isActive = null);
		Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
		Task UpdateOrderAsync(OrderDto orderDto);
		Task DeleteOrderAsync(int id);
	}
}