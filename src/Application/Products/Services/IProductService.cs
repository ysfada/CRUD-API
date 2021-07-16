using System.Collections.Generic;
using Application.Products.Model;

namespace Application.Products.Services
{
	public interface IProductService
	{
		ProductDto GetProductById(int id);
		List<ProductDto> GetProducts();
		ProductDto CreateProduct(CreateProductDto product);
		int UpdateProduct(int id, UpdateProductDto product);
		int DeleteProduct(int id);
	}
}