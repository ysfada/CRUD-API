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
		public ActionResult<OrderDto> GetOrder(int id)
		{
			var data = _orderService.GetOrder(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpPost]
		public ActionResult<OrderDto> CreateOrder(CreateOrderDto createOrderDto)
		{
			var order = _orderService.CreateOrder(createOrderDto);

			return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
		}
	}
}