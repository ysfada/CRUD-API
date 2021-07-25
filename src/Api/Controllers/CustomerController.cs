using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Customers.Commands;
using Application.Customers.Model;
using Application.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class CustomerController : BaseController
	{
		private readonly IMediator _mediator;

		public CustomerController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CustomerDto>> GetCustomerAsync(int id)
		{
			var data = await _mediator.Send(new GetCustomerQuery(id));
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public async Task<IEnumerable<CustomerDto>> GetCustomersAsync([FromQuery(Name = "isActive")] short? isActive)
		{
			return await _mediator.Send(new GetCustomersQuery(isActive));
		}

		[HttpPost]
		public async Task<ActionResult<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
		{
			var customer = await _mediator.Send(new CreateCustomerCommand(createCustomerDto));

			return CreatedAtAction(nameof(GetCustomerAsync), new {id = customer.Id}, customer);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
		{
			var existingCustomer = await _mediator.Send(new GetCustomerQuery(id));

			if (existingCustomer is null)
			{
				return NotFound();
			}

			var updatedCustomer = existingCustomer with
			{
				FirstName = updateCustomerDto.FirstName,
				LastName = updateCustomerDto.LastName,
				IsActive = updateCustomerDto.IsActive,
			};

			await _mediator.Send(new UpdateCustomerCommand(updatedCustomer));

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteCustomerAsync(int id)
		{
			var existingCustomer = await _mediator.Send(new GetCustomerQuery(id));

			if (existingCustomer is null)
			{
				return NotFound();
			}

			await _mediator.Send(new DeleteCustomerCommand(id));

			return NoContent();
		}
	}
}