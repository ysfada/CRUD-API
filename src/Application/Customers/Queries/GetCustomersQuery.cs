using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Customers.Model;
using Application.Customers.Services;
using MediatR;

namespace Application.Customers.Queries
{
	public record GetCustomersQuery(short? IsActive = null) : IRequest<IEnumerable<CustomerDto>>;

	public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
	{
		private readonly ICustomerService _service;

		public GetCustomersHandler(ICustomerService service)
		{
			_service = service;
		}

		public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request,
			CancellationToken cancellationToken)
		{
			return await _service.GetCustomersAsync(request.IsActive);
		}
	}
}