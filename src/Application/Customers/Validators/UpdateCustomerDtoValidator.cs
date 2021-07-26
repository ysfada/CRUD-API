using Application.Customers.Model;
using FluentValidation;

namespace Application.Customers.Validators
{
	public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
	{
		public UpdateCustomerDtoValidator()
		{
			RuleFor(c => c.FirstName).NotEmpty();
			RuleFor(c => c.LastName).NotEmpty();
			RuleFor(c => c.IsActive).InclusiveBetween((short) 0, (short) 1);
		}
	}
}