using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Model;
using Application.Customers.Services;
using MediatR;

namespace Application.Customers.Commands
{
	public record CreateCustomerCommand(CreateCustomerDto CreateCustomerDto) : IRequest<CustomerDto>;

	public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
	{
		private readonly ICustomerService _service;

		public CreateCustomerHandler(ICustomerService service)
		{
			_service = service;
		}

		public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
		{
			return await _service.CreateCustomerAsync(request.CreateCustomerDto);
		}
	}
}
