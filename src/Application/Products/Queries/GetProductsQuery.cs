using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Products.Model;
using Application.Products.Services;
using MediatR;

namespace Application.Products.Queries
{
	public record GetProductsQuery(short? IsActive = null) : IRequest<IEnumerable<ProductDto>>;

	public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
	{
		private readonly IProductService _service;

		public GetProductsQueryHandler(IProductService service)
		{
			_service = service;
		}

		public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			return await _service.GetProductsAsync(request.IsActive);
		}
	}
}