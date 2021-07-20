using Domain.Common;

namespace Domain.Entities
{
	public record Product : BaseEntity
	{
		public string ProductName { get; set; }
		public decimal Price { get; set; }
	}
}