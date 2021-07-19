using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Model
{
	public class CreateOrderDto
	{
		[Required]
		public int CustomerId { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}