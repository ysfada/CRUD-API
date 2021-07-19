using Application.Customers.Services;
using Application.Orders.Model;
using Application.Products.Services;
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
				Customer = order.Customer?.AsDto(),
				Product = order.Product?.AsDto(),
			};
		}

		public static Order AsOrder(this UpdateOrderDto updateOrderDto, int id)
		{
			return new()
			{
				Id = id,
				Quantity = updateOrderDto.Quantity,
				CustomerId = updateOrderDto.CustomerId,
				ProductId = updateOrderDto.ProductId,
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