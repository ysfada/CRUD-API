using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Orders.Commands;
using Application.Orders.Model;
using Application.Orders.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Api.Controllers
{
	public class OrderController : BaseController
	{
		private readonly IMediator _mediator;

		public OrderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<OrderDto>> GetOrderAsync(int id)
		{
			var data = await _mediator.Send(new GetOrderQuery(id));
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public async Task<IEnumerable<OrderDto>> GetOrdersAsync([FromQuery(Name = "isActive")] short? isActive)
		{
			return await _mediator.Send(new GetOrdersQuery(isActive));
		}

		[HttpPost]
		public async Task<ActionResult<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
		{
			var order = await _mediator.Send(new CreateOrderCommand(createOrderDto));

			return CreatedAtAction(nameof(GetOrderAsync), new {id = order.Id}, order);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
		{
			var existingOrder = await _mediator.Send(new GetOrderQuery(id));

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

			await _mediator.Send(new UpdateOrderCommand(updatedOrder));

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var existingOrder = await _mediator.Send(new GetOrderQuery(id));

			if (existingOrder is null)
			{
				return NotFound();
			}

			await _mediator.Send(new DeleteOrderCommand(id));

			return NoContent();
		}
	}
}