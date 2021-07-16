using Application.Customers.Model;
using System.Collections.Generic;

namespace Application.Customers.Services
{
	public interface ICustomerService
	{
		CustomerDto GetCustomerById(int id);
		List<CustomerDto> GetCustomers();
		CustomerDto CreateCustomer(CreateCustomerDto customer);
		int UpdateCustomer(int id, UpdateCustomerDto customer);
		int DeleteCustomer(int id);
	}
}