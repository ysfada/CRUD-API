﻿using System.ComponentModel.DataAnnotations;

namespace Application.Products.Model
{
	public class UpdateProductDto
	{
		[Required]
		public string ProductName { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public short IsActive { get; set; }
	}
}