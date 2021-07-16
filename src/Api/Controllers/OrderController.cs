using System;
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

		[HttpPost]
		public IActionResult CreateOrder(CreateOrderDto order)
		{
			// TODO: input validation

			var data = _orderService.CreateOrder(order);

			if (data is null)
			{
				return BadRequest();
			}

			return Created((new Uri($"{Request.Path}/{data.Id}", UriKind.Relative)), data);
		}
	}
}