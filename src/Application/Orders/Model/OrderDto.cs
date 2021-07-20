using Application.Customers.Model;
using Application.Products.Model;

namespace Application.Orders.Model
{
	public record OrderDto
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		public short IsActive { get; set; }

		public CustomerDto Customer { get; set; }
		public ProductDto Product { get; set; }
	}
}