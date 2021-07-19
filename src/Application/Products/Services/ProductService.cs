using System.Collections.Generic;
using System.Linq;
using Application.Products.Model;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Application.Products.Services
{
	public class ProductService : IProductService
	{
		private readonly IRepository<Product> _productRepository;

		public ProductService(IRepository<Product> productRepository)
		{
			_productRepository = productRepository;
		}

		public ProductDto GetProduct(int id, short? isActive = null)
		{
			var q = _productRepository.GetAllNoTracking.Where(p => p.Id == id);
			var product = isActive is null ? q.FirstOrDefault() : q.FirstOrDefault(p => p.IsActive == 1);
			return product?.AsDto();
		}

		public IEnumerable<ProductDto> GetProducts(short? isActive = null)
		{
			var q = _productRepository.GetAllNoTracking;
			if (isActive is not null)
			{
				q = q.Where(product => product.IsActive == isActive);
			}
			var products = q.Select(product => product.AsDto());

			return products.ToList();
		}

		public ProductDto CreateProduct(CreateProductDto createProductDto)
		{
			var newProduct = _productRepository.InsertWithoutCommit(createProductDto.AsProduct());

			_productRepository.Commit();

			return newProduct.AsDto();
		}

		public void UpdateProduct(int id, UpdateProductDto updateProductDto)
		{
			_productRepository.Update(updateProductDto.AsProduct(id));
		}

		public void DeleteProduct(int id)
		{
			_productRepository.Delete(new Product()
			{
				Id = id,
			});
		}
	}
}