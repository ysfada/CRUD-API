using System.Threading;
using System.Threading.Tasks;
using Application.Products.Model;
using Application.Products.Services;
using MediatR;

namespace Application.Products.Queries
{
	public record GetProductQuery(int Id, short? IsActive = null) : IRequest<ProductDto>;

	public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
	{
		private readonly IProductService _service;

		public GetProductQueryHandler(IProductService service)
		{
			_service = service;
		}

		public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			return await _service.GetProductAsync(request.Id, request.IsActive);
		}
	}
}