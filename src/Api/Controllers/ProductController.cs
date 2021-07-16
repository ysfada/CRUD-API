using System;
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
		public IActionResult Get(int id)
		{
			var data = _productService.GetProductById(id);
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var data = _productService.GetProducts();
			return Ok(data);
		}

		[HttpPost]
		public IActionResult Create(CreateProductDto product)
		{
			// TODO: validate input and return appropriate status code

			var data = _productService.CreateProduct(product);
			if (data is null)
			{
				return BadRequest();
			}

			return Created((new Uri($"{Request.Path}/{data.Id}", UriKind.Relative)), data);
		}

		[HttpPut("{id:int}")]
		public IActionResult Update(int id, UpdateProductDto product)
		{
			// TODO: validate input and return appropriate status code

			var done = _productService.UpdateProduct(id, product);
			if (done < 0)
			{
				return BadRequest();
			}

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public IActionResult Delete(int id)
		{
			// TODO: validate input and return appropriate status code

			var done = _productService.DeleteProduct(id);
			if (done < 0)
			{
				return BadRequest();
			}

			return NoContent();
		}
	}
}