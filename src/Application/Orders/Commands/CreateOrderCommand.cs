using System.Threading;
using System.Threading.Tasks;
using Application.Orders.Model;
using Application.Orders.Services;
using MediatR;

namespace Application.Orders.Commands
{
	public record CreateOrderCommand(CreateOrderDto CreateOrderDto) : IRequest<OrderDto>;

	public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
	{
		private readonly IOrderService _service;

		public CreateOrderHandler(IOrderService service)
		{
			_service = service;
		}

		public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
			return await _service.CreateOrderAsync(request.CreateOrderDto);
		}
	}
}