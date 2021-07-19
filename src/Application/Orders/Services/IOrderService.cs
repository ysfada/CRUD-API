using Application.Orders.Model;

namespace Application.Orders.Services
{
	public interface IOrderService
	{
		OrderDto GetOrder(int id);
		OrderDto CreateOrder(CreateOrderDto createOrderDto);
	}
}