using System.Collections.Generic;
using FluentValidation;
using Insurance.Api.Presentation.Models.Requests;

namespace Insurance.Api.Presentation.Validators
{
    public class CartInsuranceItemValidator : AbstractValidator<CartInsuranceItem>
    {
        public CartInsuranceItemValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
