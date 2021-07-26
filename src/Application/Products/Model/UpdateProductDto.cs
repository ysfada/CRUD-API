namespace Application.Products.Model
{
	public record UpdateProductDto
	{
		public string ProductName { get; set; }

		public decimal Price { get; set; }

		public short IsActive { get; set; }
	}
}