using Application.Customers.Model;
using Domain.Entities;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Application.Customers.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IRepository<Customer> _customerRepository;

		public CustomerService(IRepository<Customer> customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public CustomerDto GetCustomer(int id)
		{
			var customer = _customerRepository.GetAllNoTracking.FirstOrDefault(c => c.Id == id && c.IsActive == 1);
			return customer?.AsDto();
		}

		public IEnumerable<CustomerDto> GetCustomers(short? isActive)
		{
			var q = _customerRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(customer => customer.IsActive == isActive);
			}
			var customers = q.Select(customer => customer.AsDto());

			return customers.ToList();
		}

		public CustomerDto CreateCustomer(CreateCustomerDto createCustomerDto)
		{
			var newCustomer = _customerRepository.InsertWithoutCommit(createCustomerDto.AsCustomer());

			_customerRepository.Commit();

			return newCustomer.AsDto();
		}

		public void UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
		{
			_customerRepository.Update(updateCustomerDto.AsCustomer(id));
		}

		public void DeleteCustomer(int id)
		{
			_customerRepository.Delete(new Customer
			{
				Id = id,
			});
		}
	}
}