using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Services;
using MediatR;

namespace Application.Customers.Commands
{
	public record DeleteCustomerCommand(int Id) : IRequest<Unit>;

	public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, Unit>
	{
		private readonly ICustomerService _service;

		public DeleteCustomerHandler(ICustomerService service)
		{
			_service = service;
		}

		public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
		{
			await _service.DeleteCustomerAsync(request.Id);
			return Unit.Value;
		}
	}
}