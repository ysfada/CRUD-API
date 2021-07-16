﻿namespace Application.Customers.Model
{
	// Automapperda kullandığımız Data Transfer Object 
	public class CustomerDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ExternalId { get; set; }

		public short IsActive { get; set; }
	}
}