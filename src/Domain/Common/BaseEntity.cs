using System;

namespace Domain.Common
{
	/// <summary>
	/// Tüm projelerimizde ortak olan default fieldlarımız. Abstarct olarak tanımlıyoruz new yapılarak kullanılmaması için. 
	/// </summary>
	public abstract record BaseEntity
	{
		public int Id { get; set; }

		public int? CreatedBy { get; set; }

		public short IsActive { get; set; } = 1;

		public DateTime CreatedTime { get; set; } = DateTime.Now;

		public DateTime? UpdatedTime { get; set; }

		public int? UpdatedBy { get; set; }
	}
}