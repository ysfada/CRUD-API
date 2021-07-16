﻿using System;
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
		public IActionResult Get(int id)
		{
			var data = _customerService.GetCustomerById(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var data = _customerService.GetCustomers();
			return Ok(data);
		}

		[HttpPost]
		public IActionResult Create(CreateCustomerDto customer)
		{
			// TODO: validate input and return appropriate status code

			var data = _customerService.CreateCustomer(customer);
			if (data is null)
			{
				return BadRequest();
			}

			return Created((new Uri($"{Request.Path}/{data.Id}", UriKind.Relative)), data);
		}

		[HttpPut("{id:int}")]
		public IActionResult Update(int id, UpdateCustomerDto customer)
		{
			// TODO: validate input and return appropriate status code

			var done = _customerService.UpdateCustomer(id, customer);
			if (done < 0)
			{
				return BadRequest();
			}

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public IActionResult Delete(int id)
		{
			// TODO: validate input and return appropriate status code

			var done = _customerService.DeleteCustomer(id);
			if (done < 0)
			{
				return BadRequest();
			}

			return NoContent();
		}
	}
}