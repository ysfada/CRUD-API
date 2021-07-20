using Application.Customers.Model;
using Domain.Entities;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IRepository<Customer> _customerRepository;

		public CustomerService(IRepository<Customer> customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<CustomerDto> GetCustomerAsync(int id, short? isActive = null)
		{
			var q = _customerRepository.GetAllNoTracking.Where(p => p.Id == id);
			var customer = isActive is null ? await q.FirstOrDefaultAsync() : await q.FirstOrDefaultAsync(c => c.IsActive == 1);
			return customer?.AsDto();
		}

		public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(short? isActive = null)
		{
			var q = _customerRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(customer => customer.IsActive == isActive);
			}
			var customers = q.Select(customer => customer.AsDto());

			return await customers.ToListAsync();
		}

		public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
		{
			var newCustomer = _customerRepository.InsertWithoutCommit(createCustomerDto.AsCustomer());

			await _customerRepository.CommitAsync(CancellationToken.None);

			return newCustomer.AsDto();
		}

		public async Task UpdateCustomerAsync(CustomerDto customerDto)
		{
			_customerRepository.UpdateWithoutCommit(customerDto.AsCustomer());
			await _customerRepository.CommitAsync(CancellationToken.None);
		}

		public async Task DeleteCustomerAsync(int id)
		{
			await _customerRepository.DeleteAsync(new Customer
			{
				Id = id,
			});
		}
	}
}