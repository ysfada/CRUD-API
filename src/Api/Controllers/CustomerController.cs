using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Customers.Model;
using Application.Customers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class CustomerController : BaseController
	{
		private readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CustomerDto>> GetCustomerAsync(int id)
		{
			var data = await _customerService.GetCustomerAsync(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public async Task<IEnumerable<CustomerDto>> GetCustomersAsync([FromQuery(Name = "isActive")] short? isActive)
		{
			return await _customerService.GetCustomersAsync(isActive);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
		{
			var customer = await _customerService.CreateCustomerAsync(createCustomerDto);

			return CreatedAtAction(nameof(GetCustomerAsync), new {id = customer.Id}, customer);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
		{
			var existingCustomer = await _customerService.GetCustomerAsync(id);

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

			await _customerService.UpdateCustomerAsync(updatedCustomer);

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteCustomerAsync(int id)
		{
			var existingCustomer = await _customerService.GetCustomerAsync(id);

			if (existingCustomer is null)
			{
				return NotFound();
			}

			await _customerService.DeleteCustomerAsync(id);

			return NoContent();
		}
	}
}