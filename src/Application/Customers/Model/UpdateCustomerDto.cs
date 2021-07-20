using System.ComponentModel.DataAnnotations;

namespace Application.Customers.Model
{
	public record UpdateCustomerDto
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public short IsActive { get; set; }
	}
}