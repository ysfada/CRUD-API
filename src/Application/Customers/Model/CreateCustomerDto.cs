using System.ComponentModel.DataAnnotations;

namespace Application.Customers.Model
{
	public class CreateCustomerDto
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}