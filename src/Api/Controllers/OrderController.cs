using System;
using System.Collections.Generic;
using Application.Orders.Model;
using Microsoft.AspNetCore.Mvc;
using Application.Orders.Services;
using Microsoft.AspNetCore.Http;

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
		public ActionResult<OrderDto> GetOrder(int id)
		{
			var data = _orderService.GetOrder(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public IEnumerable<OrderDto> GetOrders([FromQuery(Name = "isActive")] short? isActive)
		{
			return _orderService.GetOrders(isActive);
		}

		[HttpPost]
		public ActionResult<OrderDto> CreateOrder(CreateOrderDto createOrderDto)
		{
			var order = _orderService.CreateOrder(createOrderDto);

			return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
		}

		[HttpPut("{id:int}")]
		public IActionResult UpdateOrder(int id, UpdateOrderDto updateOrderDto)
		{
			var existingOrder = _orderService.GetOrder(id);

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

			_orderService.UpdateOrder(updatedOrder);

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteOrder(int id)
		{
			var existingOrder = _orderService.GetOrder(id);

			if (existingOrder is null)
			{
				return NotFound();
			}

			_orderService.DeleteOrder(id);

			return NoContent();
		}
	}
}