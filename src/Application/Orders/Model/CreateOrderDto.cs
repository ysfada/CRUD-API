namespace Application.Orders.Model
{
	public record CreateOrderDto
	{
		public int CustomerId { get; set; }

		public int ProductId { get; set; }

		public int Quantity { get; set; }
	}
}