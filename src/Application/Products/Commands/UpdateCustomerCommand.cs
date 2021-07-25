using System.Threading;
using System.Threading.Tasks;
using Application.Products.Model;
using Application.Products.Services;
using MediatR;

namespace Application.Products.Commands
{
	public record UpdateProductCommand(ProductDto ProductDto) : IRequest<Unit>;

	public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
	{
		private readonly IProductService _service;

		public UpdateProductHandler(IProductService service)
		{
			_service = service;
		}

		public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			await _service.UpdateProductAsync(request.ProductDto);
			return Unit.Value;
		}
	}
}