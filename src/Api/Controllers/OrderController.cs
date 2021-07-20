using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Orders.Model;
using Microsoft.AspNetCore.Mvc;
using Application.Orders.Services;

namespace Api.Controllers
{
	public class OrderController : BaseController
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<OrderDto>> GetOrderAsync(int id)
		{
			var data = await _orderService.GetOrderAsync(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public async Task<IEnumerable<OrderDto>> GetOrdersAsync([FromQuery(Name = "isActive")] short? isActive)
		{
			return await _orderService.GetOrdersAsync(isActive);
		}

		[HttpPost]
		public async Task<ActionResult<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var order = await _orderService.CreateOrderAsync(createOrderDto);

			return CreatedAtAction(nameof(GetOrderAsync), new { id = order.Id }, order);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
		{
			var existingOrder = await _orderService.GetOrderAsync(id);

			if (existingOrder is null)
			{
				return NotFound();
			}

			var updatedOrder = existingOrder with
			{
				CustomerId = updateOrderDto.CustomerId,
				ProductId = updateOrderDto.ProductId,
				Quantity = updateOrderDto.Quantity,
			};

			await _orderService.UpdateOrderAsync(updatedOrder);

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var existingOrder = await _orderService.GetOrderAsync(id);

			if (existingOrder is null)
			{
				return NotFound();
			}

			await _orderService.DeleteOrderAsync(id);

			return NoContent();
		}
	}
}