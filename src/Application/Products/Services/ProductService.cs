using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Products.Model;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Services
{
	public class ProductService : IProductService
	{
		private readonly IRepository<Product> _productRepository;

		public ProductService(IRepository<Product> productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<ProductDto> GetProductAsync(int id, short? isActive = null)
		{
			var q = _productRepository.GetAllNoTracking.Where(p => p.Id == id);
			var product = isActive is null ? await q.FirstOrDefaultAsync() : await q.FirstOrDefaultAsync(p => p.IsActive == 1);
			return product?.AsDto();
		}

		public async Task<IEnumerable<ProductDto>> GetProductsAsync(short? isActive = null)
		{
			var q = _productRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(product => product.IsActive == isActive);
			}
			var products = q.Select(product => product.AsDto());

			return await products.ToListAsync();
		}

		public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
		{
			var newProduct = _productRepository.InsertWithoutCommit(createProductDto.AsProduct());

			await _productRepository.CommitAsync(CancellationToken.None);

			return newProduct.AsDto();
		}

		public async Task UpdateProductAsync(ProductDto productDto)
		{
			_productRepository.UpdateWithoutCommit(productDto.AsProduct());
			await _productRepository.CommitAsync(CancellationToken.None);
		}

		public async Task DeleteProductAsync(int id)
		{
			await _productRepository.DeleteAsync(new Product()
			{
				Id = id,
			});
		}
	}
}