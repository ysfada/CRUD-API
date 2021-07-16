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

		public CustomerDto GetCustomerById(int id)
		{
			var customer = _customerRepository.GetById(id);
			if (customer is null)
			{
				return null;
			}

			return new CustomerDto()
			{
				Id = customer.Id,
				FirstName = customer.FirstName,
				LastName = customer.LastName
			};
		}

		public List<CustomerDto> GetCustomers()
		{
			// Burada automapper kullanabilirsiniz
			var customers = _customerRepository.GetAll.Select(f => new CustomerDto
			{
				ExternalId = f.ExternalId,
				Id = f.Id,
				IsActive = f.IsActive,
				FirstName = f.FirstName,
				LastName = f.LastName
			});
			return customers.ToList();
		}

		public CustomerDto CreateCustomer(CreateCustomerDto customer)
		{
			var newCustomer = _customerRepository.InsertWithoutCommit(new Customer
			{
				FirstName = customer.FirstName,
				LastName = customer.LastName,
			});

			_customerRepository.Commit();

			return new CustomerDto
			{
				Id = newCustomer.Id,
				FirstName = newCustomer.FirstName,
				LastName = newCustomer.LastName,
				IsActive = newCustomer.IsActive,
			};
		}

		public int UpdateCustomer(int id, UpdateCustomerDto customer)
		{
			return _customerRepository.Update(new Customer
			{
				Id = id,
				FirstName = customer.FirstName,
				LastName = customer.LastName,
			});
		}

		public int DeleteCustomer(int id)
		{
			return _customerRepository.Delete(new Customer
			{
				Id = id,
			});
		}
	}
}