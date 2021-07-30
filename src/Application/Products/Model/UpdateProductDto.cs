namespace Application.Products.Model
{
	public record UpdateProductDto(string ProductName, decimal Price, short IsActive);
}