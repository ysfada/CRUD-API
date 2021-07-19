using Application.Products.Model;
using Domain.Entities;

namespace Application.Products.Services
{
	public static class Extensions
	{
		public static ProductDto AsDto(this Product product)
		{
			return new()
			{
				Id = product.Id,
				ProductName = product.ProductName,
				Price = product.Price,
				IsActive = product.IsActive,
			};
		}

		public static Product AsProduct(this UpdateProductDto updateProductDto, int id)
		{
			return new()
			{
				Id = id,
				ProductName = updateProductDto.ProductName,
				Price = updateProductDto.Price,
				IsActive = updateProductDto.IsActive,
			};
		}

		public static Product AsProduct(this CreateProductDto createProductDto)
		{
			return new()
			{
				ProductName = createProductDto.ProductName,
				Price = createProductDto.Price,
			};
		}
	}
}
