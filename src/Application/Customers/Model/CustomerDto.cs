namespace Application.Customers.Model
{
	// Automapperda kullandığımız Data Transfer Object 
	public record CustomerDto(int Id, string FirstName, string LastName, string ExternalId, short IsActive);
}