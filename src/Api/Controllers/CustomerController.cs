using System.Collections.Generic;
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
		public ActionResult<CustomerDto> GetCustomer(int id)
		{
			var data = _customerService.GetCustomer(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public IEnumerable<CustomerDto> GetCustomers([FromQuery(Name = "isActive")] short? isActive)
		{
			return _customerService.GetCustomers(isActive);
		}

		[HttpPost]
		public ActionResult<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto)
		{
			var customer = _customerService.CreateCustomer(createCustomerDto);

			return CreatedAtAction(nameof(GetCustomer), new {id = customer.Id}, customer);
		}

		[HttpPut("{id:int}")]
		public IActionResult UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
		{
			var existingCustomer = _customerService.GetCustomer(id);

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

			_customerService.UpdateCustomer(updatedCustomer);

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteCustomer(int id)
		{
			var existingCustomer = _customerService.GetCustomer(id);

			if (existingCustomer is null)
			{
				return NotFound();
			}

			_customerService.DeleteCustomer(id);

			return NoContent();
		}
	}
}