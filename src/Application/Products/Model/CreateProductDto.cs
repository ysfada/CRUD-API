using System.ComponentModel.DataAnnotations;

namespace Application.Products.Model
{
	public class CreateProductDto
	{
		[Required]
		public string ProductName { get; set; }

		[Required]
		public decimal Price { get; set; }
	}
}