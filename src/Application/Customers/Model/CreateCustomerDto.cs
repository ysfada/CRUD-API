using System.ComponentModel.DataAnnotations;

namespace Application.Customers.Model
{
	public record CreateCustomerDto
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}