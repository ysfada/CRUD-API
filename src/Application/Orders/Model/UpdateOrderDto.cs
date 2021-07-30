namespace Application.Orders.Model
{
	public record UpdateOrderDto(int CustomerId, int ProductId, int Quantity);
}