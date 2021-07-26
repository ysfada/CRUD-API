using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Products.Model;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Services
{
	public class ProductService : IProductService
	{
		private readonly IRepository<Product> _productRepository;
		private readonly IMapper _mapper;

		public ProductService(IRepository<Product> productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<ProductDto> GetProductAsync(int id, short? isActive = null)
		{
			var q = _productRepository.GetAllNoTracking.Where(p => p.Id == id);
			var product = isActive is null
				? await q.FirstOrDefaultAsync()
				: await q.FirstOrDefaultAsync(p => p.IsActive == 1);

			var productDto = _mapper.Map<ProductDto>(product);
			return productDto;
		}

		public async Task<IEnumerable<ProductDto>> GetProductsAsync(short? isActive = null)
		{
			var q = _productRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(product => product.IsActive == isActive);
			}

			var products = await q.ToListAsync();

			var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
			return productsDto;
		}

		public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
		{
			var createProduct = _mapper.Map<Product>(createProductDto);

			var newProduct = _productRepository.InsertWithoutCommit(createProduct);
			await _productRepository.CommitAsync(CancellationToken.None);

			var productsDto = _mapper.Map<ProductDto>(newProduct);
			return productsDto;
		}

		public async Task UpdateProductAsync(ProductDto productDto)
		{
			var product = _mapper.Map<Product>(productDto);

			_productRepository.UpdateWithoutCommit(product);
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