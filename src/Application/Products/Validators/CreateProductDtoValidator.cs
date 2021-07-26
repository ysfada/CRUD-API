using Application.Products.Model;
using FluentValidation;

namespace Application.Products.Validators
{
	public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
	{
		public CreateProductDtoValidator()
		{
			RuleFor(p => p.ProductName).NotEmpty();
			RuleFor(p => p.Price).NotEmpty();
		}
	}
}