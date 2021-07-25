using System.Threading;
using System.Threading.Tasks;
using Application.Orders.Model;
using Application.Orders.Services;
using MediatR;

namespace Application.Orders.Commands
{
	public record UpdateOrderCommand(OrderDto OrderDto) : IRequest<Unit>;

	public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Unit>
	{
		private readonly IOrderService _service;

		public UpdateOrderHandler(IOrderService service)
		{
			_service = service;
		}

		public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
		{
			await _service.UpdateOrderAsync(request.OrderDto);
			return Unit.Value;
		}
	}
}