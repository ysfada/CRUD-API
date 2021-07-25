using System.Threading;
using System.Threading.Tasks;
using Application.Orders.Services;
using MediatR;

namespace Application.Orders.Commands
{
	public record DeleteOrderCommand(int Id) : IRequest<Unit>;

	public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
	{
		private readonly IOrderService _service;

		public DeleteOrderHandler(IOrderService service)
		{
			_service = service;
		}

		public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			await _service.DeleteOrderAsync(request.Id);
			return Unit.Value;
		}
	}
}