using FluentValidation;
using Ordering.Application.Orders.CreateOrder;

namespace Ordering.Application.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(70).WithMessage("{PropertyName} must not be exceed 70 chars");

            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(o => o.CardNumber)
                .CreditCard().When(o => !string.IsNullOrEmpty(o.CardNumber))
                .WithMessage("{PropertyName} must be a valid card number.");

            RuleFor(o => o.Expiration)
                .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$")
                .When(o => !string.IsNullOrEmpty(o.Expiration))
                .WithMessage("{PropertyName} must be in MM/YY format.");

            RuleFor(o => o.Cvv)
                .Matches(@"^\d{3,4}$")
                .When(o => !string.IsNullOrEmpty(o.Cvv))
                .WithMessage("{PropertyName} must be 3 or 4 digits.");
        }
    }
}
