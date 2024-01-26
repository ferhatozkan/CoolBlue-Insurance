using FluentValidation;
using Insurance.Api.Presentation.Models.Requests;

namespace Insurance.Api.Presentation.Validators
{
    public class CartInsuranceRequestValidator : AbstractValidator<CartInsuranceRequest>
    {
        public CartInsuranceRequestValidator()
        {
            RuleFor(x => x.CartItems).NotEmpty().NotNull();
            RuleForEach(x => x.CartItems).SetValidator(new CartInsuranceItemValidator());
        }
    }
}
