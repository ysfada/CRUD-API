using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
	/// <summary>
	/// Tüm entitylerimiz BaseEntity'den türemesi gerekiyor. Aksi  taktirde Repository Patternde kullanamayız. 
	/// </summary>
	public class Customer : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ExternalId { get; set; }

		public ICollection<Order> Orders { get; set; }
	}
}