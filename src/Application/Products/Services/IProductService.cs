using System.Collections.Generic;
using Application.Products.Model;

namespace Application.Products.Services
{
	public interface IProductService
	{
		ProductDto GetProduct(int id, short? isActive = null);
		IEnumerable<ProductDto> GetProducts(short? isActive = null);
		ProductDto CreateProduct(CreateProductDto createProductDto);
		void UpdateProduct(ProductDto productDto);
		void DeleteProduct(int id);
	}
}