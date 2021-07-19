using System.Collections.Generic;
using Application.Orders.Model;

namespace Application.Orders.Services
{
	public interface IOrderService
	{
		OrderDto GetOrder(int id, short? isActive = null);
		IEnumerable<OrderDto> GetOrders(short? isActive = null);
		OrderDto CreateOrder(CreateOrderDto createOrderDto);
		void UpdateOrder(int id, UpdateOrderDto updateOrderDto);
		void DeleteOrder(int id);
	}
}