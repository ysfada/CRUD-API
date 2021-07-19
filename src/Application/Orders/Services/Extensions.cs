using Application.Orders.Model;
using Domain.Entities;

namespace Application.Orders.Services
{
	public static class Extensions
	{
		public static OrderDto AsDto(this Order order)
		{
			return new()
			{
				Id = order.Id,
				CustomerId = order.CustomerId,
				ProductId = order.ProductId,
				Quantity = order.Quantity,
				IsActive = order.IsActive,
			};
		}

		public static Order AsOrder(this CreateOrderDto createOrderDto)
		{
			return new()
			{
				CustomerId = createOrderDto.CustomerId,
				ProductId = createOrderDto.ProductId,
				Quantity = createOrderDto.Quantity,
			};
		}
	}
}
