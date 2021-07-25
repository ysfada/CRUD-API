using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Model;
using Application.Customers.Services;
using MediatR;

namespace Application.Customers.Queries
{
	public record GetCustomerQuery(int Id) : IRequest<CustomerDto>;

	public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
	{
		private readonly ICustomerService _service;

		public GetCustomerHandler(ICustomerService service)
		{
			_service = service;
		}

		public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
		{
			return await _service.GetCustomerAsync(request.Id);
		}
	}
}