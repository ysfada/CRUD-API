using Application.Customers.Model;
using Domain.Entities;

namespace Application.Customers.Services
{
	public static class Extensions
	{
		public static CustomerDto AsDto(this Customer customer)
		{
			return new()
			{
				ExternalId = customer.ExternalId,
				Id = customer.Id,
				IsActive = customer.IsActive,
				FirstName = customer.FirstName,
				LastName = customer.LastName
			};
		}

		public static Customer AsCustomer(this CustomerDto customerDto)
		{
			return new()
			{
				Id = customerDto.Id,
				FirstName = customerDto.FirstName,
				LastName = customerDto.LastName,
				IsActive = customerDto.IsActive,
				ExternalId = customerDto.ExternalId
			};
		}

		public static Customer AsCustomer(this CreateCustomerDto createCustomerDto)
		{
			return new()
			{
				FirstName = createCustomerDto.FirstName,
				LastName = createCustomerDto.LastName,
			};
		}
	}
}