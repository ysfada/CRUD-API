using System.Collections.Generic;
using Application.Orders.Model;

namespace Application.Orders.Services
{
	public interface IOrderService
	{
		OrderDto GetOrderById(int id);
		List<OrderDto> GetCustomerOrders(int customerId);
		OrderDto CreateOrder(CreateOrderDto order);
	}
}