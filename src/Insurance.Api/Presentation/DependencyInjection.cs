using FluentValidation;
using FluentValidation.AspNetCore;
using Insurance.Api.Presentation.Filters;
using Insurance.Api.Presentation.Models.Requests;
using Insurance.Api.Presentation.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilterAttribute)));
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<UpdateSurchargeRateRequest>, UpdateSurchargeRateRequestValidator>();
            services.AddScoped<IValidator<CreateSurchargeRateRequest>, CreateSurchargeRateRequestValidator>();
            services.AddScoped<IValidator<CartInsuranceRequest>, CartInsuranceRequestValidator>();
            services.AddScoped<IValidator<CartInsuranceItem>, CartInsuranceItemValidator>();
            return services;
        }
    }
}
