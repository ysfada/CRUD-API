using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Products.Model;

namespace Application.Products.Services
{
	public interface IProductService
	{
		Task<ProductDto> GetProductAsync(int id, short? isActive = null);
		Task<IEnumerable<ProductDto>> GetProductsAsync(short? isActive = null);
		Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
		Task UpdateProductAsync(ProductDto productDto);
		Task DeleteProductAsync(int id);
	}
}