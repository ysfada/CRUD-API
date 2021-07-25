using System.Threading;
using System.Threading.Tasks;
using Application.Products.Model;
using Application.Products.Services;
using MediatR;

namespace Application.Products.Commands
{
	public record CreateProductCommand(CreateProductDto CreateProductDto) : IRequest<ProductDto>;

	public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
	{
		private readonly IProductService _service;

		public CreateProductHandler(IProductService service)
		{
			_service = service;
		}

		public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			return await _service.CreateProductAsync(request.CreateProductDto);
		}
	}
}