using System.Collections.Generic;
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
		public ActionResult<ProductDto> GetProduct([FromQuery(Name = "isActive")] short? isActive, int id)
		{
			var data = _productService.GetProduct(id, isActive);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public IEnumerable<ProductDto> GetProducts([FromQuery(Name = "isActive")] short? isActive)
		{
			return _productService.GetProducts(isActive);
		}

		[HttpPost]
		public ActionResult<ProductDto> CreateProduct(CreateProductDto createProductDto)
		{
			var product = _productService.CreateProduct(createProductDto);

			return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
		}

		[HttpPut("{id:int}")]
		public IActionResult UpdateProduct(int id, UpdateProductDto updateProductDto)
		{
			var existingProduct = _productService.GetProduct(id);

			if (existingProduct is null)
			{
				return NotFound();
			}

			_productService.UpdateProduct(id, updateProductDto);

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteProduct(int id)
		{
			var existingProduct = _productService.GetProduct(id);

			if (existingProduct is null)
			{
				return NotFound();
			}

			_productService.DeleteProduct(id);

			return NoContent();
		}
	}
}