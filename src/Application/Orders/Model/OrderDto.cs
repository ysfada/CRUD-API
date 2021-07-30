using Application.Customers.Model;
using Application.Products.Model;

namespace Application.Orders.Model
{
	public record OrderDto(int Id, int CustomerId, int ProductId, int Quantity, short IsActive, CustomerDto Customer,
		ProductDto Product);
}