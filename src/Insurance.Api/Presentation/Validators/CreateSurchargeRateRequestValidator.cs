using FluentValidation;
using Insurance.Api.Presentation.Models.Requests;

namespace Insurance.Api.Presentation.Validators
{
    public class CreateSurchargeRateRequestValidator : AbstractValidator<CreateSurchargeRateRequest>
    {
        public CreateSurchargeRateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Rate).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(x => x.ProductTypeId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
