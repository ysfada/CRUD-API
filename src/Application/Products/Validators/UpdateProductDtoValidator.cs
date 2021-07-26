using Application.Products.Model;
using FluentValidation;

namespace Application.Products.Validators
{
	public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
	{
		public UpdateProductDtoValidator()
		{
			RuleFor(p => p.ProductName).NotEmpty();
			RuleFor(p => p.Price).NotEmpty();
			RuleFor(p => p.IsActive).InclusiveBetween((short) 0, (short) 1);
		}
	}
}