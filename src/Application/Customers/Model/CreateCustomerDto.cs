namespace Application.Customers.Model
{
	public record CreateCustomerDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}