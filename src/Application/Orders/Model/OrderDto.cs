﻿namespace Application.Orders.Model
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }

		public short IsActive { get; set; }
	}
}