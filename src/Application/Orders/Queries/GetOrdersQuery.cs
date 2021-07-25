using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Orders.Model;
using Application.Orders.Services;
using MediatR;

namespace Application.Orders.Queries
{
	public record GetOrdersQuery(short? IsActive = null) : IRequest<IEnumerable<OrderDto>>;

	public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
	{
		private readonly IOrderService _service;

		public GetOrdersQueryHandler(IOrderService service)
		{
			_service = service;
		}

		public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
		{
			return await _service.GetOrdersAsync(request.IsActive);
		}
	}
}