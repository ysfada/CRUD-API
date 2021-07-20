using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Model
{
	public record CreateOrderDto
	{
		[Required]
		public int CustomerId { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }
	}
}