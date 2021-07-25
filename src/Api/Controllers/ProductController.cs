using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Products.Commands;
using Application.Products.Model;
using Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Api.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IMediator _mediator;

		public ProductController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ProductDto>> GetProductAsync(int id, short? isActive)
		{
			var data = await _mediator.Send(new GetProductQuery(id, isActive));
			if (data is null)
			{
				return NotFound();
			}

			return Ok(data);
		}

		[HttpGet]
		public async Task<IEnumerable<ProductDto>> GetProductsAsync([FromQuery(Name = "isActive")] short? isActive)
		{
			return await _mediator.Send(new GetProductsQuery(isActive));
		}

		[HttpPost]
		public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
		{
			var product = await _mediator.Send(new CreateProductCommand(createProductDto));

			return CreatedAtAction(nameof(GetProductAsync), new {id = product.Id}, product);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
		{
			var existingProduct = await _mediator.Send(new GetProductQuery(id));

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

			await _mediator.Send(new UpdateProductCommand(updatedProduct));

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteProductAsync(int id)
		{
			var existingProduct = await _mediator.Send(new GetProductQuery(id));

			if (existingProduct is null)
			{
				return NotFound();
			}

			await _mediator.Send(new DeleteProductCommand(id));

			return NoContent();
		}
	}
}