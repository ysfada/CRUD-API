namespace Application.Products.Model
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public string ExternalId { get; set; }

		public short IsActive { get; set; }
	}
}