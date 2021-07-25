using System.Threading;
using System.Threading.Tasks;
using Application.Products.Services;
using MediatR;

namespace Application.Products.Commands
{
	public record DeleteProductCommand(int Id) : IRequest<Unit>;

	public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
	{
		private readonly IProductService _service;

		public DeleteProductHandler(IProductService service)
		{
			_service = service;
		}

		public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			await _service.DeleteProductAsync(request.Id);
			return Unit.Value;
		}
	}
}