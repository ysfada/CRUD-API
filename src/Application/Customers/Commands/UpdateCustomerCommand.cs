using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Model;
using Application.Customers.Services;
using MediatR;

namespace Application.Customers.Commands
{
	public record UpdateCustomerCommand(CustomerDto CustomerDto) : IRequest<Unit>;

	public class UpdateCustomer : IRequestHandler<UpdateCustomerCommand, Unit>
	{
		private readonly ICustomerService _service;

		public UpdateCustomer(ICustomerService service)
		{
			_service = service;
		}

		public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
		{
			await _service.UpdateCustomerAsync(request.CustomerDto);
			return Unit.Value;
		}
	}
}