using Application.Customers.Model;
using Domain.Entities;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IRepository<Customer> _customerRepository;
		private readonly IMapper _mapper;

		public CustomerService(IRepository<Customer> customerRepository, IMapper mapper)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}

		public async Task<CustomerDto> GetCustomerAsync(int id, short? isActive = null)
		{
			var q = _customerRepository.GetAllNoTracking.Where(p => p.Id == id);
			var customer = isActive is null
				? await q.FirstOrDefaultAsync()
				: await q.FirstOrDefaultAsync(c => c.IsActive == 1);

			var customerDto = _mapper.Map<CustomerDto>(customer);
			return customerDto;
		}

		public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(short? isActive = null)
		{
			var q = _customerRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(customer => customer.IsActive == isActive);
			}

			var customers = await q.ToListAsync();

			var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);
			return customersDto;
		}

		public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
		{
			var createCustomer = _mapper.Map<Customer>(createCustomerDto);

			var newCustomer = _customerRepository.InsertWithoutCommit(createCustomer);
			await _customerRepository.CommitAsync(CancellationToken.None);

			var customerDto = _mapper.Map<CustomerDto>(newCustomer);
			return customerDto;
		}

		public async Task UpdateCustomerAsync(CustomerDto customerDto)
		{
			var customer = _mapper.Map<Customer>(customerDto);

			_customerRepository.UpdateWithoutCommit(customer);
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