namespace Application.Products.Model
{
	public record ProductDto(int Id, string ProductName, decimal Price, string ExternalId, short IsActive);
}