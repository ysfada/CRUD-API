namespace Application.Orders.Model
{
	public record CreateOrderDto(int CustomerId, int ProductId, int Quantity);
}