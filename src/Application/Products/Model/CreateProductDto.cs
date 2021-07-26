namespace Application.Products.Model
{
	public record CreateProductDto
	{
		public string ProductName { get; set; }

		public decimal Price { get; set; }
	}
}