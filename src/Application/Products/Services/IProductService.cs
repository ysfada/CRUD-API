using System.Collections.Generic;
using Application.Products.Model;

namespace Application.Products.Services
{
	public interface IProductService
	{
		ProductDto GetProduct(int id);
		IEnumerable<ProductDto> GetProducts(short? isActive);
		ProductDto CreateProduct(CreateProductDto createProductDto);
		void UpdateProduct(int id, UpdateProductDto updateProductDto);
		void DeleteProduct(int id);
	}
}