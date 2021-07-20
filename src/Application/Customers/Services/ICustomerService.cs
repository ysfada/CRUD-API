using Application.Customers.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Customers.Services
{
	public interface ICustomerService
	{
		Task<CustomerDto> GetCustomerAsync(int id, short? isActive = null);
		Task<IEnumerable<CustomerDto>> GetCustomersAsync(short? isActive = null);
		Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
		Task UpdateCustomerAsync(CustomerDto customerDto);
		Task DeleteCustomerAsync(int id);
	}
}