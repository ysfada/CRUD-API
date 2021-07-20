﻿using Application.Customers.Model;
using System.Collections.Generic;

namespace Application.Customers.Services
{
	public interface ICustomerService
	{
		CustomerDto GetCustomer(int id, short? isActive = null);
		IEnumerable<CustomerDto> GetCustomers(short? isActive = null);
		CustomerDto CreateCustomer(CreateCustomerDto createCustomerDto);
		void UpdateCustomer(CustomerDto customerDto);
		void DeleteCustomer(int id);
	}
}