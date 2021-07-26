namespace Application.Customers.Model
{
	public record UpdateCustomerDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public short IsActive { get; set; }
	}
}