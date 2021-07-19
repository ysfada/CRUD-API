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

		public static Customer AsCustomer(this UpdateCustomerDto updateCustomerDto, int id)
		{
			return new()
			{
				Id = id,
				FirstName = updateCustomerDto.FirstName,
				LastName = updateCustomerDto.LastName,
				IsActive = updateCustomerDto.IsActive,
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
