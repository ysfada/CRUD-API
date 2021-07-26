using Application.Orders.Model;
using FluentValidation;

namespace Application.Orders.Validators
{
	public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
	{
		public UpdateOrderDtoValidator()
		{
			RuleFor(o => o.CustomerId).NotEmpty();
			RuleFor(o => o.ProductId).NotEmpty();
			RuleFor(o => o.Quantity).GreaterThan(0);
		}
	}
}