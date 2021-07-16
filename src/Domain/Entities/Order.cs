using Domain.Common;

namespace Domain.Entities
{
	public class Order : BaseEntity
	{
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		public Customer Customer { get; set; }
		public Product Product { get; set; }
	}
}