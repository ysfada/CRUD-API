using System.Threading;
using System.Threading.Tasks;
using Application.Orders.Model;
using Application.Orders.Services;
using MediatR;

namespace Application.Orders.Queries
{
	public record GetOrderQuery(int Id) : IRequest<OrderDto>;

	public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
	{
		private readonly IOrderService _service;

		public GetOrderQueryHandler(IOrderService service)
		{
			_service = service;
		}

		public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
		{
			return await _service.GetOrderAsync(request.Id);
		}
	}
}