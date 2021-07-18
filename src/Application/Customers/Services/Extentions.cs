using Application.Customers.Model;
using Domain.Entities;

namespace Application.Customers.Services
{
	public static class Extentions
	{
		public static CustomerDto AsDto(this Customer customer)
		{
			return new CustomerDto()
			{
				ExternalId = customer.ExternalId,
				Id = customer.Id,
				IsActive = customer.IsActive,
				FirstName = customer.FirstName,
				LastName = customer.LastName
			};
		}
    }
}
