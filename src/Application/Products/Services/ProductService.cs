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

		public ProductDto GetProductById(int id)
		{
			var product = _productRepository.GetById(id);
			if (product is null)
			{
				return null;
			}

			return new ProductDto()
			{
				Id = product.Id,
				ProductName = product.ProductName,
				Price = product.Price
			};
		}

		public List<ProductDto> GetProducts()
		{
			var products = _productRepository.GetAll.Where(product => product.IsActive == 1).Select(f =>
				new ProductDto()
				{
					Id = f.Id,
					IsActive = f.IsActive,
					ProductName = f.ProductName,
					Price = f.Price
				});
			return products.ToList();
		}

		public ProductDto CreateProduct(CreateProductDto product)
		{
			var newCustomer = _productRepository.InsertWithoutCommit(new Product()
			{
				ProductName = product.ProductName,
				Price = product.Price,
			});

			_productRepository.Commit();

			return new ProductDto()
			{
				Id = newCustomer.Id,
				ProductName = product.ProductName,
				Price = product.Price,
				IsActive = newCustomer.IsActive,
			};
		}

		public int UpdateProduct(int id, UpdateProductDto product)
		{
			return _productRepository.Update(new Product()
			{
				Id = id,
				ProductName = product.ProductName,
				Price = product.Price,
			});
		}

		public int DeleteProduct(int id)
		{
			return _productRepository.Delete(new Product()
			{
				Id = id,
			});
		}
	}
}