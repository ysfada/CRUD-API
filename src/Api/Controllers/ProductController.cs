using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Products.Model;
using Microsoft.AspNetCore.Mvc;
using Application.Products.Services;

namespace Api.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ProductDto>> GetProductAsync([FromQuery(Name = "isActive")] short? isActive, int id)
		{
			var data = await _productService.GetProductAsync(id, isActive);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public async Task<IEnumerable<ProductDto>> GetProductsAsync([FromQuery(Name = "isActive")] short? isActive)
		{
			return await _productService.GetProductsAsync(isActive);
		}

		[HttpPost]
		public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
		{
			var product = await _productService.CreateProductAsync(createProductDto);

			return CreatedAtAction(nameof(GetProductAsync), new { id = product.Id }, product);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
		{
			var existingProduct = await _productService.GetProductAsync(id);

			if (existingProduct is null)
			{
				return NotFound();
			}

			var updatedProduct = existingProduct with
			{
				ProductName = updateProductDto.ProductName,
				Price = updateProductDto.Price,
				IsActive = updateProductDto.IsActive,
			};

			await _productService.UpdateProductAsync(updatedProduct);

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteProductAsync(int id)
		{
			var existingProduct = await _productService.GetProductAsync(id);

			if (existingProduct is null)
			{
				return NotFound();
			}

			await _productService.DeleteProductAsync(id);

			return NoContent();
		}
	}
}