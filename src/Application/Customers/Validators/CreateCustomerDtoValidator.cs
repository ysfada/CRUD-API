using Application.Customers.Model;
using FluentValidation;

namespace Application.Customers.Validators
{
	public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
	{
		public CreateCustomerDtoValidator()
		{
			RuleFor(c => c.FirstName).NotEmpty();
			RuleFor(c => c.LastName).NotEmpty();
		}
	}
}
